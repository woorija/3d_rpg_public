public class BT_Die : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.DropItem();
        blackBoard.ReleaseHUD();
        blackBoard.ResetRespawnTime();
        return BTResult.Success;
    }
}
