using UnityEngine;

public class InventoryUI : MonoBehaviour, ICloseable
{
    [SerializeField] InventoryItemSlot[] slots; // 실제 적용될 슬롯 UI
    public ItemType currentType { get; private set; }
    [SerializeField] GoldText goldText;
    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotIndex(i);
        }
    }
    private void Start()
    {
        gameObject.SetActive(false);
        SetAllSlot();
    }
    private void Update()
    {
        if (currentType != ItemType.Useable) return;
        foreach (var slot in slots)
        {
            float fillAmount = CooltimeManager.Instance.GetCooltimeProgress(InventoryData.Instance.GetUseableItemId(slot.slotIndex));
            slot.UpdateCooltimeBar(fillAmount);
        }
    }
    public void ChangeTap(ItemType _type)
    {
        if (currentType == _type) return;
        currentType = _type;
        SetAllSlot();
    }
    public void SetAllSlot()
    {
        foreach (var slot in slots)
        {
            slot.SetSlot(currentType);
            switch (currentType)
            {
                case ItemType.Useable:
                    float fillAmount = CooltimeManager.Instance.GetCooltimeProgress(InventoryData.Instance.GetUseableItemId(slot.slotIndex));
                    slot.UpdateCooltimeBar(fillAmount);
                    break;
                case ItemType.Equipment:
                case ItemType.Misc:
                    slot.ResetCooltimeBar();
                    break;
            }
        }
    }
    public void SetSlot(int _index)
    {
        slots[_index].SetSlot(currentType);
    }
    public void SetGoldText(long _gold)
    {
        goldText.SetText(_gold);
    }

    public void Close()
    {
        ItemInformationUI.Instance.InformationClose();
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
