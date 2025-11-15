using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] Material skybox;
    private void Start()
    {
        GameManager.Instance.ChangeSkybox(skybox);
    }
}
