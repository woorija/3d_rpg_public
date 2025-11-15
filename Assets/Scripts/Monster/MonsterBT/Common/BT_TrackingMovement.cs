public class BT_TrackingMovement : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.agent.speed = blackBoard.blackBoardData.trackingMoveSpeed;
        blackBoard.agent.SetDestination(blackBoard.player.transform.position);

        return BTResult.Success;
    }
}
