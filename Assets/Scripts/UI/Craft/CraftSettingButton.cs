using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSettingButton : PoolBehaviour<CraftSettingButton>
{
    [SerializeField] CraftUI craftUI;
    Button button;

    private int craftIndex;
    CraftItemData itemData;
    [SerializeField] TMP_Text text;
    public void SetIndex(int _num)
    {
        craftIndex = _num;
        itemData = CraftDataBase.CraftDB[craftIndex].resultItem;
    }
    public void SetData()
    {
        craftUI = GetComponentInParent<CraftUI>();
        button = GetComponent<Button>();
        SetText();
        button.onClick.AddListener(() => craftUI.SetCraftUI(craftIndex));
    }
    void SetText()
    {
        int id = itemData.itemId;
        if(id >= 300000000)
        {
            text.text = $"{ItemDataBase.MiscItemDB[itemData.itemId].name} * {itemData.itemAmount}";
        }
        else if(id >= 200000000)
        {
            text.text = $"{ItemDataBase.UseableItemDB[itemData.itemId].name} * {itemData.itemAmount}";
        }
        else
        {
            text.text = $"{ItemDataBase.EquipmentItemDB[itemData.itemId].name}";
        }
        text.text = (itemData.itemId / 100000000) switch
        {
            1 => $"{ItemDataBase.EquipmentItemDB[itemData.itemId].name}",
            2 => $"{ItemDataBase.UseableItemDB[itemData.itemId].name} * {itemData.itemAmount}",
            _ => $"{ItemDataBase.MiscItemDB[itemData.itemId].name} * {itemData.itemAmount}"
        };
    }
    public int GetItemId()
    {
        return itemData.itemId;
    }
}
