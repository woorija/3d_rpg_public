using System.Collections.Generic;
using UnityEngine;

public class QuestInteractsUI : MonoBehaviour
{
    [SerializeField] GameObject completableQuestUI;
    [SerializeField] GameObject inprogressQuestUI;
    [SerializeField] GameObject startableQuestUI;

    [SerializeField] QuestInteractButton[] completableQuestButtons;
    [SerializeField] QuestInteractButton[] inprogressQuestButtons;
    [SerializeField] QuestInteractButton[] startableQuestButtons;

    public void SetUI(int _npcId)
    {
        gameObject.SetActive(true);
        QuestManager.Instance.SetStartableQuestId(_npcId);
        QuestManager.Instance.SetInprogressableQuestId(_npcId);
        QuestManager.Instance.SetCompletableQuestId(_npcId);

        SetQuestButtons(QuestManager.Instance.currentCompletableQuestIds, completableQuestUI, completableQuestButtons);
        SetQuestButtons(QuestManager.Instance.currentInprogressableQuestIds, inprogressQuestUI, inprogressQuestButtons);
        SetQuestButtons(QuestManager.Instance.currentStartableQuestIds, startableQuestUI, startableQuestButtons);


    }
    void SetQuestButtons(List<int> _questIdList, GameObject _progressTypeUI, QuestInteractButton[] _buttons)
    {
        if (_questIdList.Count == 0)
        {
            _progressTypeUI.SetActive(false);
            foreach(var button in _buttons)
            {
                button.CloseUI();
            }
        }
        else
        {
            _progressTypeUI.SetActive(true);

            int count = _questIdList.Count;
            if (count > 10)
            {
                count = 10;
            }
            for (int i = 0; i < count; i++)
            {
                _buttons[i].SetButton(_questIdList[i]);
            }
            for (int i = count; i < _buttons.Length; i++)
            {
                _buttons[i].CloseUI();
            }
        }
    }
    public void CloseUI()
    {
        foreach(QuestInteractButton button in startableQuestButtons)
        {
            button.CloseUI();
        }
        foreach (QuestInteractButton button in inprogressQuestButtons)
        {
            button.CloseUI();
        }
        foreach (QuestInteractButton button in completableQuestButtons)
        {
            button.CloseUI();
        }
        gameObject.SetActive(false);
    }
}
