public class BT_ChangeIdlePosition : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.ChangeMovePoint();
        if (!blackBoard.isBoss)
        {
            blackBoard.ReleaseHUD();
        }
        return BTResult.Success;
    }
}
