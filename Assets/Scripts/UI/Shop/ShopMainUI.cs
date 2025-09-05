using System.Collections.Generic;
using UnityEngine;

public class ShopMainUI : MonoBehaviour, ICloseable
{
    List<ShopSlot> slots = new List<ShopSlot>();
    List<ShopItemData> itemData = new List<ShopItemData>();
    [SerializeField] Transform slotParent;
    [SerializeField] ItemBuyUI itemBuyUI;
    public void OpenShop()
    {
        gameObject.SetActive(true);
        UIManager.Instance.OpenUI(this);
        GameManager.Instance.GameModeChange(GameMode.ForcedUIMode);
        SetShop();
    }
    void SetShop()
    {
        itemData = TalkManager.Instance.currentInteractNPCData.ShopDataSO.shopItemDatas;
        ShopSlot temp;
        for (int i = 0; i < itemData.Count; i++)
        {
            temp = PoolManager.Instance.shopSlotPool.Get();
            slots.Add(temp);
            temp.transform.SetParent(slotParent);
            temp.SetSlotData(itemData[i].itemId, itemData[i].price, itemBuyUI);
        }
    }
    public void CloseShop()
    {
        for (int i = 0;i < slots.Count;i++)
        {
            PoolManager.Instance.shopSlotPool.Release(slots[i]);
        }
        slots.Clear();
        gameObject.SetActive(false);
    }

    public void Close()
    {
        UIManager.Instance.CloseUI(this);
        CloseShop();
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
