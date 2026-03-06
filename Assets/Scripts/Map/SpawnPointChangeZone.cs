using UnityEngine;

public class SpawnPointChangeZone : MonoBehaviour
{
    [SerializeField] RespawnPoint spawnPoint;
    RespawnPoint prevSpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            prevSpawnPoint = GameManager.Instance.spawnPoint.ToSpawnPoint();
            GameManager.Instance.SetSpawnPoint(spawnPoint);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            GameManager.Instance.SetSpawnPoint(prevSpawnPoint);
        }
    }
}
