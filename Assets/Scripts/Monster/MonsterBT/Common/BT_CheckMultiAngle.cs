using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct AngleRange
{
    public float minAngle, maxAngle;
}
public class BT_CheckMultiAngle : BT_ConditionNode
{
    [SerializeField] List<AngleRange> angleList;
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        for (int i = 0; i < angleList.Count; i++)
        {
            if (CustomUtility.CheckAngle(angleList[i].minAngle, angleList[i].maxAngle, BT.transform.forward, transform.position, blackBoard.player.centerPos.position))
            {
                return true;
            }
        }
        return false;
    }
}
