using UnityEngine;

public class SpawnPointChanger : MonoBehaviour
{
    [SerializeField] RespawnPoint spawnPoint;
    private void Start()
    {
        GameManager.Instance.SetSpawnPoint(spawnPoint);
    }
}
