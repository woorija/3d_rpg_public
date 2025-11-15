public class SpikeBT : BehaviorTree
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
