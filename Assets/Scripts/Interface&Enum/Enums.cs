public enum ValueOperator
{
    Equal,
    NotEqual,
    Greater,
    Lower,
    GreaterOrEqual,
    LowerOrEqual
}
public enum GameMode
{
    ControllMode = 0,
    UIMode,
    ForcedUIMode,
    NotControllable
}

public enum StateType
{
    Idle,
    Walk,
    Run,
    Dash,
    Jump,
    Fall,
    Land,
    Roll,
    Attack,
    Buff,
    ActiveSkill,
    Hit,
    Die
}

public enum DragUIType
{
    Inventory = 0,
    Equipment,
    Skill
}

public enum ItemType
{
    Equipment = 0,
    Useable,
    Misc
}

public enum DropType
{
    Gold = 0,
    Equipment,
    Useable,
    Misc
}
public enum CraftType
{
    Materials = 0,
    Result
}

public enum EquipmentType
{
    Weapon = 0,
    Armor,
    Pants,
    Gloves,
    Shoes,
    Ring
}

public enum StatusType
{
    Level = 0,
    //기본스탯
    Strength,
    Dexterity,
    Vitality,
    Intelligence,
    Wisdom,
    Agility,
    //보정스탯
    PhysicalAttackPower,
    MagicAttackPower,
    PhysicalDefensePower,
    MagicDefensePower,
    HpRegen,
    MpRegen,
    Accuracy,
    Evasion,
    Hp,
    Mp,
    Stamina,
    //이 아래는 퍼센트가 기본값이라 *100을 한 수치를 저장해야합니다.
    CriticalRate,
    CriticalDamage,
    WeaponMastery,
    AttackSpeed,
    MoveSpeed,
    CooltimeReduce
}
public enum StatusInformationType
{
    Normal,
    Percentage,
    CurrentMax,
    Total
}

public enum BTResult
{
    Success,
    Running,
    Failure
}

public enum QuestType
{
    Talk = 1,
    Hunt,
    Collect,
    Mix // Hunt+Collect
}

public enum QuestProgress
{
    NotStarted,
    InProgress,
    Completed
}

public enum UseType
{
    Null,
    Item,
    Skill
}
public enum BuffType
{
    Null,
    HpRecovery,
    MpRecovery,
    HpPercentRecovery,
    MpPercentRecovery,
    StrengthIncrease,
    DexterityIncrease,
    VitalityIncrease,
    IntelligenceIncrease,
    WisdomIncrease,
    AgilityIncrease,
    AllStatIncrease,
    PhysicalAttackPowerIncrease,
    MagicAttackPowerIncrease,
    PhysicalDefensePowerIncrease,
    MagicDefensePowerIncrease,
    AccuracyIncrease,
    EvasionIncrease,
    MaxHpIncrease,
    MaxMpIncrease,
    MaxStaminaIncrease,
    CriticalRateIncrease,
    CriticalDamageIncrease,
    WeaponMasteryIncrease,
    AttackSpeedIncrease,
    MoveSpeedIncrease,
    CooltimeReduce
}