using TMPro;
using UnityEngine;

public class QuestInteractButton : ButtonEvent
{
    [Tooltip("0: 시작\n1: 완료\n2: 진행중")]
    [SerializeField] int progressType;
    [SerializeField] TMP_Text text;
    public int questId { get; private set; }
    public void SetButton(int _id)
    {
        gameObject.SetActive(true);
        questId = _id;
        text.text = QuestDataBase.InfoDB[questId].questName;
    }
    public void OnClick() // talk데이터 변경 및 퀘스트 진행
    {
        TalkManager.Instance.SetQuestTalk(questId, progressType);
        GameManager.Instance.ExitForcedUIMode();
        InteractQuest();
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    void InteractQuest()
    {
        switch (progressType)
        {
            case 0:
                QuestManager.Instance.StartQuest(questId);
                break;
            case 1:
                QuestManager.Instance.CompleteQuest(questId);
                break;
            case 2:
                TalkManager.Instance.TalkToQuest(questId);
                break;
        }
    }
}
