using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }
    float baseVolume;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SetAudio(SoundData _data, float _masterVolume = 1f)
    {
        audioSource.clip = _data.clip;
        baseVolume = _data.volume;
        audioSource.volume = baseVolume * _masterVolume;
    }
    public void SetAudio(SfxData _data, float _masterVolume = 1f)
    {
        audioSource.transform.position = _data.position;
        audioSource.priority = _data.priority;
        audioSource.clip = _data.clip;
        baseVolume = _data.volume;
        audioSource.volume = baseVolume * _masterVolume;
    }
    public void SetVolume(float _masterVolume = 1f)
    {
        audioSource.volume = baseVolume * _masterVolume;
    }
    public void Play()
    {
        audioSource.Play();
    }
    public void PlayOneShot()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
