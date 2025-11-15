public class MushroomBT : BehaviorTree
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
