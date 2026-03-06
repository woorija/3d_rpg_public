using UnityEngine;
public class BT_ResetSkillCooltime : BT_ActionNode
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
    public override BTResult Execute()
    {
        blackBoard.ResetSkillCooltime(actionKey);
        return BTResult.Success;
    }
}
