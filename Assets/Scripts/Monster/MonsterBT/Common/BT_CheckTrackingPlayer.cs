public class BT_CheckTrackingPlayer : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        //중심좌표로 부터 멀어졌을경우를 체크
        if(!CustomUtility.CheckSqrDistance(transform.position, blackBoard.spawnPointCenter, blackBoard.blackBoardData.limitTrackingRange))
        {
            blackBoard.ChangeReturn(true);
            return BTResult.Failure;
        }
        if (blackBoard.CheckDistance(blackBoard.blackBoardData.trackingRange * blackBoard.blackBoardData.trackingRange) && blackBoard.CheckHeightDifference(transform.position.y))
        {
            blackBoard.agent.stoppingDistance = blackBoard.blackBoardData.normalAttackRange;
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
}
