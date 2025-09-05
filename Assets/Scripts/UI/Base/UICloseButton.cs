using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloseButton : MonoBehaviour
{
    ICloseable UIObject;
    private void Awake()
    {
        UIObject = GetComponentInParent<ICloseable>();
    }
    public void UIClose()
    {
        UIManager.Instance.CloseUI(UIObject);
    }
}
