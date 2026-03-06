using UnityEngine;

public class BT_CheckAngle : BT_ConditionNode
{
    [SerializeField, GetBlackBoardData(typeof(float))] float angle1;
    [SerializeField, GetBlackBoardData(typeof(float))] float angle2 = -1;
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        if (angle2 == -1)
        {
            if (blackBoard.CheckAngle(transform.position,angle1))
            {
                return true;
            }
        }
        else
        {
            if (CustomUtility.CheckAngle(angle1, angle2, BT.transform.forward, transform.position, blackBoard.player.centerPos.position))
            {
                return true;
            }
        }
        return false;
    }
}
