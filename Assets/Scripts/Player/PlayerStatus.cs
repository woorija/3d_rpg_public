using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    // 고정변수
    public const float MoveSpeed = 3f;
    public const float RunSpeed = 2f;
    public const float JumpForce = 4.5f;
    public const float Gravity = -9.81f;

    // 변동 클래스변수
    public int gender = 1; // 1남 2여
    public int playerClass;
    public int classRank;
    [field: SerializeField] public StatusCorrectionTableSO correctionTable;

    // 변동 상태변수
    public bool IsDie { get; private set; } = false;
    private float moveSpeedMultiplier = 1f;
    public float MoveSpeedMultiplier
    {
        get
        {
            return moveSpeedMultiplier;
        }
        private set
        {
            if (moveSpeedMultiplier != value)
            {
                moveSpeedMultiplier = value;
                onMoveSpeedMultiplierChanged?.Invoke(moveSpeedMultiplier);
            }
        }
    }
    float actionSpeedMultiplier;
    public float ActionSpeedMultiplier
    {
        get
        {
            return actionSpeedMultiplier;
        }
        private set
        {
            if(actionSpeedMultiplier != value)
            {
                actionSpeedMultiplier = value;
                onActionSpeedMultiplierChanged?.Invoke(actionSpeedMultiplier);
            }
        }
    }
    float cooltimeReduceMultipler;
    public float CooltimeReduceMultipler
    {
        get
        {
            return cooltimeReduceMultipler;
        }
        private set
        {
            if(cooltimeReduceMultipler != value)
            {
                cooltimeReduceMultipler = value;
                onCooltimeReduceMultiplerChanged?.Invoke(cooltimeReduceMultipler);
            }
        }
    }

    float exhaustTime = 0f;
    public float ExhaustTime
    {
        get => exhaustTime;
        set
        {
            exhaustTime = Mathf.Max(exhaustTime, value);
        }
    }
    public bool IsInvincible { get; set; } = true;
    public bool IsSuperArmor { get; set; } = false;
    public int ArmorBreakLevel { get; private set; }

    //스탯 테이블
    StatusType[] baseStatusTypes = new[]
    {
        StatusType.Strength,
        StatusType.Dexterity,
        StatusType.Intelligence,
        StatusType.Wisdom,
        StatusType.Vitality,
        StatusType.Agility
    };
    StatusType[] correctionStatusTypes = new[]
    {
        StatusType.PhysicalAttackPower,
        StatusType.PhysicalAttackPower,
        StatusType.MagicAttackPower,
        StatusType.PhysicalDefensePower,
        StatusType.MagicDefensePower,
        StatusType.HpRegen,
        StatusType.MpRegen,
        StatusType.Accuracy,
        StatusType.Evasion,
        StatusType.Hp,
        StatusType.Mp,
        StatusType.Stamina,
        StatusType.CriticalRate,
        StatusType.CriticalDamage,
        StatusType.WeaponMastery,
        StatusType.AttackSpeed,
        StatusType.MoveSpeed,
        StatusType.CooltimeReduce
    };
    StatusType[] allStatusTypes;

    Dictionary<StatusType, BaseStatusCorrection> baseStatusTable = new Dictionary<StatusType, BaseStatusCorrection>();
    Dictionary<StatusType, List<StatusCorrection>> subToBaseTable = new Dictionary<StatusType, List<StatusCorrection>>();
   
    // 변동 스탯변수
    public StatusDictionary baseStats = new StatusDictionary(); // 레벨, 직업별 기본 수치
    public StatusDictionary bonusStats = new StatusDictionary(); // 그 외 고정값으로 오르는 수치
    public StatusDictionary percentStats = new StatusDictionary(); // 그 외 퍼센트로 오르는 수치
    public StatusDictionary finalStats = new StatusDictionary(); // 실 적용 수치 = (베이스+보너스)*(1+퍼센트)
    
    // 변동 스탯변수
    int level = 1;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            onStatusChangeEvent?.Invoke(StatusType.Level);
        }
    }
    int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = Mathf.Clamp(value, 0, finalStats[StatusType.Hp]);
            if (hp == 0)
            {
                IsDie = true;
                OnDie.Invoke();
            }
            hpChangeEvent?.Invoke();
            onStatusChangeEvent?.Invoke(StatusType.Hp);
        }
    }
    int mp;
    public int Mp
    {
        get
        {
            return mp;
        }
        set
        {
            mp = Mathf.Clamp(value, 0, finalStats[StatusType.Mp]);
            mpChangeEvent?.Invoke();
            onStatusChangeEvent?.Invoke(StatusType.Mp);
        }
    }
    int exp;
    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            while (exp >= MaxExp)
            {
                LevelUp();
                exp -= MaxExp;
                MaxExp = ExpTable.expTable[level];
            }
            expChangeEvent?.Invoke();
        }
    }
    int maxExp = 1;
    public int MaxExp
    {
        get
        {
            return maxExp;
        }
        set
        {
            maxExp = value;
            maxExpChangeEvent?.Invoke();
        }
    }
    float stamina;
    public float Stamina
    {
        get
        {
            return stamina;
        }
        set
        {
            float prevStamina = stamina;
            int maxStamina = finalStats[StatusType.Stamina];
            stamina = Mathf.Clamp(value, 0f, maxStamina);

            if (prevStamina != stamina)
            {
                staminaChangeEvent?.Invoke();
                if (stamina == maxStamina && prevStamina != maxStamina)
                {
                    staminaMaxEvent?.Invoke();
                }
                else if (stamina != maxStamina && prevStamina == maxStamina)
                {
                    staminaNotMaxEvent?.Invoke();
                }
            }
        }
    }

    int remainingAbilityPoint;
    public int RemainingAbilityPoint {
        get
        {
            return remainingAbilityPoint;
        }
        private set 
        {
            remainingAbilityPoint = value;
            onRemainingAPChangeEvent?.Invoke(remainingAbilityPoint);
        }
    }
    public List<int> allocatedAbilityPoints { get; private set; }
    public DamageCoefficient damageCoefficient { get; private set; }

    // 이벤트
    public event Action<float> onMoveSpeedMultiplierChanged;
    public event Action<float> onActionSpeedMultiplierChanged;
    public event Action<float> onCooltimeReduceMultiplerChanged;

    public event Action OnHit;
    public event Action OnDie;

    public Action onLevelUpEvent;
    public Action onClassRankUpEvent;
    event Action<StatusType> onStatusChangeEvent;
    public event Action<int> onRemainingAPChangeEvent;

    event Action onPhysicalAttackPowerChanged;
    event Action onMagicAttackPowerChanged;
    event Action onWeaponMasteryChanged;
    event Action onCriticalRateChanged;
    event Action onCriticalDamageChanged;
    event Action onAccuracyChanged;

    event Action onMoveSpeedChanged;
    event Action onActionSpeedChanged;
    event Action onCooltimeReduceChanged;

    public UnityEvent hpChangeEvent;
    public UnityEvent mpChangeEvent;
    public UnityEvent expChangeEvent;
    public UnityEvent staminaChangeEvent;
    public UnityEvent staminaMaxEvent;
    public UnityEvent staminaNotMaxEvent;

    public UnityEvent maxHpChangeEvent;
    public UnityEvent maxMpChangeEvent;
    public UnityEvent maxExpChangeEvent;
    public UnityEvent maxStaminaChangeEvent;

    // 메서드
    private void Awake()
    {
        allStatusTypes = (StatusType[])Enum.GetValues(typeof(StatusType));
        foreach (var statusType in allStatusTypes)
        {
            baseStats[statusType] = 0;
            bonusStats[statusType] = 0;
            percentStats[statusType] = 0;
            finalStats[statusType] = 0;
        }
        allocatedAbilityPoints = new List<int>();
        for(int i = 0; i < baseStatusTypes.Length; i++)
        {
            allocatedAbilityPoints.Add(0);
        }
        damageCoefficient = new DamageCoefficient();
    }
    public void AddStatusEvent(Action<StatusType> _event)
    {
        onStatusChangeEvent += _event;
    }

    public void GetBonusStatus(StatusType _type, int _value)
    {
        bonusStats[_type] += _value;
    }

    public void GetAllEquipmentStatus()
    {
        var equipmentStatus = EquipmentData.Instance.GetAllEquipmentStatus();

        foreach (var statusType in allStatusTypes)
        {
            if(equipmentStatus.TryGetValue(statusType, out int value))
            {
                GetBonusStatus(statusType, value);
            }
        }
    }

    void CalculateFinalStatus(StatusType _type)
    {
        finalStats[_type] = (int)((baseStats[_type] + bonusStats[_type]) * (1 + percentStats[_type] * 0.01f));
        onStatusChangeEvent?.Invoke(_type);
    }
    public void StaminaUpdate()
    {
        if (exhaustTime <= 0f)
        {
            Stamina += Time.deltaTime * 10;
        }
        exhaustTime -= Time.deltaTime;
    }
    void EventInit()
    {
        baseStats.RegisterGlobalEvent(CalculateFinalStatus);
        bonusStats.RegisterGlobalEvent(CalculateFinalStatus);
        percentStats.RegisterGlobalEvent(CalculateFinalStatus);

        onPhysicalAttackPowerChanged = () => damageCoefficient.SetMaxPhysicalDamage(finalStats[StatusType.PhysicalAttackPower]);
        onMagicAttackPowerChanged = () => damageCoefficient.SetMaxMagicalDamage(finalStats[StatusType.MagicAttackPower]);
        onWeaponMasteryChanged = () => damageCoefficient.SetWeaponMastery(finalStats[StatusType.WeaponMastery]);
        onCriticalRateChanged = () => damageCoefficient.SetCriticalRate(finalStats.PercentageReturn(StatusType.CriticalRate));
        onCriticalDamageChanged = () => damageCoefficient.SetCriticalDamage(finalStats.PercentageReturn(StatusType.CriticalDamage));
        onAccuracyChanged = () => damageCoefficient.SetAccuracy(finalStats[StatusType.Accuracy]);
        onMoveSpeedChanged = () => CalculateMoveSpeed(finalStats.PercentageReturn(StatusType.MoveSpeed));
        onActionSpeedChanged = () => CalculateActionSpeed(finalStats.PercentageReturn(StatusType.AttackSpeed));
        onCooltimeReduceChanged = () => CalculateCooltimeReduce(finalStats.PercentageReturn(StatusType.CooltimeReduce));
        onLevelUpEvent += () => CalculateSubStat(StatusType.Hp);
        onLevelUpEvent += () => CalculateSubStat(StatusType.Mp);
        onLevelUpEvent += () => CalculateSubStat(StatusType.Stamina);

        for (int i = 0; i < baseStatusTypes.Length; i++)
        {
            StatusType statusType = baseStatusTypes[i];
            finalStats.RegisterStatusEvent(statusType, () => ChangeBaseStatus(statusType));
        }
        finalStats.RegisterGlobalEvent(onStatusChangeEvent);

        finalStats.RegisterStatusEvent(StatusType.Hp, maxHpChangeEvent);
        finalStats.RegisterStatusEvent(StatusType.Mp, maxMpChangeEvent);
        finalStats.RegisterStatusEvent(StatusType.Stamina, maxStaminaChangeEvent);
        finalStats.RegisterStatusEvent(StatusType.PhysicalAttackPower, onPhysicalAttackPowerChanged);
        finalStats.RegisterStatusEvent(StatusType.MagicAttackPower, onMagicAttackPowerChanged);
        finalStats.RegisterStatusEvent(StatusType.WeaponMastery, onWeaponMasteryChanged);
        finalStats.RegisterStatusEvent(StatusType.CriticalRate, onCriticalRateChanged);
        finalStats.RegisterStatusEvent(StatusType.CriticalDamage, onCriticalDamageChanged);
        finalStats.RegisterStatusEvent(StatusType.Accuracy, onAccuracyChanged);
        finalStats.RegisterStatusEvent(StatusType.MoveSpeed, onMoveSpeedChanged);
        finalStats.RegisterStatusEvent(StatusType.AttackSpeed, onActionSpeedChanged);
        finalStats.RegisterStatusEvent(StatusType.CooltimeReduce, onCooltimeReduceChanged);
    }
    public async void Init()
    {
        gender = 1;
        playerClass = 1;
        classRank = 0;

        EventInit();
        baseStats[StatusType.Hp] = 1; // 게임 시작시 캐릭터 죽음을 방지하기 위함
        Level = 1;

        correctionTable = await AddressableManager.Instance.LoadAssetAsync<StatusCorrectionTableSO>($"{playerClass:00}_{classRank:00}");
        SetStatusTable();
        CalculateCooltimeReduce(0);
        SetBaseStatus();
        InitCorrectionStatus();
        SetRemainingAP();

        Hp = finalStats[StatusType.Hp];
        Mp = finalStats[StatusType.Mp];
        MaxExp = ExpTable.expTable[Level];
        Stamina = 100;
        Exp = 0;

        ArmorBreakLevel = 1;
    }

    public void Hit(int _damage)
    {
        if (IsInvincible) return;
        Hp -= _damage;
        if (!IsSuperArmor && !IsDie)
        {
            OnHit.Invoke();
        }
    }
    void LevelUp()
    {
        Level++;
        for (int i = 0; i < baseStatusTypes.Length; i++)
        {
            baseStats[baseStatusTypes[i]] += correctionTable.levelUpStatus[i];
        }
        RemainingAbilityPoint += correctionTable.levelUpStatus[6];
        onLevelUpEvent?.Invoke();

        Hp = finalStats[StatusType.Hp];
        Mp = finalStats[StatusType.Mp];

        SkillData.Instance.GetLevelUpSP();
        QuestManager.Instance.SetStartableQuest();
    }
    /// <summary>
    /// 전직 퀘스트를 클리어하면 발생하는 메서드
    /// </summary>
    /// <param name="_id">퀘스트 보상 id</param>
    public void ClassChange(int _id = 0)
    {
        ResetBaseStauts();
        // 이전 클래스 데이터 제거
        AddressableManager.Instance.ReleaseAsset($"{playerClass:00}_{classRank:00}");
        switch (_id)
        {
            case 0:
                ClassRankUp();
                break;
            case 1:
                ClassChange(2, 1);
                break;
            case 2:
                ClassChange(3, 1);
                break;
            case 3:
                ClassChange(4, 1);
                break;
            case 4:
                ClassChange(5, 1);
                break;
            case 5:
                ClassChange(6, 1);
                break;
        }
    }

    public void ClassChange(int _class, int _rank)
    {
        playerClass = _class;
        classRank = _rank;
        ClassRankChangeEvent();
    }
    public void ClassRankUp()
    {
        classRank++;
        ClassRankChangeEvent();
    }
    async void ClassRankChangeEvent()
    {
        SkillData.Instance.GetRankUpSP();
        SkillData.Instance.SetAcquiredSkills(playerClass, classRank);
        correctionTable = await AddressableManager.Instance.LoadAssetAsync<StatusCorrectionTableSO>($"{playerClass:00}_{classRank:00}");
        SetStatusTable();
        StatusReset();
        onClassRankUpEvent?.Invoke();
        Hp = finalStats[StatusType.Hp];
        Mp = finalStats[StatusType.Mp];
    }
    void SetStatusTable()
    {
        correctionTable.InitBaseStatusTable(baseStatusTable);
        correctionTable.InitSubToBaseTable(subToBaseTable);
    }
    void InitCorrectionStatus()
    {
        foreach(var status in correctionStatusTypes) 
        {
            CalculateSubStat(status);
        }
    }
    void ChangeBaseStatus(StatusType _baseStatus)
    {
        if(!baseStatusTable.TryGetValue(_baseStatus, out var baseStatusCorrection))
        {
            return;
        }

        var list = baseStatusCorrection.correctionList;
        for (int i = 0; i < list.Count; i++)
        {
            CalculateSubStat(list[i].statusType);
        }
    }
    void CalculateSubStat(StatusType _type)
    {
        int total = 0;

        if (_type == StatusType.Hp)
        {
            total += correctionTable.levelUpStatus[7] * level;
        }
        else if (_type == StatusType.Mp)
        {
            total += correctionTable.levelUpStatus[8] * level;
        }
        else if (_type == StatusType.Stamina)
        {
            total += 100 + level;
        }

        if (subToBaseTable.TryGetValue(_type, out var baseStatuses))
        {
            for (int i = 0; i < baseStatuses.Count; i++)
            {
                var statusCorrection = baseStatuses[i];
                int baseValue = finalStats[statusCorrection.statusType];
                total += (int)(baseValue * statusCorrection.valuePerPoint);
            }
        }

        baseStats[_type] = total;
    }
    void CalculateMoveSpeed(float _moveSpeed)
    {
        MoveSpeedMultiplier = Mathf.Min(1 + _moveSpeed, 1.5f);
    }
    void CalculateActionSpeed(float _increaseActionSpeed)
    {
        const float min = 1.0f;
        const float max = 1.5f;
        const float maxValue = 100f;
        const float curveStrength = 3.2f;

        if(_increaseActionSpeed <= 0)
        {
            ActionSpeedMultiplier = min;
            return;
        }

        if(_increaseActionSpeed >= maxValue)
        {
            ActionSpeedMultiplier = max;
            return;
        }

        float normalized = _increaseActionSpeed / maxValue;
        float curved = Mathf.Pow(normalized, 1f / curveStrength);
        ActionSpeedMultiplier = Mathf.Lerp(min, max, curved);
    }
    void CalculateCooltimeReduce(float _reduce)
    {
        const float min = 1.0f;
        const float max = 2.0f;
        const float maxValue = 500;
        const float curveStrength = 2.5f;

        if (_reduce <= 0)
        {
            CooltimeReduceMultipler = min;
            return;
        }
        if(_reduce >= maxValue)
        {
            CooltimeReduceMultipler = max;
            return;
        }

        float normalized = _reduce / maxValue;
        float curved = Mathf.Pow(normalized, 1f/ curveStrength);
        CooltimeReduceMultipler = Mathf.Lerp(min, max, curved);
    }
    void SetBaseStatus()
    {
        for (int i = 0; i < baseStatusTypes.Length; i++)
        {
            baseStats[baseStatusTypes[i]] = correctionTable.levelUpStatus[i] * level + allocatedAbilityPoints[i];
        }
    }
    void ResetBaseStauts()
    {
        for (int i = 0; i < baseStatusTypes.Length; i++)
        {
            baseStats[baseStatusTypes[i]] = 0;
        }
    }
    void SetRemainingAP()
    {
        RemainingAbilityPoint = correctionTable.levelUpStatus[6] * level;
    }
    void StatusReset()
    {
        for (int i = 0; i < baseStatusTypes.Length; i++)
        {
            baseStats[baseStatusTypes[i]] = correctionTable.levelUpStatus[i] * level;
            allocatedAbilityPoints[i] = 0;
        }
        RemainingAbilityPoint = correctionTable.levelUpStatus[6] * level;
    }
    public void StatAllocate(int _value)
    {
        allocatedAbilityPoints[_value]++;
        baseStats[baseStatusTypes[_value]]++;
        RemainingAbilityPoint--;
        DataManager.Instance.SavePlayer();
    }
    public void AllStatAllocate(int _value)
    {
        allocatedAbilityPoints[_value] += RemainingAbilityPoint;
        baseStats[baseStatusTypes[_value]] += RemainingAbilityPoint;
        RemainingAbilityPoint = 0;
        DataManager.Instance.SavePlayer();
    }
    // 데이터 메서드
    public async UniTask LoadPlayerData(int _gender, int _class, int _rank, int _level)
    {
        gender = _gender;
        playerClass = _class;
        classRank = _rank;
        EventInit();
        Level = _level;
        MaxExp =  ExpTable.expTable[level];
        correctionTable = await AddressableManager.Instance.LoadAssetAsync<StatusCorrectionTableSO>($"{playerClass:00}_{classRank:00}");
        SetStatusTable();
    }
    public void LoadBaseStatusData(List<int> _allocatedAP, int _remainingAP)
    {
        allocatedAbilityPoints = _allocatedAP;
        RemainingAbilityPoint = _remainingAP;
        SetBaseStatus();
        CalculateCooltimeReduce(0);
        InitCorrectionStatus();
        GetAllEquipmentStatus();
    }
    public void LoadCurrentStatusData(int _hp, int _mp, int _exp, float _stamina)
    {
        Hp = _hp;
        Mp = _mp;
        Exp = _exp;
        Stamina = _stamina;
    }
}
