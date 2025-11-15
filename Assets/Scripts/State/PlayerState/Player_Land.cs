public class Player_Land : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        priority = 15;
        controller.StateMoveSpeedMultiplier = 0f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (priority == 1 && FSM.CanChangeState(StateType.Walk) && controller.IsMove())
        {
            FSM.ChangeState(StateType.Walk);
        }
        if(GetAnimationNormalizedTime() > 0.98f)
        {
            FSM.ChangeState(StateType.Idle);
        }
    }
    public override void StateExit()
    {
        base.StateExit();
    }
    public void OnMotionSkip() // 애니메이션 이벤트에 등록
    {
        priority = 1;
    }
}
