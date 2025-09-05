using System.Collections.Generic;
using UnityEngine;

public class BuffManager : SingletonBehaviour<BuffManager>
{
    private Dictionary<(BuffType, int), Buff> buffList;
    private Dictionary<Buff, BuffIcon> buffIconList;
    private Queue<(BuffType, int)> expiredBuffsQueue;

    public static readonly Dictionary<BuffType, StatusType> cachedBuffTypeMap = new Dictionary<BuffType, StatusType>
    {
        { BuffType.StrengthIncrease, StatusType.Strength },
        { BuffType.DexterityIncrease, StatusType.Dexterity },
        { BuffType.VitalityIncrease, StatusType.Vitality },
        { BuffType.IntelligenceIncrease, StatusType.Intelligence },
        { BuffType.WisdomIncrease, StatusType.Wisdom },
        { BuffType.AgilityIncrease, StatusType.Agility },
        { BuffType.PhysicalAttackPowerIncrease, StatusType.PhysicalAttackPower },
        { BuffType.MagicAttackPowerIncrease, StatusType.MagicAttackPower },
        { BuffType.PhysicalDefensePowerIncrease, StatusType.PhysicalDefensePower },
        { BuffType.MagicDefensePowerIncrease, StatusType.MagicDefensePower },
        { BuffType.AccuracyIncrease, StatusType.Accuracy },
        { BuffType.EvasionIncrease, StatusType.Evasion },
        { BuffType.MaxHpIncrease, StatusType.Hp },
        { BuffType.MaxMpIncrease, StatusType.Mp },
        { BuffType.MaxStaminaIncrease, StatusType.Stamina },
        { BuffType.CriticalRateIncrease, StatusType.CriticalRate },
        { BuffType.CriticalDamageIncrease, StatusType.CriticalDamage },
        { BuffType.WeaponMasteryIncrease, StatusType.WeaponMastery },
        { BuffType.AttackSpeedIncrease, StatusType.AttackSpeed },
        { BuffType.MoveSpeedIncrease, StatusType.MoveSpeed },
        { BuffType.CooltimeReduce, StatusType.CooltimeReduce }
    };

    int capacity = 64;

    [SerializeField] PlayerStatus playerStatus;
    protected override void Awake()
    {
        base.Awake();
        buffList = new Dictionary<(BuffType, int), Buff>(capacity);
        buffIconList = new Dictionary<Buff, BuffIcon>(capacity);
        expiredBuffsQueue = new Queue<(BuffType, int)>();
    }
    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var entry in buffList)
        {
            Buff buff = entry.Value;
            buff.Update(deltaTime);

            if (buff.currentDuration <= 0f)
            {
                ExpiredBuff(buff);
                expiredBuffsQueue.Enqueue(entry.Key);
            }
            else if (buff.currentInterval <= 0f)
            {
                GetBuff(buff);
                buff.IntervalUpdate();
            }
        }

        while (expiredBuffsQueue.Count > 0)
        {
            var key = expiredBuffsQueue.Dequeue();
            buffList.Remove(key);
        }
    }
    public void ApplyBuff(int _id)
    {
        if (CooltimeManager.Instance.IsCooltime(_id)) return; // 쿨타임 중이면 사용 불가

        int digitCount = CustomUtility.GetDigitCount(_id);
        if (digitCount == 9) // 아이템의 경우
        {
            ApplyItemBuff(_id);
        }
        else // 스킬의 경우
        {
            ApplySkillBuff(_id);
        }
    }
    void ApplyItemBuff(int _id)
    {
        ItemBuffData data = BuffDataBase.itemBuffDB[_id];
        InventoryData.Instance.RemoveItem(_id);
        DataManager.Instance.SaveInventory();

        if (data.duration < 0.01f) // 지속시간이 없는 버프인 경우
        {
            for(int i = 0; i< data.buffTypes.Count; i++)
            {
                DevelopUtility.Log($"즉발형 {_id}아이템 사용 타입{(BuffType)data.buffTypes[i]}, 옵션{data.buffOptions[i]}");
                GetBuff((BuffType)data.buffTypes[i], data.buffOptions[i]);
            }
        }
        else
        {
            for (int i = 0; i < data.buffTypes.Count; i++)
            {
                var key = ((BuffType)data.buffTypes[i], data.buffKey);
                if (buffList.TryGetValue(key, out Buff existingBuff))
                {
                    int existingOption = existingBuff.optionValue;
                    int newOption = data.buffOptions[i];

                    if (existingOption < newOption)
                    {
                        ExpiredBuff(existingBuff);
                        AddBuff(_id, key, (BuffType)data.buffTypes[i], data.buffKey, newOption, data.duration, data.interval);
                    }
                    else if (existingOption == newOption)
                    {
                        existingBuff.StartBuff();
                    }
                }
                else
                {
                    AddBuff(_id, key, (BuffType)data.buffTypes[i], data.buffKey, data.buffOptions[i], data.duration, data.interval);
                }
            }
        }
        if (data.coolTime >= 0.01f)
        {
            CooltimeManager.Instance.AddCooltime(_id, data.coolTime);
        }
    }

    void ApplySkillBuff(int _id)
    {
        SkillBuffData buffData = BuffDataBase.skillBuffDB[_id];
        Skill skillData = SkillDataBase.SkillDB[_id];
        int skillLevel = SkillData.Instance.acquiredSkillLevels[_id];

        for (int i = 0; i < buffData.buffTypes.Count; i++)
        {
            int buffOption = skillData.initialSkillMultiplier[i] + skillData.increaseSkillMultiplier[i] * skillLevel;
            var key = ((BuffType)buffData.buffTypes[i], buffData.buffKey);
            float interval = skillData.interval == 0f ? 999999999f : skillData.interval;
            if (buffList.TryGetValue(key, out Buff existingBuff))
            {
                int existingOption = existingBuff.optionValue;

                if (existingOption < buffOption)
                {
                    ExpiredBuff(existingBuff);
                    AddBuff(_id, key, (BuffType)buffData.buffTypes[i], buffData.buffKey, buffOption, skillData.duration, interval);
                }
                else if (existingOption == buffOption)
                {
                    existingBuff.StartBuff();
                }
            }
            else
            {
                AddBuff(_id, key, (BuffType)buffData.buffTypes[i], buffData.buffKey, buffOption, skillData.duration, interval);
            }
        }
        if (skillData.coolTime >= 0.01f)
        {
            CooltimeManager.Instance.AddCooltime(_id, skillData.coolTime);
        }
    }
    public void ApplyPassiveSkillBuff(int _id)
    {
        SkillBuffData buffData = BuffDataBase.skillBuffDB[_id];
        Skill skillData = SkillDataBase.SkillDB[_id];
        int skillLevel = SkillData.Instance.acquiredSkillLevels[_id];
        DevelopUtility.Log($"패시브버프{_id}Lv.{skillLevel}");
        if (skillLevel == 0)
        {
            for (int i = 0; i < buffData.buffTypes.Count; i++)
            {
                var key = ((BuffType)buffData.buffTypes[i], buffData.buffKey);
                ExpiredBuff(key);
                buffList.Remove(key);
            }
        }
        else
        {
            DevelopUtility.Log($"카운트{buffData.buffTypes.Count}");
            for (int i = 0; i < buffData.buffTypes.Count; i++)
            {
                int buffOption = skillData.initialSkillMultiplier[i] + skillData.increaseSkillMultiplier[i] * skillLevel;
                var key = ((BuffType)buffData.buffTypes[i], buffData.buffKey);
                DevelopUtility.Log($"옵션{buffOption} / 키{key}");
                if (buffList.TryGetValue(key, out Buff existingBuff))
                {
                    ExpiredBuff(existingBuff);
                    buffList.Remove(key);
                    AddBuff(_id, key, (BuffType)buffData.buffTypes[i], buffData.buffKey, buffOption, 999999999f, 999999999f);
                    DevelopUtility.Log($"레벨 추가증가 적용");
                }
                else
                {
                    AddBuff(_id, key, (BuffType)buffData.buffTypes[i], buffData.buffKey, buffOption, 999999999f, 999999999f);
                    DevelopUtility.Log($"1레벨 적용");
                }
            }
        }
    }

    private void AddBuff(int _id, (BuffType, int) tupleKey, BuffType type, int buffKey, int optionValue, float duration, float interval)
    {
        DevelopUtility.Log($"지속형 사용 타입{type}, 옵션{optionValue}, 지속시간{duration}, 인터벌{interval}");
        Buff buff = PoolManager.Instance.buffPool.Get();
        BuffIcon buffIcon = PoolManager.Instance.buffIconPool.Get();
        buffIconList.Add(buff, buffIcon);
        buff.SetBuff(type, buffKey, optionValue, duration, interval);
        buffIcon.SetBuff(buff, _id);
        GetBuff(buff);
        buff.StartBuff();
        buffList.Add(tupleKey, buff);
    }
    void GetBuff(BuffType _type, int _value) // 지속시간이 없는 버프를 Buff클래스 생성 없이 호출하기 위한 함수
    {
        switch(_type)
        {
            case BuffType.HpRecovery:
                playerStatus.Hp += _value;
                break;
            case BuffType.MpRecovery: 
                playerStatus.Mp += _value;
                break;
            case BuffType.HpPercentRecovery:
                playerStatus.Hp += (int)(playerStatus.finalStats[StatusType.Hp] * (float)(_value / 100));
                break;
            case BuffType.MpPercentRecovery:
                playerStatus.Mp += (int)(playerStatus.finalStats[StatusType.Mp] * (float)(_value / 100));
                break;
        }
    }
    void GetBuff(Buff _buff) // 지속시간이 있는 버프를 적용하기 위한 함수
    {
        BuffType buffType = _buff.buffType;
        switch (buffType)
        {
            case BuffType.HpRecovery:
                playerStatus.Hp += _buff.optionValue;
                DevelopUtility.Log($"HP{_buff.optionValue}회복");
                break;
            case BuffType.MpRecovery:
                playerStatus.Mp += _buff.optionValue;
                DevelopUtility.Log($"MP{_buff.optionValue}회복");
                break;
            case BuffType.AllStatIncrease:
                playerStatus.GetBonusStatus(StatusType.Strength, _buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Dexterity, _buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Vitality, _buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Intelligence, _buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Wisdom, _buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Agility, _buff.optionValue);
                DevelopUtility.Log($"올스텟 {_buff.optionValue}증가");
                break;
            default:
                if(cachedBuffTypeMap.TryGetValue(buffType, out var statusType))
                {
                    playerStatus.GetBonusStatus(statusType, _buff.optionValue);
                    DevelopUtility.Log($"{statusType} {_buff.optionValue}증가");
                }
                break;
        }
    }
    void ExpiredBuff(Buff _buff)
    {
        BuffType buffType = _buff.buffType;
        switch (buffType)
        {
            case BuffType.HpRecovery:
            case BuffType.MpRecovery:
                // 별도 효과 없음
                break;
            case BuffType.AllStatIncrease:
                playerStatus.GetBonusStatus(StatusType.Strength, -_buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Dexterity, -_buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Vitality, -_buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Intelligence, -_buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Wisdom, -_buff.optionValue);
                playerStatus.GetBonusStatus(StatusType.Agility, -_buff.optionValue);
                break;
            default:
                if (cachedBuffTypeMap.TryGetValue(buffType, out var statusType))
                {
                    playerStatus.GetBonusStatus(statusType, -_buff.optionValue);
                }
                break;
        }
        buffIconList[_buff].ReturnPool();
        buffIconList.Remove(_buff);
        _buff.Release();
    }
    void ExpiredBuff((BuffType, int) _key)
    {
        if(buffList.TryGetValue(_key, out var buff))
        {
            ExpiredBuff(buff);
        }
    }
}
