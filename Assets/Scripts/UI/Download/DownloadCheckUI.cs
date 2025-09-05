using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DownloadCheckUI : MonoBehaviour
{
    [SerializeField] TMP_Text sizeInfoText;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void SetSizeText(long _size)
    {
        sizeInfoText.text = $"{DownloadManager.SetFileSizeText(_size)}";
    }
}
