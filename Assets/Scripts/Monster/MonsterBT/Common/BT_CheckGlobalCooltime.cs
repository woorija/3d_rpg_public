public class BT_CheckGlobalCooltime : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.globalCooltime <= 0f;
    }
}
