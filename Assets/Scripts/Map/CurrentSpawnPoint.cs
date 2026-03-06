using UnityEngine;

public class CurrentSpawnPoint
{
    public string sceneName {  get; private set; }
    public Vector3 position {  get; private set; }
    public void SetSpawnPoint(string _sceneName, Vector3 _position)
    {
        sceneName = _sceneName;
        position = _position;
    }
    public void SetSpawnPoint(RespawnPoint _spawnPoint)
    {
        sceneName = _spawnPoint.sceneName;
        position = _spawnPoint.position;
    }
    public RespawnPoint ToSpawnPoint()
    {
        return new RespawnPoint(sceneName, position);
    }
}
