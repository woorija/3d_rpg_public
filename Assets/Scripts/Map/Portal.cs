using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Vector3 teleportPosition;
    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player && !CustomSceneManager.Instance.isSceneChanged)
        {
            await CustomSceneManager.Instance.LoadScene(sceneName, teleportPosition);
        }
    }
}
