using UnityEngine;

public class BT_CheckMushroomJumpSkillUseable : BT_ActionNode
{
    MushroomBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = (MushroomBlackBoard)BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        if (blackBoard.currentJumpSkillCooltime <= 0f) {
            MushroomBlackBoardSO mushroomBlackBoardData = (MushroomBlackBoardSO)blackBoard.blackBoardData;
            if (blackBoard.CheckDistance(mushroomBlackBoardData.jumpSkillOuterRange))
            {
                if (blackBoard.CheckHeightDifference(1f))
                {
                    blackBoard.ResetJumpSkillCooltime();
                    return BTResult.Success;
                }
            }
        }
        return BTResult.Failure;
    }
}
