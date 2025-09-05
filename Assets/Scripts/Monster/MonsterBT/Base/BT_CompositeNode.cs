using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_CompositeNode : BT_Node
{
    //직접 사용하지 않고 Selector or Sequence 노드를 사용할것
    [field: SerializeField] protected List<BT_Node> children = new List<BT_Node>();

    public override BTResult Execute()
    {
        return BTResult.Success;
    }
    public void ResetNode()
    {
        children.Clear();
    }
    public void AddNode(BT_Node _node)
    {
        children.Add(_node);
    }
}
