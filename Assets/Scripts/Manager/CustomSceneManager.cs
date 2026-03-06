using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomSceneManager : DontDestroySingletonBehaviour<CustomSceneManager>
{
    const string titleSceneName = "TitleScene";
    const string managerSceneName = "ManagerScene";
    const string loadingSceneName = "LoadingScene";
    public string currentMapName { get; private set; } = "";

    [SerializeField] Slider loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image fadeScreenImage;

    public Action onUICloseHandler;
    public Action<Vector3> onPlayerTeleportHandler;
    public Action<bool> onPlayerInvincibleHandler;

    float fadeOutDuration = 0.6f;
    float fadeInDuration = 0.8f;

    public bool isSceneChanged { get; private set; } = false;

    AsyncOperationHandle<SceneInstance> currentSceneHandle;
    MapSceneConfigSO currentMapConfig;
    HashSet<string> currentSceneAssetKeys = new HashSet<string>();

    public async UniTaskVoid LoadManagerScene()
    {
        loadingScreen.SetActive(true);
        await SceneManager.LoadSceneAsync(managerSceneName, LoadSceneMode.Additive).ToUniTask();
        await SceneManager.UnloadSceneAsync(titleSceneName).ToUniTask();
    }

    public async UniTask LoadScene(string _scenename, Vector3 _pos)
    {
        if (isSceneChanged)
        {
            return;
        }
        onUICloseHandler?.Invoke();
        onPlayerInvincibleHandler?.Invoke(true);
        isSceneChanged = true;

        try
        {
            if (currentMapName == _scenename)
            {
                await LoadSameMapAsync(_pos);   
            }
            else
            {
                await LoadOtherMapAsync(_scenename, _pos);
            }
        }
        catch (Exception ex) 
        {
            DevelopUtility.Log(ex);
            await LoadTitleSceneAsync();
        }
        finally
        {
            onPlayerInvincibleHandler?.Invoke(false);
            isSceneChanged = false;
        }
    }
    async UniTask LoadSameMapAsync(Vector3 _pos)
    {
        SoundManager.Instance.FadeOutBGMAsync().Forget();
        await FadeOutScreen();
        onPlayerTeleportHandler?.Invoke(_pos);
        DataManager.Instance.SaveWorld();
        SoundManager.Instance.FadeInBGMAsync().Forget();
        await FadeInScreen();
    }
    async UniTask LoadOtherMapAsync(string _sceneName, Vector3 _pos)
    {

        await PreLoadingAsync();
        await SetLoadSceneConfigAsync(_sceneName);
        await LoadMapSceneAsync(_sceneName);
        await InstantiatePreloadedAssetsAsync();
        await FinishLoadingAsync(_pos);
    }
    async UniTask PreLoadingAsync()
    {
        GameManager.Instance.GameModeChange(GameMode.CutScene);
        SoundManager.Instance.FadeOutBGMAsync().Forget();
        await FadeOutScreen();
        loadingScreen.gameObject.SetActive(true);
        await SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive).ToUniTask();
    }
    async UniTask SetLoadSceneConfigAsync(string _sceneName)
    {
        loadingBar.value = 0f;
        currentMapConfig = await AddressableManager.Instance.LoadAssetAsync<MapSceneConfigSO>($"{_sceneName}SceneConfig", AddressableAssetScope.Global);
        
        if (currentMapConfig == null)
        {
            throw new Exception($"MapConfig Load Failed : {_sceneName}Config");
        }


        currentSceneAssetKeys.Clear();

        foreach(SpawnData spawnData in currentMapConfig.list)
        {
            currentSceneAssetKeys.Add(spawnData.addressableKey);
        }

        int totalCount = currentSceneAssetKeys.Count;
        int loadedCount = 0;

        foreach (var key in currentSceneAssetKeys)
        {
            await AddressableManager.Instance.LoadAssetAsync<GameObject>(key, AddressableAssetScope.Local);

            loadedCount++;

            loadingBar.value = (float)loadedCount / totalCount * 0.4f;
        }
    }
    async UniTask LoadMapSceneAsync(string _sceneName)
    {
        AddressableManager.Instance.ReleaseCurrentSceneAssets();
        
        if (!string.IsNullOrEmpty(currentMapName)) 
        {
            if (currentSceneHandle.IsValid())
            {
                await Addressables.UnloadSceneAsync(currentSceneHandle).ToUniTask();
            }
        }

        currentSceneHandle = Addressables.LoadSceneAsync(_sceneName, LoadSceneMode.Additive, activateOnLoad: false);

        while (!currentSceneHandle.IsDone)
        {
            float progress = Mathf.Clamp01(currentSceneHandle.PercentComplete);

            loadingBar.value = 0.4f + progress * 0.4f;

            await UniTask.Yield();
        }

        if(currentSceneHandle.Status != AsyncOperationStatus.Succeeded)
        {
            throw new Exception($"Scene Load Failed : {_sceneName}");
        }

        await currentSceneHandle.Result.ActivateAsync().ToUniTask();

        currentMapName = _sceneName;

        await UniTask.Yield();
    }
    async UniTask InstantiatePreloadedAssetsAsync()
    {
        int totalCount = currentMapConfig.list.Count;
        int spawnedCount = 0;

        foreach (var spawnData in currentMapConfig.list)
        {
            var obj = await AddressableManager.Instance.InstantiateAsync(spawnData.addressableKey);

            if(obj == null)
            {
                throw new Exception($"Instantiate Failed :{spawnData.addressableKey}");
            }

            var init = obj.GetComponent<ISpawnInitializeable>();

            if (init == null)
            {
                throw new Exception($"ISpawnInitializeable Missing : {spawnData.addressableKey}");
            }

            init.OnSpawn(spawnData.transform);

            spawnedCount++;

            loadingBar.value = 0.8f + ((float)spawnedCount / totalCount) * 0.2f;

            await UniTask.Yield();
        }

        loadingBar.value = 1f;
    }
    async UniTask FinishLoadingAsync(Vector3 _pos)
    {
        await FadeOutScreen();
        loadingScreen.gameObject.SetActive(false);
        await SceneManager.UnloadSceneAsync(loadingSceneName).ToUniTask();

        GameManager.Instance.GameModeChange(GameMode.GamePlay);
        onPlayerTeleportHandler?.Invoke(_pos);
        GraphicsManager.Instance.SetShadow();
        DataManager.Instance.SaveWorld();
        await FadeInScreen();
    }
    async UniTask LoadTitleSceneAsync()
    {
        await FadeOutScreen();
        await SceneManager.UnloadSceneAsync(managerSceneName).ToUniTask();
        AddressableManager.Instance.ReleaseAllAssets();
        await SceneManager.LoadSceneAsync(titleSceneName, LoadSceneMode.Additive).ToUniTask();
        loadingScreen.gameObject.SetActive(false);
        await SceneManager.UnloadSceneAsync(loadingSceneName).ToUniTask();
        await FadeInScreen();
    }
    async UniTask FadeInScreen()
    {
        Color color = fadeScreenImage.color;
        float elapsedTime = 0f;
        float startAlpha = color.a;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeInDuration;

            color.a = Mathf.Lerp(startAlpha, 0f, t);
            fadeScreenImage.color = color;
            await UniTask.Yield();
        }
        color.a = 0f;
        fadeScreenImage.color = color;
    }

    async UniTask FadeOutScreen()
    {
        Color color = fadeScreenImage.color;
        float elapsedTime = 0f;
        float startAlpha = color.a;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeOutDuration;

            color.a = Mathf.Lerp(startAlpha, 1f, t);
            fadeScreenImage.color = color;
            await UniTask.Yield();
        }
        color.a = 1f;
        fadeScreenImage.color = color;
    }
}
