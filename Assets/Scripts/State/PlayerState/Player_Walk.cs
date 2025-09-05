public class Player_Walk : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetBool(AnimationKey.IsWalk, true);
        controller.StateMoveSpeedMultiplier = 1.0f;
    }

    public override void StateUpdate()
    {
        if (!controller.IsMove())
        {
            FSM.ChangeState(StateType.Idle);
        }
        controller.RotateToWalk();
        base.StateUpdate();
    }
    public override void StateExit()
    {
        animator.SetBool(AnimationKey.IsWalk, false);
        base.StateExit();
    }
}
