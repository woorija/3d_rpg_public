using UnityEngine;

public class OnEnableSFXPlayer : MonoBehaviour
{
    [SerializeField] SfxData data;
    private void OnEnable()
    {
        SoundManager.Instance.SfxEnqueue(data, transform.position);
    }
}
