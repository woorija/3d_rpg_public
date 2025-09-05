using UnityEngine;

public class NpcData : MonoBehaviour
{
    [field: SerializeField] public int npcId { get; private set; }
    [field: SerializeField] public int npcType { get; private set; }
    [field: SerializeField] public  Transform camLookAtTransform {  get; private set; }
    [field: SerializeField] public bool hasQuest { get; private set; }
    [field: SerializeField] public bool hasCraft { get; private set; }
    [field: SerializeField] public TalkDataSO talkDataSO { get; private set; }
    [field: SerializeField] public ShopDataSO ShopDataSO { get; private set; }
    public TeleportData teleportData { get; private set; }
    private void Awake()
    {
        teleportData = GetComponent<TeleportData>();
    }

#if UNITY_EDITOR
    public void SetTalkSO(TalkDataSO _so)
    {
        talkDataSO = _so;
    }
    public void SetShopSO(ShopDataSO _so)
    {
        ShopDataSO = _so;
    }
    public void SetCamLookAtTransform(Transform _camLookAtTransform)
    {
        camLookAtTransform = _camLookAtTransform;
    }
#endif
}
