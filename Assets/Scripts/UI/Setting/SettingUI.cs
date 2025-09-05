using UnityEngine;

public class SettingUI : MonoBehaviour, ICloseable
{
    [SerializeField] GameObject[] UIList;
    [SerializeField] KeyRebindMainUI KeyRebindMainUI;
    int currentTab = 0;
    public void ChangeTab(int _tab)
    {
        UIList[currentTab].SetActive(false);
        currentTab = _tab;
        UIList[currentTab].SetActive(true);
    }
    public void OpenUI()
    {
        GameManager.Instance.GameModeChange(GameMode.ForcedUIMode);
        UIManager.Instance.OpenUI(this);

        KeyRebindMainUI.AllTabClose();
        KeyRebindMainUI.ChangeTab(0);

        for (int i = 0; i < UIList.Length; i++)
        {
            UIList[i].SetActive(false);
        }
        ChangeTab(0);

        gameObject.SetActive(true);
    }
    public void Close()
    {
        GameManager.Instance.GameModeChange(GameMode.ControllMode);
        DataManager.Instance.SaveSetting();
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
