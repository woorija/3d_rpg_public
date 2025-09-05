using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(transform.root.gameObject);
    }
}
