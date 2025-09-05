using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] SettingDataSO settingDataSO;
    // 화면 해상도
    [SerializeField] TMP_Dropdown resolutionDropdown;
    // 전체화면 여부
    [SerializeField] Toggle screenmodeToggle;
    // 수직동기화 유무
    [SerializeField] Toggle vSyncToggle;
    // 프레임 제한
    [SerializeField] TMP_Dropdown frameRateDropdown;

    int resolutionIndex;
    FullScreenMode screenMode;

    readonly Dictionary<int, Vector2Int> resolutionMap = new Dictionary<int, Vector2Int>
    {
        {0, new Vector2Int(854,480)},
        {1, new Vector2Int(960,540)},
        {2, new Vector2Int(1280,720)},
        {3, new Vector2Int(1600,900)},
        {4, new Vector2Int(1920,1080)},
        {5, new Vector2Int(2560,1440)},
        {6, new Vector2Int(3840,2160)}
    };

    readonly Dictionary<int, int> frameRateMap = new Dictionary<int, int>
    {
        {0, 30 },
        {1, 60 },
        {2, 120 },
        {3, 144 },
        {4, 240 },
        {5, 0 }
    };

    void SetUIEvents()
    {
        screenmodeToggle.onValueChanged.AddListener((isOn) => screenModeUpdate(isOn));
        resolutionDropdown.onValueChanged.AddListener((value) => ResolutionUpdate(value));
        vSyncToggle.onValueChanged.AddListener((isOn) => VSyncUpdate(isOn));
        frameRateDropdown.onValueChanged.AddListener((value) => FrameRateUpdate(value));
    }
    void screenModeUpdate(bool _isOn)
    {
        screenMode = _isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        settingDataSO.screenMode = _isOn;
        SetResolution();
    }
    void ResolutionUpdate(int _value)
    {
        resolutionIndex = _value;
        settingDataSO.resolution = _value;
        SetResolution();
    }
    void SetResolution()
    {
        if(screenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = screenMode;
            resolutionDropdown.interactable = false;
        }
        else
        {
            Screen.SetResolution(resolutionMap[resolutionIndex].x, resolutionMap[resolutionIndex].y, screenMode);
            resolutionDropdown.interactable = true;
        }
        DataManager.Instance.SaveSetting();
    }
    void FrameRateUpdate(int _value)
    {
        Application.targetFrameRate = frameRateMap[_value];
        settingDataSO.frameRate = _value;
        DataManager.Instance.SaveSetting();
    }
    void VSyncUpdate(bool _isOn)
    {
        QualitySettings.vSyncCount = _isOn ? 1 : 0;
        settingDataSO.vsync = _isOn;
        DataManager.Instance.SaveSetting();
    }
    public void LoadSetting(SettingDataSO _settingData)
    {
        resolutionDropdown.value = _settingData.resolution;
        screenmodeToggle.isOn = _settingData.screenMode;
        if(screenmodeToggle.isOn)
        {
            resolutionDropdown.interactable = false;
        }
        vSyncToggle.isOn = _settingData.vsync;
        frameRateDropdown.value = _settingData.frameRate;

        if(screenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = screenMode;
        }
        else
        {
            Screen.SetResolution(resolutionMap[resolutionIndex].x, resolutionMap[resolutionIndex].y, screenMode);
        }
        Application.targetFrameRate = frameRateMap[frameRateDropdown.value];
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;

        SetUIEvents();
    }
}
