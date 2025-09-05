using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_MushroomJumpSkill : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.JumpSkill))
        {
            BT.ChangeAnimatorTrigger(AnimationKey.AnimationEnd);
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.ChangeAnimatorTrigger(AnimationKey.JumpSkill);
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
