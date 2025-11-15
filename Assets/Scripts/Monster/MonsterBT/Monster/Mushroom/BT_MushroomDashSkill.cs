public class BT_MushroomDashSkill : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.DashSkill))
        {
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
