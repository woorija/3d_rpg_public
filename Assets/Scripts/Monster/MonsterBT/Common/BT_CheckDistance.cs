using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckDistance : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    [SerializeField] protected float distance;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        MoveStop();
        if (blackBoard.CheckDistance(distance * distance))
        {
            if (blackBoard.CheckHeightDifference(transform.position.y))
            {
                return BTResult.Success;
            }
        }
        return BTResult.Failure;
    }
    protected virtual void MoveStop()
    {

    }
}
