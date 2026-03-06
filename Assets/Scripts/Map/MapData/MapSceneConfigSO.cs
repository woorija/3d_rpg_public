using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSceneConfigSO", menuName = "ScriptableObjects/MapSceneConfigSO")]
public class MapSceneConfigSO : ScriptableObject
{
    public List<SpawnData> list;
}
