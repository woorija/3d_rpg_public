public class Player_Attack : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.Attack);
        if (controller.IsMove())
        {
            controller.RotateToWalk();
        }
        else
        {
            controller.Rotate();
        }
        controller.StateMoveSpeedMultiplier = 0f;
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
