using TMPro;
using UnityEngine;

public class ItemDeleteFaleUI : MonoBehaviour, ICloseable
{
    const string deleteInforText = "현재 갯수를 초과하여\r\n버릴수 없습니다.";
    const string saleInforText = "현재 갯수를 초과하여\r\n판매할 수 없습니다.";
    [SerializeField] TMP_Text text;
    public void Open()
    {
        UIManager.Instance.OpenUI(this);
        gameObject.SetActive(true);
    }
    public void Close()
    {
        UIManager.Instance.CloseUI(this);
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetText(bool _isSale)
    {
        text.text = _isSale ? saleInforText : deleteInforText;
    }
}
