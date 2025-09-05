public class Player_Fall : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.Fall);
        controller.StateMoveSpeedMultiplier = 0.3f;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (controller.IsGround())
        {
            FSM.ChangeState(StateType.Land);
        }
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
