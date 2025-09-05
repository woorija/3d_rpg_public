using UnityEngine;

public class SfxData : SoundData
{
    [field: SerializeField][field: Range(0, 256)] public int priority { get; private set; }
    public Vector3 position { get; private set; }
    public void SetPos(Vector3 _pos)
    {
        position = _pos;
    }
}
