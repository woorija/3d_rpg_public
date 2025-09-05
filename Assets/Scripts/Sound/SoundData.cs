using UnityEngine;

public class SoundData : MonoBehaviour
{
    [field: SerializeField] public AudioClip clip { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float volume { get; private set; }
}
