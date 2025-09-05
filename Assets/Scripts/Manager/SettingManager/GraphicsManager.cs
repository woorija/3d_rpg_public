using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GraphicsManager : SingletonBehaviour<GraphicsManager>
{
    UniversalRenderPipelineAsset urpAsset;
    [SerializeField] SettingDataSO settingDataSO;
    // 그림자
    [SerializeField] Toggle shadowNoneToggle, shadowHardToggle, shadowSoftToggle;
    // 그림자 해상도
    [SerializeField] Toggle shadowResolutionLowToggle, shadowResolutionMiddleToggle, shadowResolutionHighToggle;
    // 텍스쳐 해상도
    [SerializeField] Toggle textureResolutionLowToggle, textureResolutionMiddleToggle, textureResolutionHighToggle, textureResolutionUltraToggle;
    // 안티앨리어싱 배율
    [SerializeField] Toggle antiAliasingScaleNoneToggle, antiAliasingScale2Toggle, antiAliasingScale4Toggle, antiAliasingScale8Toggle;

    

    public Action<int> onShadowSettingChanged;

    private int shadowSetting = 1;
    public int ShadowSetting
    {
        get
        {
            return shadowSetting;
        }
        set
        {
            shadowSetting = value;
            settingDataSO.shadowType = value;
            SetShadow();
        }
    }
    void SetUIEvent()
    {
        SetShadowToggles();
        SetShadowResolutionToggles();
        SetTextureResolutionToggles();
        SetAntiAliasingScaleToggles();
    }
    void OnDestroy()
    {
        onShadowSettingChanged = null;
        shadowNoneToggle.onValueChanged.RemoveAllListeners();
        shadowHardToggle.onValueChanged.RemoveAllListeners();
        shadowSoftToggle.onValueChanged.RemoveAllListeners();
        shadowResolutionLowToggle.onValueChanged.RemoveAllListeners();
        shadowResolutionMiddleToggle.onValueChanged.RemoveAllListeners();
        shadowResolutionHighToggle.onValueChanged.RemoveAllListeners();
        textureResolutionLowToggle.onValueChanged.RemoveAllListeners();
        textureResolutionMiddleToggle.onValueChanged.RemoveAllListeners();
        textureResolutionHighToggle.onValueChanged.RemoveAllListeners();
        textureResolutionUltraToggle.onValueChanged.RemoveAllListeners();
        antiAliasingScaleNoneToggle.onValueChanged.RemoveAllListeners();
        antiAliasingScale2Toggle.onValueChanged.RemoveAllListeners();
        antiAliasingScale4Toggle.onValueChanged.RemoveAllListeners();
        antiAliasingScale8Toggle.onValueChanged.RemoveAllListeners();
    }
    void AddToggleListener(Toggle _toggle, Action _action)
    {
        _toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                _action?.Invoke();
                DataManager.Instance.SaveSetting();
            }
        });
    }
    void SetShadowToggles()
    {
        AddToggleListener(shadowNoneToggle, () => ShadowSetting = 0);
        AddToggleListener(shadowHardToggle, () => ShadowSetting = 1);
        AddToggleListener(shadowSoftToggle, () => ShadowSetting = 2);
    }
    public void SetShadow()
    {
        onShadowSettingChanged?.Invoke(ShadowSetting);
    }
    void SetShadowResolutionToggles()
    {
        AddToggleListener(shadowResolutionLowToggle, () =>
        {
            urpAsset.mainLightShadowmapResolution = 512;
            settingDataSO.shadowResolution = 0;
        });
        AddToggleListener(shadowResolutionMiddleToggle, () =>
        {
            urpAsset.mainLightShadowmapResolution = 2048;
            settingDataSO.shadowResolution = 1;
            
        });
        AddToggleListener(shadowResolutionHighToggle, () =>
        { 
            urpAsset.mainLightShadowmapResolution = 4096; 
            settingDataSO.shadowResolution = 2;
        });
    }

    void SetTextureResolutionToggles()
    {
        AddToggleListener(textureResolutionLowToggle, () =>
        {
            QualitySettings.globalTextureMipmapLimit = 3;
            settingDataSO.textureResolution = 0;
        });
        AddToggleListener(textureResolutionMiddleToggle, () =>
        {
            QualitySettings.globalTextureMipmapLimit = 2;
            settingDataSO.textureResolution = 1;
        });
        AddToggleListener(textureResolutionHighToggle, () =>
        {
            QualitySettings.globalTextureMipmapLimit = 1;
            settingDataSO.textureResolution = 2;
        });
        AddToggleListener(textureResolutionUltraToggle, () =>
        {
            QualitySettings.globalTextureMipmapLimit = 0;
            settingDataSO.textureResolution = 3;
        });
    }

    void SetAntiAliasingScaleToggles()
    {
        AddToggleListener(antiAliasingScaleNoneToggle, () =>
        {
            urpAsset.msaaSampleCount = 1;
            settingDataSO.antiAliasing = 0;
        });
        AddToggleListener(antiAliasingScale2Toggle, () =>
        {
            urpAsset.msaaSampleCount = 2;
            settingDataSO.antiAliasing = 1;
        });
        AddToggleListener(antiAliasingScale4Toggle, () =>
        {
            urpAsset.msaaSampleCount = 4;
            settingDataSO.antiAliasing = 2;
        });
        AddToggleListener(antiAliasingScale8Toggle, () =>
        {
            urpAsset.msaaSampleCount = 8;
            settingDataSO.antiAliasing = 3;
        });
    }
    public void LoadSetting(SettingDataSO _settingData)
    {
        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;

        shadowNoneToggle.SetIsOnWithoutNotify(false);
        shadowHardToggle.SetIsOnWithoutNotify(false);
        shadowSoftToggle.SetIsOnWithoutNotify(false);
        shadowResolutionLowToggle.SetIsOnWithoutNotify(false);
        shadowResolutionMiddleToggle.SetIsOnWithoutNotify(false);
        shadowResolutionHighToggle.SetIsOnWithoutNotify(false);
        textureResolutionLowToggle.SetIsOnWithoutNotify(false);
        textureResolutionMiddleToggle.SetIsOnWithoutNotify(false);
        textureResolutionHighToggle.SetIsOnWithoutNotify(false);
        textureResolutionUltraToggle.SetIsOnWithoutNotify(false);
        antiAliasingScaleNoneToggle.SetIsOnWithoutNotify(false);
        antiAliasingScale2Toggle.SetIsOnWithoutNotify(false);
        antiAliasingScale4Toggle.SetIsOnWithoutNotify(false);
        antiAliasingScale8Toggle.SetIsOnWithoutNotify(false);

        switch (_settingData.shadowType)
        {
            case 0:
                shadowNoneToggle.isOn = true;
                ShadowSetting = 0;
                break;
            case 1:
                shadowHardToggle.isOn = true;
                ShadowSetting = 1;
                break;
            case 2:
                shadowSoftToggle.isOn = true;
                ShadowSetting = 2;
                break;
        }

        switch(_settingData.shadowResolution)
        {
            case 0:
                shadowResolutionLowToggle.isOn = true;
                urpAsset.mainLightShadowmapResolution = 512;
                break;
            case 1:
                shadowResolutionMiddleToggle.isOn = true;
                urpAsset.mainLightShadowmapResolution = 2048;
                break;
            case 2:
                shadowResolutionHighToggle.isOn = true;
                urpAsset.mainLightShadowmapResolution = 4096;
                break;
        }

        switch(_settingData.textureResolution)
        {
            case 0:
                textureResolutionLowToggle.isOn = true;
                QualitySettings.globalTextureMipmapLimit = 3;
                break;
            case 1:
                textureResolutionMiddleToggle.isOn = true;
                QualitySettings.globalTextureMipmapLimit = 2;
                break;
            case 2:
                textureResolutionHighToggle.isOn = true;
                QualitySettings.globalTextureMipmapLimit = 1;
                break;
            case 3:
                textureResolutionUltraToggle.isOn = true;
                QualitySettings.globalTextureMipmapLimit = 0;
                break;
        }
        switch(_settingData.antiAliasing)
        {
            case 0:
                antiAliasingScaleNoneToggle.isOn = true;
                urpAsset.msaaSampleCount = 1;
                break;
            case 1:
                antiAliasingScale2Toggle.isOn = true;
                urpAsset.msaaSampleCount = 2;
                break;
            case 2:
                antiAliasingScale4Toggle.isOn = true;
                urpAsset.msaaSampleCount = 4;
                break;
            case 3:
                antiAliasingScale8Toggle.isOn = true;
                urpAsset.msaaSampleCount = 8;
                break;
        }

        SetUIEvent();
    }
}
