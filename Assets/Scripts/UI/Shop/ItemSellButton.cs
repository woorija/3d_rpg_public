using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellButton : MonoBehaviour
{
    public void SellUIOpen()
    {
        if (DragManager.Instance.isClick && DragManager.Instance.dragType.Equals(DragUIType.Inventory))
        {
            DragManager.Instance.SellItem();
        }
    }
}
