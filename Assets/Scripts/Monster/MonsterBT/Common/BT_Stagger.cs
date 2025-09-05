using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Stagger : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if (BT.IsAnimationEnd(AnimationKey.Stagger))
        {
            BT.ChangeAnimatorTrigger(AnimationKey.AnimationEnd);
            BT.CheckDeleteRunningNode(99);
            return BTResult.Success;
        }
        if (blackBoard.staggerTime <= 0)
        {
            BT.PlayAnimation();
        }
        BT.ChangeAnimatorTrigger(AnimationKey.Stagger);
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
