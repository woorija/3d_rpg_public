using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestInformationUI : MonoBehaviour
{
    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questDescription;
    [SerializeField] TMP_Text questProgress;
    StringBuilder stringBuilder = new StringBuilder(100);
    public void OpenUI(QuestProgress _progress, int _index)
    {
        gameObject.SetActive(true);
        questName.text = QuestDataBase.InfoDB[_index].questName;
        switch (_progress)
        {
            case QuestProgress.NotStarted:
                questDescription.text = QuestDataBase.InfoDB[_index].startDescription;
                questProgress.text = string.Empty;
                break;
            case QuestProgress.InProgress:
                questDescription.text = QuestDataBase.InfoDB[_index].inprogressDescription;
                SetProgressText(_index);
                break;
            case QuestProgress.Completed:
                questDescription.text = QuestDataBase.InfoDB[_index].completeDescription;
                questProgress.text = string.Empty;
                break;
        }
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    public void SetProgressText(int _index)
    {
        stringBuilder.Clear();
        List<int> ids = QuestDataBase.QuestDB[_index].ids;
        List<int> counts = QuestDataBase.QuestDB[_index].counts;
        for (int i = 0; i < ids.Count; i++)
        {
            switch (CustomUtility.GetDigitCount(ids[i]))
            {
                case 1:
                    int count = QuestManager.Instance.talkDatas[_index][ids[i]] ? 1 : 0;
                    stringBuilder.Append($"{NPCDataBase.NPCDB[ids[i]].name}와(과) 대화하기\n {count} / 1\n");
                    break;
                case 5:
                    stringBuilder.Append($"{MonsterNameDataBase.monsterNameDB[ids[i]]} 사냥하기 {QuestManager.Instance.huntDatas[_index][ids[i]]}/{counts[i]}");
                    break;
                case 9:
                    stringBuilder.Append($"{ItemDataBase.GetItemName(ids[i])}  {InventoryData.Instance.GetItemCount(ids[i])}/{counts[i]}\n");
                    break;
            }
        }
        questProgress.text = stringBuilder.ToString();
    }
}
