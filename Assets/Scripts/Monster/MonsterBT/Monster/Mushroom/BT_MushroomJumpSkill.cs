public class BT_MushroomJumpSkill : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.JumpSkill))
        {
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
