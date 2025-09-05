using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigSO", menuName = "ScriptableObjects/PoolConfigSO")]
public class PoolConfigSO : ScriptableObject
{
    public string poolName;
    public string poolDataType;
    public int poolCapacity;
    public bool isClassPool;
    public bool hasParent;
    public string parentObjectName;
    public GameObject prefab;
    
}
