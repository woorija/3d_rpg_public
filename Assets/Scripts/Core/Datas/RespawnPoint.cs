using System;
using UnityEngine;

[Serializable]
public struct RespawnPoint
{
    public string sceneName;
    public Vector3 position;
    public RespawnPoint(string _sceneName, Vector3 _position)
    {
        sceneName = _sceneName;
        position = _position;
    }
}
