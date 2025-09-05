using UnityEngine;

public class ItemDeleteButton : MonoBehaviour
{
    public void DeleteUIOpen()
    {
        if (DragManager.Instance.isClick && DragManager.Instance.dragType.Equals(DragUIType.Inventory))
        {
            DragManager.Instance.DeleteItem();
        }
    }
}
