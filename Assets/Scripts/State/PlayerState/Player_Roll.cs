public class Player_Roll : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        controller.StateMoveSpeedMultiplier = 0f;
        animator.SetTrigger(AnimationKey.Roll);
        controller.SetInvincible(true);
    }
    public override void StateUpdate()
    {
        controller.MoveRoll();
        base.StateUpdate();
    }
    public override void StateExit()
    {
        controller.SetInvincible(false);
        base.StateExit();
    }
}
