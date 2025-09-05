using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : SingletonBehaviour<SkillData>
{
    int skillPoint;
    public int SkillPoint
    {
        get
        {
            return skillPoint; 
        }
        private set
        {
            skillPoint = value;
            onSPChangedEvent?.Invoke(skillPoint); 
        } 
    }
    public Dictionary<int, int> acquiredSkillLevels { get; private set; } = new Dictionary<int, int>(128);
    public Dictionary<int, int> prevSkillLevels { get; private set; } = new Dictionary<int, int>(128); // 선행스킬 레벨 감소 제한을 두기 위함
    public List<int> currentRankFilterSkills { get; private set; } = new List<int>(32);
    
    public List<int> saveSkillIdList { get; private set; } = new List<int>();
    public List<int> saveSkillLevelList { get; private set; } = new List<int>();
    public Action<int> onSPChangedEvent;

    [SerializeField] SkillMainUI skillMainUI;
    [SerializeField] PlayerEffectManager playerEffectManager;
    public void GetLevelUpSP()
    {
        SkillPoint += 20;
    }
    public void GetRankUpSP()
    {
        SkillPoint += 100;
    }
    public void RankFilter(int _rank)
    {
        currentRankFilterSkills.Clear();
        foreach(int id in acquiredSkillLevels.Keys)
        {
            if (id / 10000 % 10 == _rank && id >= 100000) // 기본공격 예외처리
            {
                currentRankFilterSkills.Add(id);
            }
        }
    }
    public void ResetSkill(int _level, int _rank)
    {
        SkillPoint = (_level - 1) * 20 + _rank * 100;
        foreach(int id in acquiredSkillLevels.Keys)
        {
            acquiredSkillLevels[id] = 0;
        }
    }
    public void SetAcquiredSkills(int _class, int _rank)
    {
        int id = _class * 10 + _rank;
        List<int> newSkillList = SkillDataBase.ClassSkillTable[id];

        playerEffectManager.SetClassEffect(id);
        playerEffectManager.SetNormalAttackEffect(id);

        foreach(int skillId in newSkillList)
        {
            acquiredSkillLevels.TryAdd(skillId, 0);
        }
        DataManager.Instance.SaveSkill();
    }
    public void IncreaseSkillLevel(int _id)
    {
        if (acquiredSkillLevels.ContainsKey(_id))
        {
            var skill = SkillDataBase.SkillDB[_id];
            if (acquiredSkillLevels[_id] < skill.masterLevel && skill.acquisitionSp <= SkillPoint)
            {
                SkillPoint -= skill.acquisitionSp;
                acquiredSkillLevels[_id]++;
                TryGetPassiveBuff(_id);
                if (acquiredSkillLevels[_id] == 1)
                {
                    SetPrevSkillLevel(skill.prevSkillId, skill.prevSkillLevel);
                }
                SkillInformationUI.Instance.SetSkillInfomations();
            }
            DataManager.Instance.SaveSkill();
        }
    }
    public void DecreaseSkillLevel(int _id)
    {
        if (acquiredSkillLevels.ContainsKey(_id))
        {
            if (acquiredSkillLevels[_id] >= 1)
            {
                var skill = SkillDataBase.SkillDB[_id];
                acquiredSkillLevels[_id]--;
                SkillPoint += skill.acquisitionSp;
                TryGetPassiveBuff(_id);
                if (acquiredSkillLevels[_id] == 0)
                {
                    RemovePrevSkillLevel(skill.prevSkillId);
                    QuickSlotData.Instance.ResetSlotToSkillLevelZero(_id);
                }
                SkillInformationUI.Instance.SetSkillInfomations();
            }
            DataManager.Instance.SaveSkill();
        }
    }
    public void SetPrevSkillLevel(int _id, int _level)
    {
        if (_id == 0) return;
        DevelopUtility.Log(_id + "/" + _level);
        prevSkillLevels.Add(_id, _level);
    }
    public void RemovePrevSkillLevel(int _id)
    {
        DevelopUtility.Log(_id);
        if (prevSkillLevels.ContainsKey(_id))
        {
            prevSkillLevels.Remove(_id);
        }
    }
    public int GetSkillMultiplier(int _id, int _index)
    {
        var skill = SkillDataBase.SkillDB[_id];
        return skill.initialSkillMultiplier[_index] + skill.increaseSkillMultiplier[_index] * acquiredSkillLevels[_id];
    }
    public int GetSkillUseMp(int _id)
    {
        var skill = SkillDataBase.SkillDB[_id];
        return (int)MathF.Floor(skill.initialMp + skill.increaseMp * acquiredSkillLevels[_id]);
    }
    public void Init()
    {
        onSPChangedEvent += skillMainUI.SetSpText;
        SkillPoint = 1000;
        onSPChangedEvent?.Invoke(SkillPoint);
        SetAcquiredSkills(0, 0);
        SetAcquiredSkills(1, 0);
    }
    public void SaveSkillData()
    {
        saveSkillIdList.Clear();
        saveSkillLevelList.Clear();

        foreach(var kvp in acquiredSkillLevels)
        {
            saveSkillIdList.Add(kvp.Key);
            saveSkillLevelList.Add(kvp.Value);
        }
    }
    public void LoadData(int _point, List<int> _ids, List<int> _levels)
    {
        onSPChangedEvent += skillMainUI.SetSpText;
        SkillPoint = _point;
        onSPChangedEvent.Invoke(SkillPoint);

        for (int i = 0; i < _ids.Count; i++)
        {
            acquiredSkillLevels.Add(_ids[i], _levels[i]);
        }

        int prevRank = 0;
        int currentRank;
        int maxRank = 0;
        foreach(var kvp in acquiredSkillLevels)
        {
            int skillId = kvp.Key;
            int skillLevel = kvp.Value;

            if(skillLevel >= 1)
            {
                Skill skill = SkillDataBase.SkillDB[skillId];
                SetPrevSkillLevel(skill.prevSkillId, skill.prevSkillLevel);
            }
            currentRank = skillId / 10000;
            if (prevRank != currentRank)
            {
                playerEffectManager.SetClassEffect(currentRank);
                prevRank = currentRank;
                maxRank = Math.Max(maxRank, currentRank);
            }
            TryGetPassiveBuff(skillId);
        }
        playerEffectManager.SetNormalAttackEffect(maxRank);
    }
    private void OnDestroy()
    {
        onSPChangedEvent -= skillMainUI.SetSpText;
    }
    void TryGetPassiveBuff(int _id)
    {
        if(SkillDataBase.SkillDB[_id].skillType == 1)
        {
            BuffManager.Instance.ApplyPassiveSkillBuff(_id);
        }
    }
}
