using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : DontDestroySingletonBehaviour<AddressableManager>
{
    Dictionary<string, UniTask<AsyncOperationHandle>> loadingGlobalTasks = new Dictionary<string, UniTask<AsyncOperationHandle>>();
    Dictionary<string, UniTask<AsyncOperationHandle>> loadingLocalTasks = new Dictionary<string, UniTask<AsyncOperationHandle>>();
    Dictionary<string, AsyncOperationHandle> loadedGlobalHandles = new Dictionary<string, AsyncOperationHandle>();
    Dictionary<string, AsyncOperationHandle> loadedLocalHandles = new Dictionary<string, AsyncOperationHandle>();
    Dictionary<string, List<GameObject>> instantiatedGlobalObjects = new Dictionary<string, List<GameObject>>();
    Dictionary<string, List<GameObject>> instantiatedLocalObjects = new Dictionary<string, List<GameObject>>();
    HashSet<string> pendingReleaseGlobalKeys = new HashSet<string>();
    HashSet<string> pendingReleaseLocalKeys = new HashSet<string>();

    public void LoadAsset<T>(string _key, Action<T> _onComplete)
    {
        if(TryGetLoaded<T>(_key, AddressableAssetScope.Global,  out var asset))
        {
            _onComplete?.Invoke(asset);
            return;
        }
        LoadAssetWithAction(_key, AddressableAssetScope.Global, _onComplete).Forget();
    }
    public void LoadLocalSceneAsset<T>(string _key, Action<T> _onComplete)
    {
        if (TryGetLoaded<T>(_key, AddressableAssetScope.Local, out var asset))
        {
            _onComplete?.Invoke(asset);
            return;
        }
        LoadAssetWithAction(_key, AddressableAssetScope.Local, _onComplete).Forget();
    }
    async UniTaskVoid LoadAssetWithAction<T>(string _key, AddressableAssetScope _scope, Action<T> _onComplete)
    {
        try
        {
            var result = await LoadAssetAsync<T>(_key, _scope);
            _onComplete?.Invoke(result);
        }
        catch (Exception e)
        {
            DevelopUtility.Log($"LoadAssetWithAction 실패 - Key:{_key}, Error:{e.Message}");
        }
    }
    public async UniTask<T> LoadAssetAsync<T>(string _key, AddressableAssetScope _scope, CancellationToken cancellationToken = default)
    {
        var targetHandles = GetHandleDictionary(_scope);
        var targetLoadingTasks = GetLoadingTaskDictionary(_scope);

        if (targetHandles.TryGetValue(_key, out AsyncOperationHandle existingHandle))
        {
            if (existingHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (existingHandle.Result is not T typedResult)
                {
                    throw new InvalidCastException($"Type mismatch: key={_key}, result={existingHandle.Result?.GetType()}, requested={typeof(T)}");
                }
                return typedResult;
            }

            if(existingHandle.Status == AsyncOperationStatus.Failed)
            {
                DevelopUtility.Log($"어드레서블로딩실패 - Key:{_key}");

                targetHandles.Remove(_key);
                if (existingHandle.IsValid())
                {
                    Addressables.Release(existingHandle);
                }
            }

            if(existingHandle.Status == AsyncOperationStatus.None)
            {
                await existingHandle.WithCancellation(cancellationToken);
                if (existingHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (existingHandle.Result is not T typedResult)
                    {
                        throw new InvalidCastException($"Type mismatch: key={_key}");
                    }
                    return typedResult;
                }
                throw new Exception($"{_key} 로딩이 실패했습니다.");
            }
        }

        if(targetLoadingTasks.TryGetValue(_key, out var loadingTask))
        {
            try
            {
                var handle = await loadingTask.WithCancellation(cancellationToken);

                if (handle.Result is not T typedResult)
                {
                    throw new InvalidCastException($"Type mismatch: key={_key}");
                }

                return typedResult;
            }
            catch (OperationCanceledException)
            {
                throw;
            }

        }

        var loadHandle = Addressables.LoadAssetAsync<T>(_key);
        var newLoadingTask = WaitForHandleAsync(loadHandle).Preserve();

        targetLoadingTasks.Add(_key, newLoadingTask);
        targetHandles.Add(_key, loadHandle);

        try
        {
            await loadHandle.WithCancellation(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            targetHandles.Remove(_key);
            targetLoadingTasks.Remove(_key);
            if (loadHandle.IsValid())
            {
                Addressables.Release(loadHandle);
            }
            throw;
        }
        finally
        {
            targetLoadingTasks.Remove(_key);
        }

        if(loadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            if (targetHandles.Remove(_key) && loadHandle.IsValid())
            {
                Addressables.Release(loadHandle);
            }

            throw new Exception($"{_key} 로딩이 실패했습니다.");
        }

        var pendingReleaseKeys = GetPendingReleaseKeySet(_scope);
        if (pendingReleaseKeys.Remove(_key))
        {
            DevelopUtility.Log($"{_key}에 대한 릴리즈 확인 및 실행");
            ReleaseAssetInternal(_key, targetHandles, GetInstantiatedObjectsDictionary(_scope));
            return default;
        }

        if (loadHandle.Result is not T result)
        {
            throw new InvalidCastException($"Type mismatch: key={_key}");
        }

        return result;
    }

    private async UniTask<AsyncOperationHandle> WaitForHandleAsync(AsyncOperationHandle _handle)
    {
        await _handle;
        if (_handle.Status != AsyncOperationStatus.Succeeded)
        {
            throw new Exception($"Handle 로딩 실패");
        }
        return _handle;
    }

    public async UniTask<GameObject> InstantiateAsync(string _key)
    {
        GameObject prefab = default;
        if(!TryGetLoaded(_key, AddressableAssetScope.Local, out prefab))
        {
            prefab = await LoadAssetAsync<GameObject>(_key, AddressableAssetScope.Local);
        }

        if(prefab == null)
        {
            DevelopUtility.Log($"{_key}의 프리팹 로딩 실패");
            return null;
        }

        GameObject instance = Instantiate(prefab);
        if(!instantiatedLocalObjects.TryGetValue(_key, out var list))
        {
            list = new List<GameObject>(8);
            instantiatedLocalObjects[_key] = list;
        }
        list.Add(instance);
        return instance;
    }
    public async UniTask<GameObject> InstantiateGlobalAsync(string _key)
    {
        GameObject prefab = default;
        if (!TryGetLoaded(_key, AddressableAssetScope.Global, out prefab))
        {
            prefab = await LoadAssetAsync<GameObject>(_key, AddressableAssetScope.Global);
        }

        if (prefab == null)
        {
            DevelopUtility.Log($"{_key}의 프리팹 로딩 실패");
            return null;
        }

        GameObject instance = Instantiate(prefab);
        if (!instantiatedGlobalObjects.TryGetValue(_key, out var list))
        {
            list = new List<GameObject>(8);
            instantiatedGlobalObjects[_key] = list;
        }
        list.Add(instance);
        return instance;
    }
    public void ReleaseCurrentSceneAssets()
    {
        ReleaseAssets(loadedLocalHandles, instantiatedLocalObjects, pendingReleaseLocalKeys);
    }
    public void ReleaseGlobalAssets()
    {
        ReleaseAssets(loadedGlobalHandles, instantiatedGlobalObjects, pendingReleaseGlobalKeys);
    }
    public void ReleaseAllAssets()
    {
        ReleaseCurrentSceneAssets();
        ReleaseGlobalAssets();
    }
    public void ReleaseAsset(string _key)
    {
        Dictionary<string, AsyncOperationHandle> targetHandles;
        Dictionary<string, List<GameObject>> targetInstantiatedObjects;
        HashSet<string> pendingReleaseKeys;

        if (loadedGlobalHandles.ContainsKey(_key) || loadingGlobalTasks.ContainsKey(_key))
        {
            targetHandles = loadedGlobalHandles;
            targetInstantiatedObjects = instantiatedGlobalObjects;
            pendingReleaseKeys = pendingReleaseGlobalKeys;
        }
        else if(loadedLocalHandles.ContainsKey(_key) || loadingLocalTasks.ContainsKey(_key)) 
        {
            targetHandles = loadedLocalHandles;
            targetInstantiatedObjects = instantiatedLocalObjects;
            pendingReleaseKeys = pendingReleaseLocalKeys;
        }
        else
        {
            DevelopUtility.Log($"{_key}키가 없음");
            return;
        }

        if (loadingGlobalTasks.ContainsKey(_key) || loadingLocalTasks.ContainsKey(_key))
        {
            pendingReleaseKeys.Add(_key);
            return;
        }

        ReleaseAssetInternal(_key, targetHandles, targetInstantiatedObjects);
    }
    void ReleaseAssetInternal(string _key, Dictionary<string, AsyncOperationHandle> _targetHandles, Dictionary<string, List<GameObject>> _targetInstantiatedObjects)
    {
        if (_targetInstantiatedObjects.TryGetValue(_key, out var list))
        {
            foreach (var obj in list)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            _targetInstantiatedObjects.Remove(_key);
        }
        if (_targetHandles.TryGetValue(_key, out AsyncOperationHandle handle))
        {
            DevelopUtility.Log($"Release:{_key}");
            Addressables.Release(handle);
            _targetHandles.Remove(_key);
        }
    }
    public void ReleaseAssets(Dictionary<string, AsyncOperationHandle> targetHandles, Dictionary<string, List<GameObject>> targetInstantiatedObjects, HashSet<string> pendingReleaseKeys)
    {
        foreach(var kvp in targetInstantiatedObjects)
        {
            foreach(GameObject obj in kvp.Value)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            kvp.Value.Clear();
        }
        targetInstantiatedObjects?.Clear();

        foreach(var kvp in targetHandles)
        {
            if (loadingGlobalTasks.ContainsKey(kvp.Key) || loadingLocalTasks.ContainsKey(kvp.Key))
            {
                pendingReleaseKeys.Add(kvp.Key);
            }
            if (kvp.Value.IsValid())
            {
                Addressables.Release(kvp.Value);
            }
        }
        targetHandles.Clear();
    }
    bool TryGetLoaded<T>(string _key, AddressableAssetScope _scope, out T asset)
    {
        asset = default;

        var targetHandles = GetHandleDictionary(_scope);

        if (!targetHandles.TryGetValue(_key, out var handle)) return false;
        if (!handle.IsValid()) return false;
        if (handle.Status != AsyncOperationStatus.Succeeded) return false;

        if(handle.Result is not T typedResult)
        {
            DevelopUtility.Log($"Type mismatch: key={_key}, result={handle.Result?.GetType()}, requested={typeof(T)}");
            return false;
        }
        
        asset = typedResult;
        return true;
    }
    Dictionary<string, AsyncOperationHandle> GetHandleDictionary(AddressableAssetScope _scope)
    {
        switch (_scope)
        {
            case AddressableAssetScope.Global:
                return loadedGlobalHandles;

            case AddressableAssetScope.Local:
                return loadedLocalHandles;

            default:
                throw new ArgumentOutOfRangeException(nameof(_scope), _scope, null);
        }
    }
    Dictionary<string, UniTask<AsyncOperationHandle>> GetLoadingTaskDictionary(AddressableAssetScope _scope)
    {
        switch (_scope)
        {
            case AddressableAssetScope.Global:
                return loadingGlobalTasks;

            case AddressableAssetScope.Local:
                return loadingLocalTasks;

            default:
                throw new ArgumentOutOfRangeException(nameof(_scope), _scope, null);
        }
    }
    HashSet<string> GetPendingReleaseKeySet(AddressableAssetScope _scope)
    {
        switch (_scope)
        {
            case AddressableAssetScope.Global:
                return pendingReleaseGlobalKeys;

            case AddressableAssetScope.Local:
                return pendingReleaseLocalKeys;

            default:
                throw new ArgumentOutOfRangeException(nameof(_scope), _scope, null);
        }
    }
    Dictionary<string, List<GameObject>> GetInstantiatedObjectsDictionary(AddressableAssetScope _scope)
    {
        switch (_scope)
        {
            case AddressableAssetScope.Global:
                return instantiatedGlobalObjects;

            case AddressableAssetScope.Local:
                return instantiatedLocalObjects;

            default:
                throw new ArgumentOutOfRangeException(nameof(_scope), _scope, null);
        }
    }
}
