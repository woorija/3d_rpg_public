using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CheckAngle : BT_ActionNode
{
    [SerializeField] float angle1;
    [SerializeField] float angle2 = -1;
    public override BTResult Execute()
    {
        BaseBlackBoard blackBoard = BT.GetBlackBoard();
        if(angle2 == -1)
        {
            if(CustomUtility.CheckNormalAngle(angle1, BT.transform.forward, transform.position, blackBoard.player.transform.position))
            {
                return BTResult.Success;
            }
        }
        else
        {
            if (CustomUtility.CheckAngle(angle1, angle2, BT.transform.forward, transform.position, blackBoard.player.transform.position))
            {
                return BTResult.Success;
            }
        }
        return BTResult.Failure;
    }
}
