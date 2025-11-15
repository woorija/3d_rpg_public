using UnityEngine;

public class BT_RotationToPlayer : BT_ActionNode
{
    [SerializeField] float rotationSpeed;
    protected BaseBlackBoard blackBoard;
    protected override void Awake()
    {
        base.Awake();
        blackBoard = BT.GetBlackBoard();
    }
    public override BTResult Execute()
    {
        Vector3 RotatePos = blackBoard.player.transform.position - BT.transform.position;
        RotatePos.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(RotatePos);
        BT.transform.parent.rotation = Quaternion.RotateTowards(BT.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        return BTResult.Success;
    }
}
