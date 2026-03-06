using UnityEngine;

public class BT_SetGlobalCooltime : BT_ActionNode
{
    [SerializeField] float globalCooltime;
    protected BaseBlackBoard blackBoard;

    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        blackBoard.SetGlobalCooltime(globalCooltime);
        return BTResult.Success;
    }
}
