using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckAttackRange : BT_CheckDistance
{
    protected override void Awake()
    {
        base.Awake();
        distance = blackBoard.blackBoardData.attackRange;
    }
    protected override void MoveStop()
    {
        blackBoard.agent.stoppingDistance = blackBoard.blackBoardData.normalAttackRange;
    }
}
