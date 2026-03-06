public class BT_CheckReturn : BT_ConditionNode
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
            blackBoard.ReleaseHUD();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
    protected override bool CheckCondition()
    {
        return blackBoard.isReturn;
    }
}
