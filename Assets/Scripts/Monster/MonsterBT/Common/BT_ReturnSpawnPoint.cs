using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ReturnSpawnPoint : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        blackBoard.agent.speed = blackBoard.blackBoardData.returnMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.movePoint);
        if (blackBoard.agent.remainingDistance <= blackBoard.agent.stoppingDistance)
        {
            blackBoard.ChangeReturn(false);
            return BTResult.Failure;
        }
        return BTResult.Success;
    }
}
