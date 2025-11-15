public class BT_Idle : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.currentIdleTime > 0)
        {
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
