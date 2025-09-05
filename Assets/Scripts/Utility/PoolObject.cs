public class PoolObject : PoolBehaviour<PoolObject>
{
    public void ReturnPool()
    {
        Release(this);
    }
}
