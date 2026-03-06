public class SpikeAnimationEvent : GenericMonsterAnimationEvent<SpikeBlackBoard>
{
    public void DashSkillEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.DashSkillAttack();
        }
    }
    public void RageSkillEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.RageSkillAttack();
        }
    }
    public void RightSmashEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.RightSmashAttack();
        }
    }
    public void LeftSmashEvent()
    {
        if (!blackBoard.player.IsInvincible)
        {
            blackBoard.LeftSmashAttack();
        }
    }
}
