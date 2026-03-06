using UnityEngine;

public class BT_DecoratorNode : BT_Node
{
    [field: SerializeField] protected BT_Node child;
    public override BTResult Execute()
    {
        return BTResult.Success;
    }
    public override void ResetNode()
    {
        child = null;
    }
    public void SetChildNode(BT_Node _node)
    {
        child = _node;
    }
}
