public class BT_IdleMovement : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.agent.stoppingDistance = 0;
        blackBoard.agent.speed = blackBoard.blackBoardData.idleMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.movePoint);
        if (blackBoard.agent.remainingDistance <= 0f)
        {
            return BTResult.Failure;
        }
        return BTResult.Success;
    }
}
