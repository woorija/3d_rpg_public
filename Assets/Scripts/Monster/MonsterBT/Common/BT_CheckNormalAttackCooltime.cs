using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckNormalAttackCooltime : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        BT.ChangeAnimatorBool(AnimationKey.Move, false);
        if (blackBoard.currentAttackCooltime < 0)
        {
            blackBoard.ResetAttackCooltime();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
