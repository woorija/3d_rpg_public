using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckTrackingPlayer : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        //중심좌표로 부터 멀어졌을경우를 체크
        if(!CustomUtility.CheckSqrDistance(transform.position, blackBoard.spawnPointCenter, blackBoard.blackBoardData.limitTrackingRange))
        {
            blackBoard.ChangeReturn(true);
            BT.ChangeAnimatorBool(AnimationKey.Move, true);
            return BTResult.Failure;
        }
        if (blackBoard.CheckDistance(blackBoard.blackBoardData.trackingRange * blackBoard.blackBoardData.trackingRange) && blackBoard.CheckHeightDifference(transform.position.y))
        {
            BT.ChangeAnimatorBool(AnimationKey.Move, true);
            blackBoard.agent.stoppingDistance = blackBoard.blackBoardData.normalAttackRange;
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
