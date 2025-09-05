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
    public override void MeshSetActiveTrue()
    {
        _animator.gameObject.SetActive(true);
    }
    public override void MeshSetActiveFalse()
    {
        _animator.gameObject.SetActive(false);
    }
}
