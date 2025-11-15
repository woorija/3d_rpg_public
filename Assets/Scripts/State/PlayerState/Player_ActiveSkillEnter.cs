using UnityEngine;

public class Player_ActiveSkillEnter : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetInteger(AnimationKey.SkillId, controller.currentPlaySkillId);
        FSM.ChangeState(StateType.ActiveSkill);
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
