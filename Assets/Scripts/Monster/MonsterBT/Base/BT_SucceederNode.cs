public class BT_SucceederNode : BT_DecoratorNode
{
    public override BTResult Execute()
    {
        if(child == null) return BTResult.Success;
        child.Execute();
        return BTResult.Success;
    }
}
