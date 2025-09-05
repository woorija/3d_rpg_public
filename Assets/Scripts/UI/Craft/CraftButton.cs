using UnityEngine;

public class CraftButton : MonoBehaviour
{
    CraftData craftData;
    int craftIndex;

    public void SetOnClickButton(int _index)
    {
        craftIndex = _index;
        craftData = CraftDataBase.CraftDB[craftIndex];
    }
    public void OnClick()
    {
        int count = 0;
        for(int i = 0; i < craftData.materialItems.Count; i++)
        {
            if (InventoryData.Instance.GetItemCount(craftData.materialItems[i].itemId) >= craftData.materialItems[i].itemAmount)
            {
                count++;
            }
        }
        if (count == craftData.materialItems.Count)
        {
            InventoryData.Instance.GetItem(craftData.resultItem.itemId, craftData.resultItem.itemAmount);

            for (int i = 0; i < craftData.materialItems.Count; i++)
            {
                InventoryData.Instance.RemoveItem(craftData.materialItems[i].itemId, craftData.materialItems[i].itemAmount);
            }
            DataManager.Instance.SaveInventory();
        }
    }
}
