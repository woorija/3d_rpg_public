using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_MushroomDashSkill : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.DashSkill))
        {
            BT.ChangeAnimatorTrigger(AnimationKey.AnimationEnd);
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.ChangeAnimatorTrigger(AnimationKey.DashSkill);
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
