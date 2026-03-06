using System;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
public struct DropTable
{
    public int itemNumber;
    public int minAmount;
    public int maxAmount;
    public float probability;
    public DropTable(int _itemNumber, int _minAmount, int _maxAmount, float _Probability)
    {
        itemNumber = _itemNumber;
        minAmount = _minAmount;
        maxAmount = _maxAmount;
        probability = _Probability;
    }
}

[CreateAssetMenu(fileName = "MonsterDropTable", menuName = "ScriptableObjects/DropTable", order = 1)]
public class DropTableSO : ScriptableObject
{
    public int id;
    public int exp;
    public int minGold;
    public int maxGold;
    public List<DropTable> dropTables;
    public void Reset()
    {
        id = 0;
        exp = 0;
        minGold = 0;
        maxGold = 0;
        dropTables = null;
    }
    public void SetTable(List<DropTable> _list)
    {
        dropTables = _list;
    }
    public void ResetTable()
    {
        dropTables = null;
    }
}
