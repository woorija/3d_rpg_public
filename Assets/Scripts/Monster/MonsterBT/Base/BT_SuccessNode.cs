using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SuccessNode : BT_Node
{
    public override BTResult Execute()
    {
        return BTResult.Success;
    }
}
