using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemSlot : ItemSlotBase
{
    [SerializeField] EquipmentType slotIndex;
    public void SetSlot()
    {
        ItemBase _item = EquipmentData.Instance.GetEquipmentItemData(slotIndex);
        ChangeSlotData(_item);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            if (iconImage.sprite != null)
            {
                ItemInformationUI.Instance.SetInformation(EquipmentData.Instance.GetEquipmentItemData(slotIndex));
            }
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (!DragManager.Instance.isClick)
                {
                    if (iconImage.sprite != null)
                    {
                        DragManager.Instance.DragStart(DragUIType.Equipment, (int)slotIndex, iconImage.sprite);
                    }
                }
                else
                {
                    DragManager.Instance.DragEndToEquipment((int)slotIndex);
                }
                break;
            case PointerEventData.InputButton.Right:
                if (InventoryData.Instance.IsEquipmentSlotEmpty())
                {
                    if (!DragManager.Instance.isClick)
                    {
                        if (iconImage.sprite != null)
                        {
                            InventoryData.Instance.GetItem(EquipmentData.Instance.GetEquipmentItemData(slotIndex).itemId);
                            EquipmentData.Instance.EquipmentReset((int)slotIndex);
                            DataManager.Instance.SaveInventory();
                            DataManager.Instance.SaveEquipment();
                        }
                    }
                }
                break;
        }
                
    }
}
