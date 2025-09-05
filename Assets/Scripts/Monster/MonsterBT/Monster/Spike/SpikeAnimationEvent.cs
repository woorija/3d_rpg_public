public class SpikeAnimationEvent : GenericMonsterAnimationEvent<SpikeBT, SpikeBlackBoard>
{
    public void DashSkillEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.DashSkillAttack();
        }
    }
    public void JumpSkillEvent()
    {
        if (!blackBoard.player.IsInvincible) 
        {
            blackBoard.JumpSkillAttack();
        }
    }
}
