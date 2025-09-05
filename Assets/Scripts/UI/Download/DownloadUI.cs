using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DownloadUI : MonoBehaviour
{
    [SerializeField] TMP_Text sizeInfoText;
    [SerializeField] TMP_Text downloadPercentageText;
    [SerializeField] Slider downloadSlider;

    long totalSize;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void SetSlider(float _value)
    {
        downloadSlider.value = _value;
    }
    public void SetPercentageText(float _percentage)
    {
        downloadPercentageText.text = $"{_percentage:##.#}%";
    }
    public void SetTotalSize(long _size)
    {
        totalSize = _size;
    }
    public void SetSizeInfoText(float _size)
    {
        sizeInfoText.text = $"{DownloadManager.SetFileSizeText((long)_size)} / {DownloadManager.SetFileSizeText(totalSize)}";
    }
}
