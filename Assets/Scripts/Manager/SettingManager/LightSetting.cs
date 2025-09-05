using UnityEngine;

public class LightSetting : MonoBehaviour
{
    Light baseLight;
    private void Start()
    {
        baseLight = GetComponent<Light>();
        GraphicsManager.Instance.onShadowSettingChanged += ShadowSetting;
    }
    private void OnDestroy()
    {
        GraphicsManager.Instance.onShadowSettingChanged -= ShadowSetting;
    }
    void ShadowSetting(int _value)
    {
        switch (_value)
        {
            case 0:
                baseLight.shadows = LightShadows.None;
                break;
            case 1:
                baseLight.shadows = LightShadows.Hard;
                break;
            case 2:
                baseLight.shadows = LightShadows.Soft;
                break;
        }
    }
}
