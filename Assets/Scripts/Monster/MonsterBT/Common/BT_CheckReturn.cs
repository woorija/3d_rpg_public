public class BT_CheckReturn : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.isReturn)
        {
            blackBoard.ReleaseHUD();
            return BTResult.Success;
        }
        else
        {
            return BTResult.Failure;
        }
    }
}
