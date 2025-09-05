using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Button button;
    public void SetButton(Teleport _data)
    {
        text.text = _data.UIText;
        button.onClick.AddListener(() => TalkManager.Instance.CloseUI());
        button.onClick.AddListener(async () => await CustomSceneManager.Instance.LoadScene(_data.sceneName, _data.teleportPos));
    }
    public void Reset()
    {
        text.text = string.Empty;
        button.onClick.RemoveAllListeners();
    }
    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }
}
