public class BT_CheckNormalAttackCooltime : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.currentAttackCooltime < 0)
        {
            blackBoard.ResetAttackCooltime();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
