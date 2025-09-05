using UnityEngine;

public class BT_CheckMushroomDashSkillUseable : BT_ActionNode
{
    MushroomBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = (MushroomBlackBoard)BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.currentDashSkillCooltime <= 0f)
        {
            if (blackBoard.CheckAngle(transform.position, 15f))
            {
                if (blackBoard.CheckDistance(3f))
                {
                    if (blackBoard.CheckHeightDifference(2f))
                    {
                        blackBoard.ResetDashSkillCooltime();
                        return BTResult.Success;
                    }
                }
            }
        }
        return BTResult.Failure;
    }
}
