using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonBehaviour<QuestManager>
{
    #region 퀘스트리스트
    List<QuestData> notStartQuests = new List<QuestData>(128);
    public List<QuestData> startableQuests { get; private set; } = new List<QuestData>(64);
    public List<QuestData> inProgressQuests { get; private set; } = new List<QuestData>(64);
    public List<QuestData> completableQuests { get; private set; } = new List<QuestData>(64);
    public List<QuestData> completeQuests { get; private set; } = new List<QuestData>(64);
    public HashSet<int> blockedQuestIds { get; private set; } = new HashSet<int>(64);
    public HashSet<int> unavailableQuestIds { get; private set; } = new HashSet<int>(64); //진행,완료가능,완료 퀘스트 모음
    public Dictionary<int, Dictionary<int, bool>> talkDatas { get; private set; } = new Dictionary<int, Dictionary<int, bool>>();
    public Dictionary<int, Dictionary<int,int>> huntDatas { get; private set; }  = new Dictionary<int, Dictionary<int, int>>();
    // 1번 딕셔너리 키 = 퀘스트id
    // 2번 딕셔너리 키 = 몬스터id
    // 2번 딕셔너리 값 = 몬스터 사냥수
    Dictionary<int, List<int>> monsterToQuestMap = new Dictionary<int, List<int>>(64);
    public List<QuestData> activeQuests { get; private set; } = new List<QuestData>(128); //진행,완료가능퀘스트 모음
    #endregion

    #region NPC 퀘스트 리스트
    //아래 3개 리스트는 npc와 대화 시 적용시킬 퀘스트 id 리스트
    public List<int> currentCompletableQuestIds { get; private set; } = new List<int>(32);
    public List<int> currentInprogressableQuestIds { get; private set; } = new List<int>(32);
    public List<int> currentStartableQuestIds { get; private set; } = new List<int>(32);
    #endregion
    List<int> emptyMonsterIds  = new List<int>(16);
    [SerializeField] PlayerStatus playerStatus;
    public void LoadCompleteQuests(List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            completeQuests.Add(QuestDataBase.QuestDB[_list[i]]);
            unavailableQuestIds.Add(_list[i]);
        }
    }
    public void LoadCompletableQuests(List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            completableQuests.Add(QuestDataBase.QuestDB[_list[i]]);
            activeQuests.Add(QuestDataBase.QuestDB[_list[i]]);
            unavailableQuestIds.Add(_list[i]);
        }
    }
    public void LoadInProgressQuests(List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            inProgressQuests.Add(QuestDataBase.QuestDB[_list[i]]);
            activeQuests.Add(QuestDataBase.QuestDB[_list[i]]);
            unavailableQuestIds.Add(_list[i]);
        }
    }
    public void LoadBlockedQuestIds(List<int> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            blockedQuestIds.Add(_list[i]);
        }
    }
    public void LoadTalkDatas(Dictionary<int, Dictionary<int, bool>> _data)
    {
        talkDatas = _data;
    }
    public void LoadHuntDatas(Dictionary<int, Dictionary<int, int>> _data)
    {
        huntDatas = _data;
        monsterToQuestMap.Clear();
        foreach (var questEntry in huntDatas)
        {
            int questId = questEntry.Key;
            var monsterDict = questEntry.Value;

            foreach(int monsterId in monsterDict.Keys)
            {
                if (!monsterToQuestMap.TryGetValue(monsterId, out var questList))
                {
                    questList = new List<int>();
                    monsterToQuestMap[monsterId] = questList;
                }

                questList.Add(questId);
            }
        }
    }
    public void SetQuestData()
    {
        var quests = QuestDataBase.QuestDB.Values;
        foreach(var quest in quests)
        {
            if (inProgressQuests.Contains(quest) || completableQuests.Contains(quest) || completeQuests.Contains(quest) || blockedQuestIds.Contains(quest.questId))
            {
                continue;
            }
            if (IsQuestStartable(quest))
            {
                startableQuests.Add(quest);
            }
            else
            {
                notStartQuests.Add(quest);
            }
        }
    }
    public void SetStartableQuest() // 퀘스트 클리어 or 레벨업시 적용
    {
        for(int i = notStartQuests.Count - 1; i >= 0; i--)
        {
            if (IsQuestStartable(notStartQuests[i]))
            {
                startableQuests.Add(notStartQuests[i]);
                notStartQuests.Remove(notStartQuests[i]);
            }
        }
    }
    public void SetblockedQuest()
    {
        for( int i = startableQuests.Count - 1; i >= 0; i--)
        {
            if (!IsQuestIncompatible(startableQuests[i]))
            {
                blockedQuestIds.Add(startableQuests[i].questId);
                startableQuests.Remove(startableQuests[i]);
            }
        }
    }
    public void SetStartableQuestId(int _npcId)
    {
        currentStartableQuestIds.Clear();
        for(int i = 0; i < startableQuests.Count; i++)
        {
            QuestData questData = startableQuests[i];
            if (questData.questId / 100 == _npcId)
            {
                currentStartableQuestIds.Add(questData.questId);
            }
        }
    }
    public void SetInprogressableQuestId(int _npcId)
    {
        currentInprogressableQuestIds.Clear();
        for(int i = 0; i < inProgressQuests.Count; i++)
        {
            QuestData questData = inProgressQuests[i];
            if (questData.ids.Contains(_npcId) && !talkDatas[questData.questId][_npcId])
            {
                currentInprogressableQuestIds.Add(questData.questId);
            }
        }
    }
    public void SetCompletableQuestId(int _npcId)
    {
        IsQuestCompletable(_npcId);
        currentCompletableQuestIds.Clear();
        for (int i = 0; i < completableQuests.Count; i++)
        {
            QuestData questData = completableQuests[i];
            if (questData.completeNpcId == _npcId)
            {
                currentCompletableQuestIds.Add(questData.questId);
            }
        }
    }
    public bool IsQuestStartable(QuestData _quest)
    {
        if (_quest.constraintLevel > playerStatus.Level)
        {
            return false;
        }

        if (_quest.preQuestIds != null && _quest.preQuestIds.Count > 0)
        {
            foreach (int questId in _quest.preQuestIds)
            {
                bool preQuestCompleted = false;

                for (int i = 0; i < completeQuests.Count; i++)
                {
                    if (completeQuests[i].questId == questId)
                    {
                        preQuestCompleted = true;
                        break;
                    }
                }

                if (!preQuestCompleted)
                {
                    return false;
                }
            }
        }

        return IsQuestIncompatible(_quest);
    }
    bool IsQuestIncompatible(QuestData _quest)
    {
        if (_quest.incompatibleQuestIds != null && _quest.incompatibleQuestIds.Count > 0)
        {
            foreach (int questId in _quest.incompatibleQuestIds)
            {
                if (unavailableQuestIds.Contains(questId))
                {
                    return false;
                }
            }
        }
        return true;
    }
    void IsQuestCompletable(int _npcid)
    {
        for (int i = inProgressQuests.Count - 1; i >= 0; i--)
        {
            if (inProgressQuests[i].completeNpcId == _npcid)
            {
                if (IsQuestCompletable(inProgressQuests[i]))
                {
                    completableQuests.Add(inProgressQuests[i]);
                    inProgressQuests.Remove(inProgressQuests[i]);
                }
            }
        }
    }
    public bool IsQuestCompletable(QuestData _quest)
    {
        for (int i = 0; i < _quest.ids.Count; i++)
        {
            switch (CustomUtility.GetDigitCount(_quest.ids[i]))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    if (!talkDatas[_quest.questId][_quest.ids[i]])
                    {
                        return false;
                    }
                    break;
                case 5:
                    if (huntDatas[_quest.questId][_quest.ids[i]] < _quest.counts[i])
                    {
                        return false;
                    }
                    break;
                case 9:
                    if (InventoryData.Instance.GetItemCount(_quest.ids[i]) < _quest.counts[i])
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }
    public void StartQuest(int _questId)
    {
        QuestData quest = QuestDataBase.QuestDB[_questId];
        startableQuests.Remove(quest);
        List<int> ids = quest.ids;
        switch (quest.questType)
        {
            case QuestType.Talk:
                InitTalkQuestData(_questId, ids);
                break;
            case QuestType.Hunt:
                InitHuntQuestData(_questId, ids);
                break;
            case QuestType.Mix:
                InitMixQuestData(_questId, ids);
                break;
        }
        inProgressQuests.Add(quest);
        activeQuests.Add(quest);
        unavailableQuestIds.Add(_questId);
        SetblockedQuest();
        DataManager.Instance.SaveQuest();
    }
    private void InitTalkQuestData(int _questId, List<int> _ids)
    {
        Dictionary<int, bool> talkdata = new Dictionary<int, bool>(_ids.Count);
        for (int i = 0; i < _ids.Count; i++)
        {
            talkdata[_ids[i]] = false;
        }
        talkDatas.Add(_questId, talkdata);
    }
    private void InitHuntQuestData(int _questId, List<int> _ids)
    {
        Dictionary<int, int> huntData = new Dictionary<int, int>(_ids.Count);
        for (int i = 0; i < _ids.Count; i++)
        {
            int monsterId = _ids[i];

            huntData[monsterId] = 0;
            if (!monsterToQuestMap.TryGetValue(monsterId, out var quests))
            {
                quests = new List<int>();
                monsterToQuestMap[monsterId] = quests;
            }
            quests.Add(_questId);
        }
        huntDatas.Add(_questId, huntData);
    }
    private void InitMixQuestData(int _questId, List<int> _ids)
    {
        Dictionary<int, int> huntData = new Dictionary<int, int>();
        for (int i = 0; i < _ids.Count; i++)
        {
            int id = _ids[i];

            if(id < 100000000)
            {
                huntData[id] = 0;
                if (!monsterToQuestMap.TryGetValue(id, out var quests))
                {
                    quests = new List<int>();
                    monsterToQuestMap[id] = quests;
                }
                quests.Add(_questId);
            }
        }
        if(huntData.Count > 0)
        {
            huntDatas[_questId] = huntData;
        }
    }
    public void CompleteQuest(int _questId)
    {
        huntDatas.Remove(_questId);
        talkDatas.Remove(_questId);
        emptyMonsterIds.Clear();

        foreach (var kvp in monsterToQuestMap)
        {
            var questList = kvp.Value;
            questList.Remove(_questId);
            
            if (questList.Count == 0)
            {
                emptyMonsterIds.Add(kvp.Key);
            }
        }

        for(int  i = 0; i < emptyMonsterIds.Count; i++)
        {
            monsterToQuestMap.Remove(emptyMonsterIds[i]);
        }

        if(QuestDataBase.QuestDB.TryGetValue(_questId, out var quest))
        {
            completableQuests.Remove(quest);
            activeQuests.Remove(quest);
            completeQuests.Add(quest);
        }
        

        SetStartableQuest();
        GetQuestRewards(_questId);
        DataManager.Instance.SaveQuest();
    }
    void GetQuestRewards(int _questId)
    {
        QuestRewards rewards = QuestDataBase.RewardDB[_questId];

        playerStatus.Exp += rewards.exp;
        InventoryData.Instance.GetGold(rewards.gold);

        for(int i = 0; i < rewards.itemIds.Count; i++)
        {
            InventoryData.Instance.GetItem(rewards.itemIds[i], rewards.itemAmounts[i]);
        }

        if (QuestDataBase.SepcialRewardDB.TryGetValue(_questId, out QuestSpecialRewards specialRewards))
        {
            switch (specialRewards.rewardType)
            {
                case 1:
                    playerStatus.ClassChange(specialRewards.rewardId);
                    break;
                case 2:
                    playerStatus.ClassRankUp();
                    break;
            }
        }

        DataManager.Instance.SaveInventory();
    }
    public void Hunt(int _monsterId)
    {
        bool check = false;

        if(!monsterToQuestMap.TryGetValue(_monsterId, out var questList))
        {
            return;
        }

        for(int i = 0; i < questList.Count; i++)
        {
            int questId = questList[i];
            if(huntDatas.TryGetValue(questId, out var huntData))
            {
                huntData[_monsterId]++;
                check = true;
            }
        }

        if (check)
        {
            DataManager.Instance.SaveQuest();
        }
    }
    public void Talk(int _questId, int _npcId)
    {
        talkDatas[_questId][_npcId] = true;
        DataManager.Instance.SaveQuest();
    }
}
