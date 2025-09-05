using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_DecoratorNode : BT_Node
{
    //직접 사용하지 않고 if 노드를 사용할것
    [field: SerializeField] protected BT_Node child;
    public override BTResult Execute()
    {
        return BTResult.Success;
    }
    public virtual void ResetNode()
    {
        child = null;
    }
    public void SetChildNode(BT_Node _node)
    {
        child = _node;
    }
}
