using UnityEngine;

public class InteractNpc : MonoBehaviour
{
    NpcData data;
    int playerLayer;
    private void Awake()
    {
        data = GetComponent<NpcData>();
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(playerLayer))
        {
            TalkManager.Instance.SetNpc(data);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(playerLayer))
        {
            TalkManager.Instance.ResetNpc();
        }
    }
}
