using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("Data")]
    [SerializeField] SettingDataSO settingDataSO;

    [Header("UI")]
    [SerializeField] Slider masterBGMVolumeSlider;
    [SerializeField] Slider masterSFXVolumeSlider;

    [Header("SoundPlayer")]
    [SerializeField] SoundPlayer mainBGMPlayer;
    [SerializeField] SoundPlayer subBGMPlayer;
    [SerializeField] SoundPlayer[] longSFXPlayer;
    [SerializeField] SoundPlayer[] shortSFXPlayer;
    int shortIndex;
    int longIndex;

    float masterBGMVolume;
    float masterSFXVolume;
    float fadeTime = 1f;
    int currentFadeId = 0; // 페이드인,아웃 식별코드

    List<SfxData> sfxList = new List<SfxData>(32);
    static readonly SfxComparer sfxComparer = new SfxComparer();
    protected override void Awake()
    {
        base.Awake();
        shortIndex = 0;
        longIndex = 0;
    }
    private void LateUpdate()
    {
        if(sfxList.Count > 0)
        {
            ShortSfxPlay();           
        }
    }
    void SetUIEvents()
    {
        masterBGMVolumeSlider.onValueChanged.AddListener(SetMasterBGMVolume);
        masterSFXVolumeSlider.onValueChanged.AddListener(SetMasterSFXVolume);
    }
    void ShortSfxPlay()
    {
        int count = sfxList.Count;

        if(count <= shortSFXPlayer.Length)
        {
            for(int i = 0;i < count; i++)
            {
                PlayOneShotShortSfx(sfxList[i]);
            }
        }
        else
        {
            sfxList.Sort(sfxComparer);
            for(int i = 0; i < shortSFXPlayer.Length; i++)
            {
                PlayOneShotShortSfx(sfxList[i]);
            }
        }
        sfxList.Clear();
    }
    public void SfxEnqueue(SfxData _data, Vector3 _pos)
    {
        _data.SetPos(_pos);
        sfxList.Add(_data);
    }
    void PlayOneShotShortSfx(SfxData _data)
    {
        SoundPlayer soundPlayer = shortSFXPlayer[shortIndex];
        soundPlayer.SetAudio(_data, masterSFXVolume);
        soundPlayer.PlayOneShot();
        shortIndex = (shortIndex + 1) % shortSFXPlayer.Length;
    }
    public void PlayOneShotLongSfx(SfxData _data)
    {
        SoundPlayer soundPlayer = longSFXPlayer[longIndex];
        soundPlayer.SetAudio(_data, masterSFXVolume);
        soundPlayer.PlayOneShot();
        longIndex = (longIndex + 1) % longSFXPlayer.Length;
    }
    public void PlayBGM(SoundData _data)
    {
        currentFadeId++;
        mainBGMPlayer.SetAudio(_data, 0f);
        mainBGMPlayer.Play();

        int fadeId = currentFadeId;
        float targetVolume = _data.volume * masterBGMVolume;
        FadeInBGMAsync(fadeId, targetVolume).Forget();
    }
    public async UniTask FadeOutBGMAsync()
    {
        currentFadeId++;
        int fadeId = currentFadeId;

        float startVolume = mainBGMPlayer.audioSource.volume;
        float elapsedTime = 0f;

        while(elapsedTime < fadeTime && fadeId == currentFadeId)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeTime;
            mainBGMPlayer.SetVolume(Mathf.Lerp(startVolume, 0f, progress));

            await UniTask.Yield();
        }

        if(fadeId == currentFadeId)
        {
            mainBGMPlayer.SetVolume(0f);
        }
    }
    async UniTask FadeInBGMAsync(int _fadeId, float _targetVolume)
    {
        float elapsedTime = 0f;

        while(elapsedTime < fadeTime && _fadeId == currentFadeId)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeTime;
            mainBGMPlayer.SetVolume(Mathf.Lerp(0f, _targetVolume, progress));

            await UniTask.Yield();
        }
        if (_fadeId == currentFadeId)
        {
            mainBGMPlayer.SetVolume(_targetVolume);
        }
    }
    public async UniTask FadeInBGMAsync()
    {
        currentFadeId++;
        int fadeId = currentFadeId;

        float elapsedTime = 0f;
        float targetVolume = masterBGMVolume;
        while (elapsedTime < fadeTime && fadeId == currentFadeId)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeTime;
            mainBGMPlayer.SetVolume(Mathf.Lerp(0f, targetVolume, progress));

            await UniTask.Yield();
        }
        if (fadeId == currentFadeId)
        {
            mainBGMPlayer.SetVolume(targetVolume);
        }
    }
    public void SetMasterBGMVolume(float _value)
    {
        masterBGMVolume = _value;
        mainBGMPlayer.SetVolume(masterBGMVolume);
        subBGMPlayer.SetVolume(masterBGMVolume);
        settingDataSO.bgmVolume = masterBGMVolume;
    }
    public void SetMasterSFXVolume(float _value)
    {
        masterSFXVolume = _value;
        settingDataSO.sfxVolume = masterSFXVolume;
    }
    public void LoadSetting(SettingDataSO _settingData)
    {
        masterBGMVolume = _settingData.bgmVolume;
        masterSFXVolume = _settingData.sfxVolume;
        masterBGMVolumeSlider.value = masterBGMVolume;
        masterSFXVolumeSlider.value = masterSFXVolume;

        SetUIEvents();
    }
}
