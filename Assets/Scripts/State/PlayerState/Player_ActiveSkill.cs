public class Player_ActiveSkill : Player_Base
{
    public override void StateEnter()
    {
        base.StateEnter();
        DevelopUtility.Log(controller.currentPlaySkillId);
        switch (controller.currentPlaySkillId)
        {
            case 100001:
            case 210001:
                SetRotate();
                SetMoveSpeed(0);
                break;
        }
    }
    public override void StateUpdate()
    {
        base.StateUpdate();
    }
    public override void StateExit()
    {
        animator.SetInteger(AnimationKey.SkillId, 0);
        base.StateExit();
    }
    private void SetMoveSpeed(float _speed)
    {
        controller.StateMoveSpeedMultiplier = _speed;
    }
    private void SetRotate()
    {
        if (controller.IsMove())
        {
            controller.RotateToWalk();
        }
        else
        {
            controller.Rotate();
        }
    }
}
