using UnityEngine;

public class GenericMonsterAnimationEvent<TBlackBoard> : MonoBehaviour where TBlackBoard : BaseBlackBoard
{
    [SerializeField] protected BehaviorTree BT;
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
