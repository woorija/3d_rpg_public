using UnityEngine.Pool;

public abstract class ClassPool<T> where T : ClassPool<T>, new()
{
    protected IObjectPool<T> pool;
    public abstract void Init();
    public abstract void Reset();
    public void Release()
    {
        pool.Release(this as T);
    }
    public void SetPool(IObjectPool<T> _pool)
    {
        pool = _pool;
    }
}
