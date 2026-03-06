public class Player_Walk : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        controller.StateMoveSpeedMultiplier = 1.0f;
    }

    public override void StateUpdate()
    {
        if (controller.IsFall(-4f))
        {
            FSM.ChangeState(StateType.Fall);
        }
        else if (controller.isRun && FSM.CanChangeState(StateType.Run))
        {
            FSM.ChangeState(StateType.Run);
        }
        controller.RotateToWalk();
        base.StateUpdate();
    }
    public override void StateExit()
    {
        base.StateExit();
    }
}
