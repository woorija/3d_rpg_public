using UnityEngine;

public class BossArea : MonoBehaviour, ISpawnInitializeable
{
    [SerializeField] BaseBlackBoard blackBoard;
    public void OnSpawn(TransformData _data)
    {
        transform.SetPositionAndRotation(_data.position, Quaternion.Euler(_data.rotation));
        transform.localScale = _data.scale;

        blackBoard.OnSpawn(_data);
    }
}
