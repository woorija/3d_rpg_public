using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : PoolBehaviour<ShopSlot>, IPointerClickHandler
{
    [SerializeField] ShopSlotIcon icon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text priceText;
    ItemBuyUI itemBuyUI;

    ItemBase item;
    int price;

    public void SetSlotData(int _id, int _price,ItemBuyUI _ui)
    {
        GetItemInfor(_id);
        price = _price;
        itemBuyUI = _ui;
        priceText.text = $"{price}G";
    }
    private void GetItemInfor(int _id)
    {
        if(_id >= 300000000)
        {
            item = ItemDataBase.MiscItemDB[_id];
        }
        else if(_id >= 200000000)
        {
            item = ItemDataBase.UseableItemDB[_id];
        }
        else
        {
            item = ItemDataBase.EquipmentItemDB[_id];
        }
        nameText.text = item.name;
        icon.SetIcon(item);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryData.Instance.gold >= price)
        {
            itemBuyUI.SetItem(item, price);
        }
        else
        {
            itemBuyUI.FailUIOpen();
        }
    }
}
