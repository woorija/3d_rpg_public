using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BT_IdleMovement : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        blackBoard.agent.stoppingDistance = 0;
        blackBoard.agent.speed = blackBoard.blackBoardData.idleMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.movePoint);
        if (blackBoard.agent.remainingDistance <= 0f) return BTResult.Failure;

        return BTResult.Success;
    }
}
