using UnityEngine;

public class BT_CheckHeightDifference : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    [SerializeField, GetBlackBoardData(typeof(float))] protected float heightDifference;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.CheckHeightDifference(heightDifference);
    }
}
