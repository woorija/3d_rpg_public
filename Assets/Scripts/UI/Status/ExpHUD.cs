using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ExpHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject expInfor;
    [SerializeField] TMP_Text expText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!expInfor.activeSelf)
            expInfor.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (expInfor.activeSelf)
            expInfor.SetActive(false);
    }
    public void SetExpText(int _exp, int _maxexp)
    {
        expText.SetText("{0} / {1}", _exp, _maxexp);
    }
}
