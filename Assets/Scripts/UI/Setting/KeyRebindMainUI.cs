using UnityEngine;

public class KeyRebindMainUI : MonoBehaviour
{
    [SerializeField] GameObject[] keyRebindTabs;
    int currentTab = 0;
    public void ChangeTab(int _index)
    {
        keyRebindTabs[currentTab].SetActive(false);
        currentTab = _index;
        keyRebindTabs[currentTab].SetActive(true);
    }

    public void AllTabClose()
    {
        for (int i = 0; i < keyRebindTabs.Length; i++)
        {
            keyRebindTabs[i].SetActive(false);
        }
    }
}
