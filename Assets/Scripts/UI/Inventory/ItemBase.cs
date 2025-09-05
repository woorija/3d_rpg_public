using System.Collections.Generic;

public class ItemBase
{
    public int itemId { get; protected set; } // 아이템고유ID
    public string name { get; protected set; } // 아이템 이름
    public string description { get; protected set; } // 아이템 설명
    public int sellPrice { get; protected set; } // 개당 판매 가격
    public int maxAmount { get; protected set; } // 1칸당 아이템 최대 소지 수
    public int curruntAmount { get; protected set; } // 현재 아이템 소지 수
    public ItemBase()
    {

    }
    public ItemBase(int _itemId, string _name, string _description, int _sellPrice, int _maxAmount = 1, int _curruntAmount = 1)
    {
        itemId = _itemId;
        name = _name;
        description = _description;
        sellPrice = _sellPrice;
        maxAmount = _maxAmount;
        curruntAmount = _curruntAmount;
    }
    public void LoadItem(int _amount = 1)
    {
        curruntAmount = _amount;
    }
    public void ChangeAmount(int _amount)
    {
        curruntAmount += _amount;
        if(curruntAmount <= 0 )
        {
            Reset();
        }
    }
    public virtual void Reset()
    {
        itemId = 0;
        name = string.Empty;
        description = string.Empty;
        sellPrice = 0;
        curruntAmount = 0;
        maxAmount = 0;
    }
}

public class EquipmentItem : ItemBase
{
    public int equipType { get; protected set; } // 장비 장착 위치
    public Dictionary<StatusType,int> options { get; protected set; }
    public EquipmentItem()
    {
        options = new Dictionary<StatusType,int>();
    }
    public EquipmentItem(int _itemId, string _name, string _description, int _sellPrice, Dictionary<StatusType,int> _options) : base(_itemId, _name, _description, _sellPrice)
    {
        equipType = _itemId / 1000;
        options = _options;
    }
    public void DeepCopy(EquipmentItem _temp)
    {
        itemId = _temp.itemId;
        name = _temp.name;
        description = _temp.description;
        sellPrice = _temp.sellPrice;
        curruntAmount = _temp.curruntAmount;
        maxAmount = _temp.maxAmount;
        equipType = _temp.equipType;
        options = _temp.options;
    }
    public override void Reset()
    {
        base.Reset();
        equipType = 0;
        options = ItemDataBase.EquipmentItemDB[0].options;
    }
    public void SetEquipType(int _equipType)
    {
        equipType = _equipType;
    }
}
public class UseableItem : ItemBase
{
    public UseableItem()
    {

    }
    public UseableItem(int _itemId, string _name, string _description, int _sellPrice, int _maxCount, int _curruntCount = 0) : base(_itemId, _name, _description, _sellPrice, _maxCount, _curruntCount)
    {
    }
    public void DeepCopy(UseableItem _temp)
    {
        itemId = _temp.itemId;
        name = _temp.name;
        description = _temp.description;
        sellPrice = _temp.sellPrice;
        maxAmount = _temp.maxAmount;
        curruntAmount = _temp.curruntAmount;
    }
    public override void Reset()
    {
        base.Reset();
    }
}
public class MiscItem : ItemBase
{
    public MiscItem()
    {

    }
    public MiscItem(int _itemId, string _name, string _description, int _sellPrice, int _maxCount, int _curruntCount = 0) : base(_itemId, _name, _description, _sellPrice, _maxCount, _curruntCount)
    {

    }
    public void DeepCopy(MiscItem _temp)
    {
        itemId = _temp.itemId;
        name = _temp.name;
        description = _temp.description;
        sellPrice = _temp.sellPrice;
        maxAmount = _temp.maxAmount;
        curruntAmount = _temp.curruntAmount;
    }
    public override void Reset() 
    {
        base.Reset();
    }
}
