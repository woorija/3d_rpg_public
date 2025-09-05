using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_RotationToPlayer : BT_ActionNode
{
    [SerializeField] float rotationSpeed;
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        Vector3 RotatePos = blackBoard.player.transform.position - BT.transform.position;
        RotatePos.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(RotatePos);
        BT.transform.parent.rotation = Quaternion.RotateTowards(BT.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        return BTResult.Success;
    }
}
