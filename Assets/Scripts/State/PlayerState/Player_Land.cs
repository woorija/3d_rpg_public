using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Land : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.Land);
        priority = 5;
        controller.StateMoveSpeedMultiplier = 0f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (FSM.CanChangeState(StateType.Walk) && animator.GetBool(AnimationKey.Move))
        {
            FSM.ChangeState(StateType.Walk);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f)
        {
            FSM.ChangeState(StateType.Idle);
        }
    }
    public override void StateExit()
    {
        priority = 5;
        base.StateExit();
    }
    public void OnMotionSkip() // 애니메이션 이벤트에 등록
    {
        priority = 1;
    }
}
