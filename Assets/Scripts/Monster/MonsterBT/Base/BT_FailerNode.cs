public class BT_FailerNode : BT_DecoratorNode
{
    public override BTResult Execute()
    {
        if (child == null) return BTResult.Failure;
        child.Execute();
        return BTResult.Failure;
    }
}
