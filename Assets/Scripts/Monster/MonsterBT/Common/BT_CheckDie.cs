public class BT_CheckDie : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.isDie)
        {
            BT.PlayAnimation();
            blackBoard.DropItem();
            blackBoard.ReleaseHUD();
            blackBoard.ResetRespawnTime();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
