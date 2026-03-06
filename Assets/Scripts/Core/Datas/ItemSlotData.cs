public struct ItemSlotData
{
    public int equipmentSlotId;
    public int useableSlotId;
    public int useableSlotAmount;
    public int miscSlotId;
    public int miscSlotAmount;
    public ItemSlotData(int _equipmentSlotId, int _useableSlotId, int _useableSlotAmount, int _miscSlotId, int _miscSlotAmount)
    {
        equipmentSlotId = _equipmentSlotId;
        useableSlotId = _useableSlotId;
        useableSlotAmount = _useableSlotAmount;
        miscSlotId = _miscSlotId;
        miscSlotAmount = _miscSlotAmount;
    }
}