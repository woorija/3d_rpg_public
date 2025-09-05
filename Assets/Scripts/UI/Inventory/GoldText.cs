using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldText : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    public void SetText(long _gold)
    {
       goldText.text = _gold.ToString();
    }
}
