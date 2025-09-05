using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckDie : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if (blackBoard.isDie)
        {
            BT.PlayAnimation();
            blackBoard.DropItem();
            blackBoard.ReleaseHUD();
            blackBoard.ResetRespawnTime();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
