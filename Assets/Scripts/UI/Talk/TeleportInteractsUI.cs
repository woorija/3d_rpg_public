using UnityEngine;

public class TeleportInteractsUI : MonoBehaviour
{
    [SerializeField] TeleportButton[] teleportButtons;
    public void SetUI(TeleportData _data)
    {
        for (int i = 0; i < teleportButtons.Length; i++)
        {
            teleportButtons[i].Reset();
        }

        for (int i = 0; i < _data.GetCount(); i++)
        {
            int index = i;
            teleportButtons[index].SetActive(true);
            teleportButtons[index].SetButton(_data.teleports[index]);
        }
        for(int i = _data.GetCount(); i < teleportButtons.Length; i++)
        {
            teleportButtons[i].SetActive(false);
        }
    }
    public void OpenUI()
    {
        gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
