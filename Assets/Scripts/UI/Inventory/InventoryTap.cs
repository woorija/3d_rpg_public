using UnityEngine;

public class InventoryTap : MonoBehaviour
{
    [SerializeField] ItemType itemType;
    [SerializeField] InventoryUI inventory;
    public void ChangeTap()
    {
        if (!DragManager.Instance.isClick)
        {
            inventory.ChangeTap(itemType);
        }
    }
}
