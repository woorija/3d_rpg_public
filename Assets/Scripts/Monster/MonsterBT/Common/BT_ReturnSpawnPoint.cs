public class BT_ReturnSpawnPoint : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.agent.speed = blackBoard.blackBoardData.returnMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.movePoint);
        if (blackBoard.agent.remainingDistance <= blackBoard.agent.stoppingDistance)
        {
            blackBoard.ChangeReturn(false);
            return BTResult.Failure;
        }
        return BTResult.Success;
    }
}
