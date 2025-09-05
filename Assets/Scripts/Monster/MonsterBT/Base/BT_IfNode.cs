using UnityEngine;

public class BT_IfNode : BT_DecoratorNode
{
    [field: SerializeField] protected BT_Node successNode;
    [field: SerializeField] protected BT_Node failureNode;
    public override BTResult Execute()
    {
        switch (child.Execute())
        {
            case BTResult.Success:
                return successNode.Execute();
            default: // 성공하지 못하면 무조건 failureNode실행
                return failureNode.Execute();
        }
    }
    public override void ResetNode()
    {
        base.ResetNode();
        successNode = null;
        failureNode = null;
    }
    public void SetSuccessNode(BT_Node _node)
    {
        successNode = _node;
    }
    public void SetFailureNode(BT_Node _node)
    {
        failureNode = _node;
    }
}
