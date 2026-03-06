public class BT_CheckStagger : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (CheckCondition())
        {
            if (BT.IsCurrentAnimatorStateName(AnimationKey.Stagger))
            {
                BT.ReplayAnimation();
            }
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
    protected override bool CheckCondition()
    {
        return blackBoard.staggerTime > 0;
    }
}
