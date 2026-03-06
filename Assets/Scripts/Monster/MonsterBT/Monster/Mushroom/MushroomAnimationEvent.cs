using UnityEngine;

public class MushroomAnimationEvent : GenericMonsterAnimationEvent<MushroomBlackBoard>
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
