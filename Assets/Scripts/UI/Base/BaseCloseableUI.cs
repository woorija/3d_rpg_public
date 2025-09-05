using UnityEngine;

public class BaseCloseableUI : MonoBehaviour, ICloseable
{
    public void Open()
    {
        gameObject.SetActive(true);
        UIManager.Instance.OpenUI(this);
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
