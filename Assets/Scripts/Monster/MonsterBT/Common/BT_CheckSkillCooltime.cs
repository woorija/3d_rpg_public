using UnityEngine;

public class BT_CheckSkillCooltime : BT_ConditionNode
{
    [MonsterActionKeyDropdown]
    [SerializeField]
    string actionKey;

    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    protected override bool CheckCondition()
    {
        return blackBoard.GetSkillCooltime(actionKey) < 0;
    }
}
