using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : SingletonBehaviour<EquipmentData>
{
    /*----------UI정보값------------*/
    [SerializeField] EquipmentUI equipmentUI;
    List<EquipmentItem> equipmentItemList;
    List<int> equipmentItemIds;

    /*--------보조 정보값-------------*/
    static readonly int equipmentCount = System.Enum.GetNames(typeof(EquipmentType)).Length;
    Dictionary<StatusType, int> tempStatusMap;

    /*----------스텟정보값------------*/
    [SerializeField] PlayerStatus status;
    protected override void Awake()
    {
        base.Awake();
        equipmentItemList = new List<EquipmentItem>(equipmentCount);
        for (int i = 0; i < equipmentCount; i++)
        {
            equipmentItemList.Add(new EquipmentItem());
        }
        equipmentItemIds = new List<int>(equipmentCount);
        tempStatusMap = new Dictionary<StatusType, int>();
    }
    public void Init()
    {
        for(int i = 0; i < equipmentCount; i++)
        {
            equipmentItemList[i].Reset();
        }
    }
    public void EquipmentReset(int _index)
    {
        SetEquipmentItem(new EquipmentItem(), _index);
        equipmentItemList[_index].Reset();
    }
    public EquipmentItem GetEquipmentItemData(EquipmentType _type)
    {
        return equipmentItemList[(int)_type];
    }
    public EquipmentItem GetEquipmentItemData(int _type)
    {
        return equipmentItemList[_type];
    }
    public void SetEquipmentItem(EquipmentItem _item, int _index)
    {
        tempStatusMap.Clear();

        var options = equipmentItemList[_index].options;
        foreach (var kvp in options)
        {
            AddOrUpdateStatusMap(kvp.Key, -kvp.Value);
        }

        equipmentItemList[_index] = _item;

        foreach (var kvp in _item.options)
        {
            AddOrUpdateStatusMap(kvp.Key, kvp.Value);
        }

        foreach (var kvp in tempStatusMap)
        {
            status.GetBonusStatus(kvp.Key, kvp.Value);
        }
        
        equipmentUI.SetSlot(_index);
    }
    public int GetEquipmentStatus(StatusType _type)
    {
        return CalcEquipmentStatus(_type);
    }
    private int CalcEquipmentStatus(StatusType _type)
    {
        int tempStatus = 0;
        for(int i = 0; i < equipmentCount; i++)
        {
            if(equipmentItemList[i].options.TryGetValue(_type, out int value))
            {
                tempStatus += value;
            }
        }
        return tempStatus;
    }
    public Dictionary<StatusType,int> GetAllEquipmentStatus()
    {
        tempStatusMap.Clear();

        for(int i = 0;i < equipmentCount; i++)
        {
            foreach(var kvp in equipmentItemList[i].options)
            {
                AddOrUpdateStatusMap(kvp.Key, kvp.Value);
            }
        }

        return tempStatusMap;
    }
    void AddOrUpdateStatusMap(StatusType _key, int _value)
    {
        if(tempStatusMap.TryGetValue(_key, out int existingValue))
        {
            tempStatusMap[_key] = existingValue + _value;
        }
        else
        {
            tempStatusMap[_key] = _value;
        }
    }
    public List<int> SaveEquipmentItems()
    {
        equipmentItemIds.Clear();

        for(int i = 0;i < equipmentCount;i++)
        {
            equipmentItemIds.Add(equipmentItemList[i].itemId);
        }

        return equipmentItemIds;
    }
    public void LoadEquipmentItems(List<int> _data)
    {
        for (int i = 0; i < equipmentCount; i++)
        {
            if (_data[i] != 0)
            {
                equipmentItemList[i].DeepCopy(ItemDataBase.EquipmentItemDB[_data[i]]);
            }
        }
        equipmentUI.SetAllSlot();
    }
}
