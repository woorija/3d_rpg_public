using UnityEngine;

public class Environment : MonoBehaviour, ISpawnInitializeable
{
    public void OnSpawn(TransformData _data)
    {
        transform.SetPositionAndRotation(_data.position, Quaternion.Euler(_data.rotation));
        transform.localScale = _data.scale;
    }
}
