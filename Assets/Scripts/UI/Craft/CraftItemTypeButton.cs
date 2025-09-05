using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItemTypeButton : MonoBehaviour
{
    [SerializeField] GameObject craftTypeListObject;
    bool isActive = false;
    public void OnClick()
    {
        isActive = !isActive;
        craftTypeListObject.SetActive(isActive);
    }
}
