public class BT_CheckTrackingLimitRange : BT_ConditionNode
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
            blackBoard.ChangeReturn(true);
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
    protected override bool CheckCondition()
    {
        return blackBoard.CheckTrackingLimit();
    }
}
