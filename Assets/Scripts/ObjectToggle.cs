using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public void Toggle()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
