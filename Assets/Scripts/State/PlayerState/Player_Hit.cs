public class Player_Hit : Player_Base
{
    public override void Awake()
    {
        base.Awake();
        controller.StateMoveSpeedMultiplier = 0f;
    }
    public override void StateEnter()
    {
        base.StateEnter();
        animator.SetTrigger(AnimationKey.IsHit);
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
