public class BT_InverterNode : BT_DecoratorNode
{
    public override BTResult Execute()
    {
        if (child == null) return BTResult.Failure;
        BTResult result = child.Execute();
        switch (result)
        {
            case BTResult.Success:
                return BTResult.Failure;
            case BTResult.Failure:
                return BTResult.Success;
            default:
                return BTResult.Running;
        }
    }
}
