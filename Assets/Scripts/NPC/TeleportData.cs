using System;
using UnityEngine;
[Serializable]
public class Teleport
{
    public string UIText;
    public string sceneName;
    public Vector3 teleportPos;
}
public class TeleportData : MonoBehaviour
{
    [field: SerializeField] public Teleport[] teleports { get; private set; }
    public int GetCount()
    {
        return teleports.Length;
    }
}
