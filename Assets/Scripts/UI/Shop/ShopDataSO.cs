using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItemData
{
    public int itemId;
    public int price;
    public ShopItemData(int _id, int _price)
    {
        itemId = _id;
        price = _price;
    }
}
[CreateAssetMenu(fileName = "ShopDataSO", menuName = "ScriptableObjects/ShopDataSO")]
public class ShopDataSO : ScriptableObject
{
    public List<ShopItemData> shopItemDatas;
    public void SetData(List<ShopItemData> _data)
    {
        shopItemDatas = _data;
    }
}
