using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTap : MonoBehaviour
{
    [SerializeField] QuestProgress progressType;
    [SerializeField] QuestUI questUI;
    public void OnClick()
    {
        questUI.SetTap(progressType);
    }
}
