using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_TrackingMovement : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        blackBoard.agent.speed = blackBoard.blackBoardData.trackingMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.player.transform.position);

        return BTResult.Success;
    }
}
