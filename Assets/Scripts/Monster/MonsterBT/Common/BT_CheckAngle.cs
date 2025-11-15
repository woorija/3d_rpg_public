using UnityEngine;

public class BT_CheckAngle : BT_ActionNode
{
    [SerializeField] float angle1;
    [SerializeField] float angle2 = -1;
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if(angle2 == -1)
        {
            if(CustomUtility.CheckNormalAngle(angle1, BT.transform.forward, transform.position, blackBoard.player.transform.position))
            {
                return BTResult.Success;
            }
        }
        else
        {
            if (CustomUtility.CheckAngle(angle1, angle2, BT.transform.forward, transform.position, blackBoard.player.transform.position))
            {
                return BTResult.Success;
            }
        }
        return BTResult.Failure;
    }
}
