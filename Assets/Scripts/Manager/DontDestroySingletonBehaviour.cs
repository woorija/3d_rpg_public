using UnityEngine;

public class DontDestroySingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj.transform.root.gameObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
            return;
        }
        DontDestroyOnLoad(transform.root.gameObject);
    }
}
