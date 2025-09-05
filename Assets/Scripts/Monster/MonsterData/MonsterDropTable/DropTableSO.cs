using System;
using UnityEngine;

[Serializable]
public struct DropTable
{
    public int itemNumber;
    public int minAmount;
    public int maxAmount;
    public float probability;
}

[CreateAssetMenu(fileName = "MonsterDropTable", menuName = "ScriptableObjects/DropTable", order = 1)]
public class DropTableSO : ScriptableObject
{
    public int exp;
    public int minGold;
    public int maxGold;
    public DropTable[] dropTables;
    public void Reset()
    {
        exp = 0;
        minGold = 0;
        maxGold = 0;
        dropTables = null;
    }
}
