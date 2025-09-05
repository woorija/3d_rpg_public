public class Player_Buff : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.Buff);
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