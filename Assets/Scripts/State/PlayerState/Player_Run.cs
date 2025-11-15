public class Player_Run : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        controller.StateMoveSpeedMultiplier = 1.0f;
    }

    public override void StateUpdate()
    {
        if (!controller.isRun)
        {
            FSM.ChangeState(StateType.Walk);
        }
        controller.RotateToWalk();
        base.StateUpdate();
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
