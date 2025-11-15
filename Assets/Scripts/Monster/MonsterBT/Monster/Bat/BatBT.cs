public class BatBT : BehaviorTree
{
    private void Update()
    {
        if(RunningNode != null)
        {
            RunningNode.Execute();
        }
        else
        {
            RootNode.Execute();
        }
    }
}
