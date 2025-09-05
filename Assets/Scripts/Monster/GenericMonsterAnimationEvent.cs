using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonsterAnimationEvent<TBehaviorTree, TBlackBoard> : MonoBehaviour where TBehaviorTree : BehaviorTree where TBlackBoard : BaseBlackBoard
{
    [SerializeField] protected TBehaviorTree BT;
    [SerializeField] protected TBlackBoard blackBoard;

    public void DieEvent()
    {
        BT.MeshSetActiveFalse();
    }
    public void StaggerEvent()
    {
        if (blackBoard.staggerTime > 0)
        {
            BT.PauseAnimation();
        }
    }
    public void NormalAttackEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.NormalAttack();
        }
    }
}
