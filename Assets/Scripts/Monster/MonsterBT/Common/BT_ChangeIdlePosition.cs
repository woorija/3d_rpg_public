using UnityEngine;

public class BT_ChangeIdlePosition : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        blackBoard.ChangeMovePoint();
        if (!blackBoard.isBoss)
        {
            blackBoard.ReleaseHUD();
        }
        return BTResult.Success;
    }
}
