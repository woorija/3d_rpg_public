using UnityEngine;

public class BT_SetDashData : BT_ActionNode
{
    [SerializeField] float speed;
    [SerializeField] float distance;
    protected BaseBlackBoard blackBoard;

    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.SetDashData(speed, distance);
        return BTResult.Success;
    }
}
