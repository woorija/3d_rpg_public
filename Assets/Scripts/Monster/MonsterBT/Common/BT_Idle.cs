using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Idle : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if (blackBoard.currentIdleTime > 0)
        {
            BT.ChangeAnimatorBool(AnimationKey.Move, false);
            return BTResult.Success;
        }
        BT.ChangeAnimatorBool(AnimationKey.Move, true);
        return BTResult.Failure;
    }
}
