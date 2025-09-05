using UnityEngine.EventSystems;

public class ShopSlotIcon : ItemSlotBase
{
    ItemBase item;
    protected override void ChangeSlotData(ItemBase _item)
    {
        base.ChangeSlotData(_item);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ItemInformationUI.Instance.SetInformation(item);
    }
    public void SetIcon(ItemBase _item)
    {
        item = _item;
        ChangeSlotData(item);
    }
}
