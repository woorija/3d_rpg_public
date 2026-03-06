using UnityEngine;

public class BT_CheckDistance : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    [SerializeField, GetBlackBoardData(typeof(float))] protected float distance;
    [SerializeField, GetBlackBoardData(typeof(float))] protected float stoppingDistance;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (CheckCondition())
        {
            MoveStop();
            return BTResult.Success;
        }
        return BTResult.Failure;
    }
    protected override bool CheckCondition()
    {
        return blackBoard.CheckDistance(distance * distance);
    }
    protected virtual void MoveStop()
    {
        blackBoard.agent.stoppingDistance = stoppingDistance;
    }
}
