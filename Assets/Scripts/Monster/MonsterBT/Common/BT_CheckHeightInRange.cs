using UnityEngine;

public class BT_CheckHeightInRange : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    [SerializeField, GetBlackBoardData(typeof(float))] protected float min, max;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.CheckHeightInRange(min, max);
    }
}
