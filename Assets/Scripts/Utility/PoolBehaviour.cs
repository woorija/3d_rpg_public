using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PoolBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected IObjectPool<T> pool;
    public void Release(T _obj)
    {
        pool.Release(_obj);
    }
    public void SetPool(IObjectPool<T> _pool)
    {
        pool = _pool;
    }
}
