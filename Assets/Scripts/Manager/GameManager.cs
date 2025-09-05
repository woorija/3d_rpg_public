using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.IO;

public class GameManager : SingletonBehaviour<GameManager>, IInputBindable
{
    public GameMode gameMode = GameMode.ControllMode;
    [SerializeField] CinemachineCamera cineCam;
    public static bool playerControllable {  get; private set; }
    string screenshotPath;

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
    }
    private void InputInit()
    {
        BindAllInputActions();
    }
    public void PerformedModeChange(InputAction.CallbackContext context)
    {
        switch (gameMode)
        {
            case GameMode.ControllMode:
                GameModeChange(GameMode.UIMode);
                break;
            case GameMode.UIMode:
                GameModeChange(GameMode.ControllMode);
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
        gameMode = _mode;
        switch(gameMode)
        {
            case GameMode.ControllMode:
            case GameMode.NotControllable:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cineCam.enabled = true;
                break;
            case GameMode.UIMode:
            case GameMode.ForcedUIMode:
                cineCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
    public void ExitForcedUIMode()
    {
        GameModeChange(GameMode.ControllMode);
    }
    public void ChangeUIMode()
    {
        GameModeChange(GameMode.UIMode);
    }
    public void EnableCam()
    {
        cineCam.enabled = true;
    }
    public void CameraTeleport(Transform _player, Vector3 _deltaPos)
    {
        cineCam.OnTargetObjectWarped(_player, _deltaPos);
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
