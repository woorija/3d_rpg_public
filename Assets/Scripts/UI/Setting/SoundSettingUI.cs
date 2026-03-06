using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] Slider masterBGMVolumeSlider;
    [SerializeField] Slider masterSFXVolumeSlider;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        var soundManager = SoundManager.Instance;

        masterBGMVolumeSlider.value = soundManager.masterBGMVolume;
        masterSFXVolumeSlider.value = soundManager.masterSFXVolume;

        masterBGMVolumeSlider.onValueChanged.AddListener(soundManager.SetMasterBGMVolume);
        masterSFXVolumeSlider.onValueChanged.AddListener(soundManager.SetMasterSFXVolume);

        soundManager.onBgmVolumeChanged += OnBgmChanged;
        soundManager.onSfxVolumeChanged += OnSfxChanged;
    }

    private void OnDestroy()
    {
        var soundManager = SoundManager.Instance;

        soundManager.onBgmVolumeChanged -= OnBgmChanged;
        soundManager.onSfxVolumeChanged -= OnSfxChanged;
    }

    void OnBgmChanged(float _volume)
    {
        masterBGMVolumeSlider.SetValueWithoutNotify(_volume);
    }
    void OnSfxChanged(float _volume)
    {
        masterSFXVolumeSlider.SetValueWithoutNotify(_volume);
    }
}
