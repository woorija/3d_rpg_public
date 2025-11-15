using UnityEngine;

public class Player_Jump : Player_Base
{
    float jumpTime = 0f;
    float minJumpTime = 0.1f;
    public override void StateEnter()
    {
        base.StateEnter();
        jumpTime = 0f;
        priority = 10;
        controller.StateMoveSpeedMultiplier = 0.6f;
        controller.Jump();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        jumpTime += Time.deltaTime;
        if (controller.IsGround() && jumpTime >= minJumpTime)
        {
            FSM.ChangeState(StateType.Land);
        }
        else if (controller.IsFall())
        {
            FSM.ChangeState(StateType.Fall);
        }
    }
    public override void StateExit()
    {
        priority = 10;
        base.StateExit();
    }
}
