using UnityEngine;

[CreateAssetMenu(fileName = "SettingDataSO", menuName = "ScriptableObjects/SettingDataSO")]
public class SettingDataSO : ScriptableObject
{
    public int resolution;
    public bool screenMode;
    public bool vsync;
    public int frameRate;
    public int shadowType;
    public int shadowResolution;
    public int textureResolution;
    public int antiAliasing;
    public float bgmVolume;
    public float sfxVolume;
    public void Init()
    {
        resolution = 4;
        screenMode = false;
        vsync = false;
        frameRate = 1;
        shadowType = 1;
        shadowResolution = 2;
        textureResolution = 0;
        antiAliasing = 3;
        bgmVolume = 1f;
        sfxVolume = 1f;
    }
}
