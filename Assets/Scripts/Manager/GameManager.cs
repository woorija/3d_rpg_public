using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.IO;
using Cysharp.Threading.Tasks;
using System;

public class GameManager : SingletonBehaviour<GameManager>, IInputBindable
{
    public GameMode gameMode = GameMode.GamePlay;
    [SerializeField] CinemachineCamera cineCam;
    [SerializeField] Skybox mainCamSkybox;
    public static bool playerControllable {  get; private set; }
    string screenshotPath;

    public CurrentSpawnPoint spawnPoint {  get; private set; } = new CurrentSpawnPoint();
    public Action onPlayerRespawn;
    void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControllable = true;
        screenshotPath = $"{Application.dataPath}/Screenshots";
        InputInit();
    }
    private void OnApplicationQuit()
    {
        DataManager.Instance.SavePlayer();
        DataManager.Instance.SaveWorld();
    }
    private void OnDisable()
    {
        UnbindAllInputActions();
    }
    private void InputInit()
    {
        BindAllInputActions();
    }
    public void PerformedModeChange(InputAction.CallbackContext context)
    {
        switch (gameMode)
        {
            case GameMode.GamePlay:
                GameModeChange(GameMode.UI);
                break;
            case GameMode.UI:
                GameModeChange(GameMode.GamePlay);
                break;
        }
    }
    public void PerformedScreenShot(InputAction.CallbackContext context)
    {
        if (!Directory.Exists(screenshotPath))
        {
            Directory.CreateDirectory(screenshotPath);
        }
        string fileName = $"Screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string filePath = Path.Combine(screenshotPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
    }
    public void GameModeChange(GameMode _mode)
    {
        Debug.Log($"적용된 게임모드{_mode}");
        gameMode = _mode;
        switch(gameMode)
        {
            case GameMode.GamePlay:
            case GameMode.CutScene:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cineCam.enabled = true;
                break;
            case GameMode.UI:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameMode.UIForced:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                cineCam.enabled = false;
                break;
        }
    }
    public void ExitUIForcedMode()
    {
        GameModeChange(GameMode.GamePlay);
    }
    public void ChangeUIMode()
    {
        GameModeChange(GameMode.UI);
    }
    public void ChangeUIForcedMode()
    {
        GameModeChange(GameMode.UIForced);
    }
    public void EnableCam()
    {
        cineCam.enabled = true;
    }
    public void PlayerRespawn()
    {
        PlayerRespawnAsync().Forget();
    }
    private async UniTask PlayerRespawnAsync()
    {
        await CustomSceneManager.Instance.LoadScene(spawnPoint.sceneName, spawnPoint.position);
        onPlayerRespawn?.Invoke();
        CustomInputManager.Instance.EnablePlayerActionMap();
    }
    public void CameraTeleport(Transform _player, Vector3 _deltaPos)
    {
        cineCam.OnTargetObjectWarped(_player, _deltaPos);
    }
    public void SetSpawnPoint(RespawnPoint _spawnPoint)
    {
        spawnPoint.SetSpawnPoint(_spawnPoint);
    }
    public void ChangeSkybox(Material _material)
    {
        mainCamSkybox.material = _material;
    }
    public void SetControllable(bool _value)
    {
        playerControllable = _value;
    }

    public void InitInputHandlers()
    {
    }

    public void BindAllInputActions()
    {
        var ManagerAction = CustomInputManager.Instance.Manager;

        ManagerAction.ModeChange.performed += PerformedModeChange;
        ManagerAction.Screenshot.performed += PerformedScreenShot;
    }

    public void UnbindAllInputActions()
    {
        var ManagerAction = CustomInputManager.Instance.Manager;

        ManagerAction.ModeChange.performed -= PerformedModeChange;
        ManagerAction.Screenshot.performed -= PerformedScreenShot;
    }
}
