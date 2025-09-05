using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemBuyUI : MonoBehaviour, ICloseable
{
    [SerializeField] NumericInputField field;
    [SerializeField] TMP_Text inforText;
    [SerializeField] BaseCloseableUI buyFailUI;

    ItemBase item;
    int price;

    const string equipBuyInforText = "아이템을 구매하시겠습니까?";
    const string otherBuyInforText = "구매할 갯수를 입력하세요.";
    private void OnEnable()
    {
        CustomInputManager.Instance.UI.ConfirmUI.performed += BuyItem;
    }
    private void OnDisable()
    {
        CustomInputManager.Instance.UI.ConfirmUI.performed -= BuyItem;
    }
    void BuyItem(InputAction.CallbackContext context)
    {
        BuyItem();
    }
    public void BuyItem() // 구매버튼에 등록할것
    {
        if(item.itemId < 200000000)
        {
            BuyEquipmentItem();
        }
        else
        {
            BuyOtherItem();
        }
        DragManager.Instance.DragReset();
    }
    public void SetItem(ItemBase _item, int _price)
    {
        UIManager.Instance.OpenUI(this);
        gameObject.SetActive(true);

        item = _item;
        price = _price;
        if (IsEquipmentItem(item.itemId))
        {
            field.gameObject.SetActive(false);
            inforText.text = equipBuyInforText;
        }
        else
        {
            field.gameObject.SetActive(true);
            inforText.text = otherBuyInforText;
        }
    }
    public bool IsEquipmentItem(int _id)
    {
        return _id >= 100000000 && _id < 200000000;
    }
     
    void BuyEquipmentItem()
    {
        if (InventoryData.Instance.gold >= price)
        {
            InventoryData.Instance.GetItem(item.itemId);
            InventoryData.Instance.GetGold(-price);
            DataManager.Instance.SaveInventory();
            Close();
        }
        else
        {
            FailUIOpen();
        }
    }
    void BuyOtherItem()
    {
        int value = field.GetValue();
        if (value == 0) return;
        if (InventoryData.Instance.gold >= value * price)
        {
            InventoryData.Instance.GetItem(item.itemId, value);
            InventoryData.Instance.GetGold(-value * price);
            DataManager.Instance.SaveInventory();
            Close();
        }
        else
        {
            FailUIOpen();
        }
    }
    public void FailUIOpen()
    {
        Close();
        buyFailUI.Open();
    }

    public void Close()
    {
        UIManager.Instance.CloseUI(this);
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
