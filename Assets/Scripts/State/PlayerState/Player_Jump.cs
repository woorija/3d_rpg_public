public class Player_Jump : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.Jump);
        priority = 5;
        controller.StateMoveSpeedMultiplier = 0.6f;
        controller.Jump();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (controller.IsFall())
        {
            FSM.ChangeState(StateType.Fall);
        }
    }
    public override void StateExit()
    {
        priority = 2;
        animator.ResetTrigger(AnimationKey.Jump);
        base.StateExit();
    }
}
