using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : SingletonBehaviour<AddressableManager>
{
    public Dictionary<string, AsyncOperationHandle> loadedHandles = new Dictionary<string, AsyncOperationHandle>();
    public Dictionary<string, List<GameObject>> instantiatedObjects = new Dictionary<string, List<GameObject>>();
    // 씬 이동시 제거할 핸들 목록
    public HashSet<string> currentSceneLoadedHandles = new HashSet<string>();
    
    public void LoadAsset<T>(string _key, Action<T> _onComplete)
    {
        LoadAssetWithAction(_key, _onComplete).Forget();
    }
    async UniTaskVoid LoadAssetWithAction<T>(string _key, Action<T> _onComplete)
    {
        var result = await LoadAssetAsync<T>(_key);
        _onComplete?.Invoke(result);
    }
    public async UniTask<T> LoadAssetAsync<T>(string _key, CancellationToken cancellationToken = default)
    {
        if (loadedHandles.TryGetValue(_key, out AsyncOperationHandle handle))
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    return (T)handle.Result;
                case AsyncOperationStatus.Failed:
                    loadedHandles.Remove(_key);
                    if (handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }
                    break;
                case AsyncOperationStatus.None:
                default:
                    await handle.WithCancellation(cancellationToken);
                    return HandleCompletedOperation<T>(_key, handle);
            }
        }
        var loadHandle = Addressables.LoadAssetAsync<T>(_key);

        loadedHandles.Add(_key, loadHandle);

        await loadHandle.WithCancellation(cancellationToken);

        return HandleCompletedOperation<T>(_key, loadHandle);
    }
    private T HandleCompletedOperation<T>(string _key, AsyncOperationHandle _handle)
    {
        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            return (T)_handle.Result;
        }
        else
        {
            loadedHandles.Remove(_key);
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
            DevelopUtility.Log($"{_key}:로딩실패");
            throw new Exception($"{_key}로딩이 실패했습니다.");
        }
    }
    public async UniTask<GameObject> InstantiateAsync(string _key)
    {
        GameObject prefab = await LoadAssetAsync<GameObject>(_key);
        if(prefab == null)
        {
            DevelopUtility.Log($"{_key}의 프리팹 로딩 실패");
            return null;
        }

        GameObject instance = Instantiate(prefab);
        if(!instantiatedObjects.TryGetValue(_key, out var list))
        {
            list = new List<GameObject>(8);
            instantiatedObjects[_key] = list;
        }
        list.Add(instance);
        return instance;
    }
    public void ReleaseAsset(string _key)
    {
        if (instantiatedObjects.TryGetValue(_key, out var list))
        {
            foreach(var obj in list)
            {
                if(obj != null)
                {
                    Destroy(obj);
                }
            }
            list.Clear();
            instantiatedObjects.Remove(_key);
        }
        if (loadedHandles.TryGetValue(_key, out AsyncOperationHandle handle))
        {
            DevelopUtility.Log($"Release:{_key}");
            Addressables.Release(handle);
            loadedHandles.Remove(_key);
        }
    }
}
