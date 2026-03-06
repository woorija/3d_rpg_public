using UnityEngine;

public class InteractNpc : MonoBehaviour
{
    NpcData data;

    private void Awake()
    {
        data = GetComponent<NpcData>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Layers.Player)
        {
            TalkManager.Instance.SetNpc(data);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            TalkManager.Instance.ResetNpc();
        }
    }
}
