public class BT_Respawn : BT_ActionNode
{
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.currentRespawnTime <= 0)
        {
            BT.MeshSetActiveTrue();
            blackBoard.ChangeMovePoint();
            blackBoard.Respawn();
            BT.CheckDeleteRunningNode(99);
            return BTResult.Success;
        }
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
