using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomSceneManager : DontDestroySingletonBehaviour<CustomSceneManager>
{
    const string loadingSceneName = "LoadingScene";
    public string currentMapName { get; private set; } = "";

    [SerializeField] Slider loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image fadeScreenImage;

    public UnityAction<Vector3> playerTeleportEvent;

    float fadeOutDuration = 0.6f;
    float fadeInDuration = 0.8f;

    public bool isSceneChanged { get; private set; } = false;
    public async UniTaskVoid LoadManagerScene()
    {
        loadingScreen.SetActive(true);
        await SceneManager.LoadSceneAsync("ManagerScene", LoadSceneMode.Additive).ToUniTask();
        await SceneManager.UnloadSceneAsync("TitleScene").ToUniTask();
    }
    public async UniTask LoadScene(string _scenename, Vector3 _pos)
    {
        UIManager.Instance.CloseAllUI();
        isSceneChanged = true;
        if (currentMapName.Equals(_scenename))
        {
            SoundManager.Instance.FadeOutBGMAsync().Forget();
            await FadeOutScreen();
            playerTeleportEvent.Invoke(_pos);
            DataManager.Instance.SaveWorld();
            SoundManager.Instance.FadeInBGMAsync().Forget();
            await FadeInScreen();
        }
        else
        {
            GameManager.Instance.GameModeChange(GameMode.NotControllable);
            SoundManager.Instance.FadeOutBGMAsync().Forget();
            await FadeOutScreen();
            loadingScreen.gameObject.SetActive(true);
            await SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive).ToUniTask();
            
            if (!currentMapName.Equals(""))
            {
                await SceneManager.UnloadSceneAsync(currentMapName).ToUniTask();
            }
            await FadeInScreen();
            var sceneLoadOperation = SceneManager.LoadSceneAsync(_scenename, LoadSceneMode.Additive);
            currentMapName = _scenename;
            sceneLoadOperation.allowSceneActivation = false;

            while (!sceneLoadOperation.isDone)
            {
                float progress = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f);
                loadingBar.value = progress;

                if (progress >= 1f)
                {
                    sceneLoadOperation.allowSceneActivation = true;
                }

                await UniTask.Yield();
            }
            await FadeOutScreen();
            loadingScreen.gameObject.SetActive(false);
            await SceneManager.UnloadSceneAsync(loadingSceneName).ToUniTask();

            GameManager.Instance.GameModeChange(GameMode.ControllMode);
            playerTeleportEvent.Invoke(_pos);
            GraphicsManager.Instance.SetShadow();
            DataManager.Instance.SaveWorld();
            await FadeInScreen();
        }
        isSceneChanged = false;
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
