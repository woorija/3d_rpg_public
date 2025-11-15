using UnityEngine;

public class CraftItemTypeButton : MonoBehaviour
{
    [SerializeField] GameObject craftTypeListObject;
    bool isActive;

    private void Awake()
    {
        isActive = craftTypeListObject.activeSelf;
    }
    public void OnClick()
    {
        isActive = !isActive;
        craftTypeListObject.SetActive(isActive);
    }
}
