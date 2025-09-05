using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_ActionNode : BT_Node
{
    //직접 사용하지 않고 하위 노드 클래스를 사용
    public override BTResult Execute()
    { 
        return BTResult.Success;
    }
}
