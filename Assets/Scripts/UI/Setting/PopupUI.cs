using TMPro;
using UnityEngine;

public class PopupUI : MonoBehaviour, ICloseable
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject button;

    public void PopupMessage(string _message, bool _setActiveButton = false)
    {
        gameObject.SetActive(true);
        UIManager.Instance.OpenUI(this);
        text.SetText(_message);
        button.SetActive(_setActiveButton);
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
}
