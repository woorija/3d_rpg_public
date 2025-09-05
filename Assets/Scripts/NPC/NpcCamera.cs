using UnityEngine;
using Unity.Cinemachine;

public class NpcCamera : MonoBehaviour
{
    CinemachineCamera npcCamera;
    private void Awake()
    {
        npcCamera = GetComponent<CinemachineCamera>();
    }
    public void SetCameraPos(Transform npcTransform)
    {
        npcCamera.enabled = true;
        Vector3 pos = npcTransform.position + 2f * npcTransform.forward;
        npcCamera.transform.position = pos;
        npcCamera.LookAt = npcTransform;
    }
    public void CloseNpcCam()
    {
        DevelopUtility.Log("closeCam");
        npcCamera.enabled = false;
    }
}
