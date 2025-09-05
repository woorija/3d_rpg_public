using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Die : BT_ActionNode
{
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if(blackBoard.currentRespawnTime <= 0)
        {
            BT.MeshSetActiveTrue();
            BT.ChangeAnimatorTrigger(AnimationKey.AnimationEnd);
            blackBoard.ChangeMovePoint();
            blackBoard.Respawn();
            BT.CheckDeleteRunningNode(99);
            return BTResult.Success;
        }
        BT.ChangeAnimatorTrigger(AnimationKey.Die);
        BT.GetRunningNode(this);
        return BTResult.Running;
    }
}
