public class BT_CheckStagger : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
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
