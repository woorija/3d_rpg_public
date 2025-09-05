using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] QuestUI questUI;
    int questId;
    public void SetSlot(int _id)
    {
        gameObject.SetActive(true);
        questId = _id;
        text.text = QuestDataBase.InfoDB[questId].questName;
    }
    public void OnClick()
    {
        questUI.InformationOpen(questId);
    }
    public void DisableSlot()
    {
        gameObject.SetActive(false);
    }
}
