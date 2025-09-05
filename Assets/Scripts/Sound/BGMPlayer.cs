using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] SoundData soundData;
    private void Start()
    {
        Debug.Log("PlayBGM");
        SoundManager.Instance.PlayBGM(soundData);
    }
}