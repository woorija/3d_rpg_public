public class BT_ConditionNode : BT_Node
{
    //직접 사용하지 않고 하위 노드 클래스를 사용
    public override BTResult Execute()
    {
        return CheckCondition() ? BTResult.Success : BTResult.Failure;
    }
    protected virtual bool CheckCondition()
    {
        return true;
    }
}
