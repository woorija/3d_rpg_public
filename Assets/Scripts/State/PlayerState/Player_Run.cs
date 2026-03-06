public class Player_Run : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        controller.StateMoveSpeedMultiplier = 1.0f;
    }

    public override void StateUpdate()
    {
        if (controller.IsFall(-1.5f))
        {
            FSM.ChangeState(StateType.Fall);
        }
        else if (!controller.isRun)
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
