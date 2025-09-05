using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckStagger : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if (blackBoard.staggerTime > 0)
        {
            if (BT.IsCurrentAnimatorStateName(AnimationKey.Stagger))
            {
                BT.ReplayAnimation();
            }
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
