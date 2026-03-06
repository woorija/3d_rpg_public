using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class PopupUI : SingletonBehaviour<PopupUI>, ICloseable
{
    [SerializeField] GameObject popupUI;
    [SerializeField] TMP_Text text;
    [SerializeField] Button button;

    Action onPopupClosed;
    protected override void Awake()
    {
        base.Awake();
        popupUI.gameObject.SetActive(false);
    }
    public void PopupMessage(string _message, bool _setActiveButton = false, Action _onPopupClosed = null)
    {
        GameManager.Instance.ChangeUIMode();
        popupUI.gameObject.SetActive(true);
        UIManager.Instance.OpenUI(this);

        text.SetText(_message);
        button.gameObject.SetActive(_setActiveButton);
        button.interactable = true;
        if (_setActiveButton)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Close);
            onPopupClosed = _onPopupClosed;
        }
    }
    public void Close()
    {
        button.interactable = false;
        UIManager.Instance.CloseUI(this);

        var callback = onPopupClosed;
        onPopupClosed = null;
        callback?.Invoke();
        popupUI.gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
    public void ShowPlayerDiePopup()
    {
        PopupMessage("사망하였습니다.\n가까운 마을에서 부활합니다.", true, GameManager.Instance.PlayerRespawn);
    }
}
