public class BT_CheckDie : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.isDie;
    }
}
