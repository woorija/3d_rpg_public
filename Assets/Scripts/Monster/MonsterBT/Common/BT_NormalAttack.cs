using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_NormalAttack : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.NormalAttack))
        {
            BT.ChangeAnimatorTrigger(AnimationKey.AnimationEnd);
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.ChangeAnimatorTrigger(AnimationKey.NormalAttack);
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
