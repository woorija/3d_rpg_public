using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : SingletonBehaviour<InventoryData>
{
    /*----------UI정보값------------*/
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ItemInformationUI itemInformationUI;
    List<EquipmentItem> equipmentItems;
    List<UseableItem> useableItems;
    List<MiscItem> miscItems;
    public int maxcount { get; private set; } = 60;
    public long gold { get; private set; }
    long maxGold = 999999999999;

    /*------------소비,기타아이템 획득을 위한 보조 정보값-------------*/
    int tempIndex; // 순차탐색을 위한 다음 인덱스
    int EmptyIndex; // 첫번째 빈 아이템칸 인덱스
    int remainingAmount; // 획득,제거해야할 남은 수량

    static readonly Func<ItemBase, int, bool> cachedFindNotFullSlot = FindNotFullSlotIndex;
    static readonly Func<ItemBase, int, bool> cachedFindSlotIndex = FindSlotIndex;
    protected override void Awake()
    {
        base.Awake();
        equipmentItems = new List<EquipmentItem>(maxcount);
        useableItems = new List<UseableItem>(maxcount);
        miscItems = new List<MiscItem>(maxcount);
        for(int i = 0; i < maxcount; i++)
        {
            equipmentItems.Add(new EquipmentItem());
            useableItems.Add(new UseableItem());
            miscItems.Add(new MiscItem());
        }
    }
    public void Init()
    {
        gold = 0;
        inventoryUI.SetAllSlot();
        inventoryUI.SetGoldText(gold);
    }
    #region 정보 받아오기
    public EquipmentItem GetEquipmentItemData(int _index)
    {
        return equipmentItems[_index];
    }
    public int GetEquipmentItemId(int _index)
    {
        return equipmentItems[_index].itemId;
    }
    public UseableItem GetUseableItemData(int _index)
    {
        return useableItems[_index];
    }
    public int GetUseableItemId(int _index)
    {
        return useableItems[_index].itemId;
    }
    public MiscItem GetMiscItemData(int _index)
    {
        return miscItems[_index];
    }
    public int GetMiscItemId(int _index)
    {
        return miscItems[_index].itemId;
    }
    public ItemType GetItemType()
    {
        return inventoryUI.currentType;
    }
    #endregion
    #region 아이템 획득
    public void GetItem(int _id, int _count = 1)
    {
        tempIndex = -1; // 0번칸부터 찾기위한 세팅
        switch (_id/100000000)
        {
            case 1:
                GetEquipmentItem(_id);
                break;
            case 2:
                GetUseableItem(_id, _count);
                break;
            case 3:
                GetMiscItem(_id, _count);
                break;
            default:
                break;
        }
    }
    public void GetEquipmentItem(EquipmentItem _item, int _index)
    {
        equipmentItems[_index] = _item;
        SetEquipmentSlot(_index);
    }
    void GetEquipmentItem(int _id)
    {
        EmptyIndex = CheckItemEmptyIndex(equipmentItems);
        if (EmptyIndex == -1)
        {
            DevelopUtility.Log("장비창꽉참");
            return;
        }
        CopyEquipmentItemData(_id, EmptyIndex);
        SetEquipmentSlot(EmptyIndex);
    }
    public void SetEquipmentSlot(int _index)
    {
        if (inventoryUI.currentType.Equals(ItemType.Equipment))
        {
            inventoryUI.SetSlot(_index);
        }
    }
    private int CheckItemIndex<T>(List<T> _list, int _id) where T : ItemBase
    {
        for(int i = 0; i < _list.Count; i++)
        {
            if (_list[i].itemId == _id)
            {
                return i;
            }
        }
        return -1;
    }
    private int CheckItemEmptyIndex<T>(List<T> _list) where T : ItemBase
    {
        return CheckItemIndex(_list, 0);
    }
    public bool IsEquipmentSlotEmpty()
    {
        EmptyIndex = CheckItemEmptyIndex(equipmentItems);
        if (EmptyIndex == -1) return false;
        return true;
    }
    void CopyEquipmentItemData(int _id, int _index)
    {
        equipmentItems[_index].DeepCopy(ItemDataBase.EquipmentItemDB[_id]);
    }
    void GetUseableItem(int _id, int _count)
    {
        remainingAmount = _count;
        EmptyIndex = CheckItemEmptyIndex(useableItems);
        while (remainingAmount > 0) // 남은 획득할 수량이 남아있을때 반복
        {
            tempIndex = FindSlotIndex(tempIndex + 1, useableItems, _id, cachedFindNotFullSlot);
            if (tempIndex == -1) // 빈칸에 획득할 경우
            {
                if (EmptyIndex == -1) break; // 빈칸이 없으면 종료
                CopyUseableItemData(_id, EmptyIndex);
                if (remainingAmount - useableItems[EmptyIndex].maxAmount < 0)
                {
                    useableItems[EmptyIndex].ChangeAmount(remainingAmount);
                    SetUseableSlot(EmptyIndex);
                    remainingAmount = 0;
                }
                else
                {
                    useableItems[EmptyIndex].ChangeAmount(useableItems[EmptyIndex].maxAmount);
                    SetUseableSlot(EmptyIndex);
                    remainingAmount -= useableItems[EmptyIndex].maxAmount;
                    EmptyIndex = CheckItemEmptyIndex(useableItems);
                }
            }
            else // 이미 해당 아이템이 존재하는 칸이 있는경우
            {
                if (remainingAmount - useableItems[tempIndex].maxAmount + useableItems[tempIndex].curruntAmount < 0)
                {
                    useableItems[tempIndex].ChangeAmount(remainingAmount);
                    remainingAmount = 0;
                }
                else
                {
                    remainingAmount -= useableItems[tempIndex].maxAmount - useableItems[tempIndex].curruntAmount;
                    useableItems[tempIndex].ChangeAmount(useableItems[tempIndex].maxAmount - useableItems[tempIndex].curruntAmount);
                }
                SetUseableSlot(tempIndex);
            }
        }
    }
    public void SetUseableSlot(int _index)
    {
        if (inventoryUI.currentType.Equals(ItemType.Useable))
        {
            inventoryUI.SetSlot(_index);
        }
    }
    void CopyUseableItemData(int _id, int _index)
    {
        useableItems[_index].DeepCopy(ItemDataBase.UseableItemDB[_id]);
    }
    void GetMiscItem(int _id, int _count)
    {
        remainingAmount = _count;
        EmptyIndex = CheckItemEmptyIndex(miscItems);
        while (remainingAmount > 0) // 남은 획득할 수량이 남아있을때 반복
        {
            tempIndex = FindSlotIndex(tempIndex + 1, miscItems, _id, cachedFindNotFullSlot); // 동일 아이템칸 찾기 반복
            if (tempIndex == -1) // 빈칸에 획득할 경우
            {
                if (EmptyIndex == -1) break; // 빈칸이 없으면 종료
                CopyMiscItemData(_id, EmptyIndex);
                if (remainingAmount - miscItems[EmptyIndex].maxAmount < 0)
                {
                    miscItems[EmptyIndex].ChangeAmount(remainingAmount);
                    SetMiscSlot(EmptyIndex);
                    remainingAmount = 0;
                }
                else
                {
                    miscItems[EmptyIndex].ChangeAmount(miscItems[EmptyIndex].maxAmount);
                    SetMiscSlot(EmptyIndex);
                    remainingAmount -= miscItems[EmptyIndex].maxAmount;
                    EmptyIndex = CheckItemEmptyIndex(miscItems);
                }
            }
            else // 이미 해당 아이템이 존재하는 칸이 있는경우
            {
                if (remainingAmount - miscItems[tempIndex].maxAmount + miscItems[tempIndex].curruntAmount < 0)
                {
                    miscItems[tempIndex].ChangeAmount(remainingAmount);
                    SetMiscSlot(tempIndex);
                    remainingAmount = 0;
                }
                else
                {
                    remainingAmount -= miscItems[tempIndex].maxAmount - miscItems[tempIndex].curruntAmount;
                    miscItems[tempIndex].ChangeAmount(miscItems[tempIndex].maxAmount - miscItems[tempIndex].curruntAmount);
                    SetMiscSlot(tempIndex);
                }
            }
        }
    }
    public void SetMiscSlot(int _index)
    {
        if (inventoryUI.currentType.Equals(ItemType.Misc))
        {
            inventoryUI.SetSlot(_index);
        }
    }
    void CopyMiscItemData(int _id, int _index)
    {
        miscItems[_index].DeepCopy(ItemDataBase.MiscItemDB[_id]);
    }
    public void GetGold(long _gold)
    {
        gold += _gold;
        if (gold > maxGold) 
        {
            gold = maxGold; 
        }
        inventoryUI.SetGoldText(gold);
    }
    public static bool FindNotFullSlotIndex(ItemBase _item, int _id)
    {
        return _item.itemId == _id && _item.curruntAmount != _item.maxAmount;
    }
    public static bool FindSlotIndex(ItemBase _item, int _id)
    {
        return _item.itemId == _id;
    }
    public static int FindSlotIndex<T>(int _startIndex, List<T> _list, int _id, Func<ItemBase,int,bool> _func) where T : ItemBase
    {
        for(int i = _startIndex; i < _list.Count; i++)
        {
            if (_func(_list[i], _id))
            {
                return i;
            }
        }
        return -1;
    }
    #endregion
    #region 아이템 제거
    public void RemoveItem(int _id, int _count = 1)
    {
        // 외부에서 미리 해당 카운트 만큼 제거할 수 있는지 먼저 체크하고 사용할 것
        tempIndex = -1; // 0번칸부터 찾기위한 세팅
        switch (_id/100000000)
        {
            case 1:
                RemoveItems(equipmentItems, _id, _count);
                break;
            case 2:
                RemoveItems(useableItems, _id, _count);
                break;
            case 3:
                RemoveItems(miscItems, _id, _count);
                break;
            default:
                break;
        }
    }
    void RemoveItems<T>(List<T> _items,int _id, int _count) where T : ItemBase
    {
        remainingAmount = _count;
        while (remainingAmount > 0)
        {
            tempIndex = FindSlotIndex(tempIndex + 1, _items, _id, cachedFindSlotIndex);
            DevelopUtility.Log(tempIndex + "/" + remainingAmount);
            if (remainingAmount >= _items[tempIndex].maxAmount)
            {
                remainingAmount -= _items[tempIndex].maxAmount;
                _items[tempIndex].ChangeAmount(-_items[tempIndex].maxAmount);
                inventoryUI.SetSlot(tempIndex);
            }
            else
            {
                _items[tempIndex].ChangeAmount(-remainingAmount);
                remainingAmount = 0;
                inventoryUI.SetSlot(tempIndex);
            }
        }
    }
    #endregion
    #region 정보반환
    public int GetItemCount(int _id)
    {
        int count = 0;
        int type = _id / 100000000;
        switch (type)
        {
            case 1:
                count = SumEquipmentItemCount(_id);
                break;
            case 2:
                count = SumUseableItemCount(_id);
                break;
            case 3:
                count = SumMiscItemCount(_id);
                break;
        }
        return count;
    }
    private int SumEquipmentItemCount(int _id)
    {
        int count = 0;
        foreach(EquipmentItem item in equipmentItems)
        {
            if(item.itemId == _id)
            {
                count++;
            }
        }
        return count;
    }
    private int SumUseableItemCount(int _id)
    {
        int count = 0;
        foreach (UseableItem item in useableItems)
        {
            if (item.itemId == _id)
            {
                count += item.curruntAmount;
            }
        }
        return count;
    }
    private int SumMiscItemCount(int _id)
    {
        int count = 0;
        foreach (MiscItem item in miscItems)
        {
            if (item.itemId == _id)
            {
                count += item.curruntAmount;
            }
        }
        return count;
    }
    public int SaveEquipmentSlotId(int _index)
    {
        return equipmentItems[_index].itemId;
    }
    public int SaveUseableSlotId(int _index)
    {
        return useableItems[_index].itemId;
    }
    public int SaveUseableSlotAmount(int _index)
    {
        return useableItems[_index].curruntAmount;
    }
    public int SaveMiscSlotId(int _index)
    {
        return miscItems[_index].itemId;
    }
    public int SaveMiscSlotAmount(int _index)
    {
        return miscItems[_index].curruntAmount;
    }
    public void LoadSlotData(int _index, ItemSlotData _slotData)
    {
        equipmentItems[_index].DeepCopy(ItemDataBase.EquipmentItemDB[_slotData.equipmentSlotId]);
        equipmentItems[_index].LoadItem();
        useableItems[_index].DeepCopy(ItemDataBase.UseableItemDB[_slotData.useableSlotId]);
        useableItems[_index].LoadItem(_slotData.useableSlotAmount);
        miscItems[_index].DeepCopy(ItemDataBase.MiscItemDB[_slotData.miscSlotId]);
        miscItems[_index].LoadItem(_slotData.miscSlotAmount);
    }
    public void LoadGold(long _value)
    {
        GetGold(_value);
    }
    public void LoadUI()
    {
        inventoryUI.SetAllSlot();
    }
    #endregion
    #region 드래그
    public void ChangeSlot(int _startIndex, int _endIndex)
    {
        switch (inventoryUI.currentType)
        {
            case ItemType.Equipment:
                if (equipmentItems[_startIndex].itemId != equipmentItems[_endIndex].itemId)
                {
                    ChangeSlot(equipmentItems, _startIndex, _endIndex);
                }
                break;
            case ItemType.Useable:
                ChangeSlot(useableItems, _startIndex, _endIndex);
                break;
            case ItemType.Misc:
                ChangeSlot(miscItems, _startIndex, _endIndex);
                break;
            default:
                break;
        }
    }
    public void ChangeSlot<T>(List<T> items,int _startIndex,int _endIndex) where T : ItemBase
    {
        if (items[_startIndex].itemId == items[_endIndex].itemId)
        {
            int tempAmount = items[_endIndex].maxAmount - items[_endIndex].curruntAmount;
            items[_startIndex].ChangeAmount(-tempAmount);
            items[_endIndex].ChangeAmount(tempAmount);
        }
        else
        {
            T temp = items[_startIndex];
            items[_startIndex] = items[_endIndex];
            items[_endIndex] = temp;
        }
        inventoryUI.SetSlot(_startIndex);
        inventoryUI.SetSlot(_endIndex);
    }
    #endregion
}
