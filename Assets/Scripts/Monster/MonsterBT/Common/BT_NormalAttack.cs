public class BT_NormalAttack : BT_ActionNode
{
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.NormalAttack))
        {
            BT.CheckDeleteRunningNode(1);
            return BTResult.Success;
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
