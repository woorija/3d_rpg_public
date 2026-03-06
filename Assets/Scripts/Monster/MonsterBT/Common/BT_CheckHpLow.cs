using UnityEngine;

public class BT_CheckHpLow : BT_ConditionNode
{
    protected BaseBlackBoard blackBoard;
    [SerializeField, GetBlackBoardData(typeof(int))] protected int hpValue;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.hp <= hpValue;
    }
}
