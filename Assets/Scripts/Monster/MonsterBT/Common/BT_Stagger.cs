public class BT_Stagger : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (BT.IsAnimationEnd(AnimationKey.Stagger))
        {
            BT.CheckDeleteRunningNode(99);
            return BTResult.Success;
        }
        if (blackBoard.staggerTime <= 0)
        {
            BT.PlayAnimation();
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
