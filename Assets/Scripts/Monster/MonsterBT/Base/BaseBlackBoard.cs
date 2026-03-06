using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBlackBoard : MonoBehaviour, ISpawnInitializeable
{
    //모든 몬스터 블랙보드는 해당 클래스를 베이스로 상속받음
    [field: SerializeField] public NavMeshAgent agent { get; protected set; }

    [SerializeField] DropTableSO dropTable;
    [field: SerializeField] public MonsterBlackBoardSO blackBoardData { get; protected set; }
    public PlayerStatus player { get; protected set; }

    [Tooltip("보스몬스터는 비워둡니다."), SerializeField] Transform HUDTransform;
    [SerializeField] GameObject minimapMark;
    MonsterHUD monsterHUD;
    CapsuleCollider monsterCollider;

    [field: SerializeField] public bool isBoss {  get; protected set; }
    [Tooltip("스폰 위치 랜덤성 부여")][field: SerializeField] public float randomSpawnRange { get; protected set; } //자율이동범위
    

    public bool isReturn { get; protected set; }
    public float currentIdleTime { get; protected set; }
    public float currentRespawnTime { get; protected set; }

    public int hp { get; protected set; }
    public float staggerTime {  get; protected set; }
    int armorLevel;
    public bool isDie { get; protected set; }
    public Vector3 movePoint { get; protected set; } //자율이동 좌표
    public Vector3 spawnPointCenter { get; protected set; }
    DashData dashData;
    public float globalCooltime { get; protected set; }
    Dictionary<string, float> cooltimeMap;
    Dictionary<string, float> currentCooltimeMap;
    List<string> skillNameList;
    protected Dictionary<string, AttackDataSO> attackDataMap;

    public event Action<int> onHit;
    public Action<int> OnHpChanged; // hud changehp 연동
    protected void Awake()
    {
        player = FindAnyObjectByType<PlayerStatus>();
        monsterCollider = GetComponent<CapsuleCollider>();
        spawnPointCenter = transform.position;
        Init();
        ChangeMovePoint();
    }
    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;
        staggerTime -= deltaTime;
        currentRespawnTime -= deltaTime;
        globalCooltime -= deltaTime;
        for (int i = 0; i < skillNameList.Count; i++)
        {
            string skillName = skillNameList[i];
            currentCooltimeMap[skillName] -= deltaTime;
        }
        if (IsStand())
        {
            currentIdleTime -= deltaTime;
        }
    }
    protected virtual void Init()
    {
        cooltimeMap = new Dictionary<string, float>();
        currentCooltimeMap = new Dictionary<string, float>();
        attackDataMap = new Dictionary<string, AttackDataSO>();
        skillNameList = new List<string>();
        currentIdleTime = 0;
        isDie = false;
        hp = blackBoardData.maxHp;
        monsterCollider.enabled = true;
        minimapMark.SetActive(true);
        SetSkillData();
        RegisterSkill();
    }
    public void OnSpawn(TransformData _data)
    {
        transform.SetPositionAndRotation(_data.position, Quaternion.Euler(_data.rotation));
        transform.localScale = _data.scale;
        spawnPointCenter = transform.position;
        agent.enabled = true;
    }
    public void Hit(DamageCoefficient _damageCoefficient, int _hitCount, int _playerLevel, int _armorBreakLevel, float _staggerTime)
    {
        if (isDie) return; // 죽은상태면 처리 안함

        int totalDamage = 0;
        List<DamageData> damageList = new List<DamageData>();

        for (int i = 0; i < _hitCount; i++)
        {
            int damage = _damageCoefficient.GetDamage();
            bool isCritical;
            if(UnityEngine.Random.value <= _damageCoefficient.criticalRate)
            {
                damage = HitUtility.CalculatePlayerDamage((int)(damage * _damageCoefficient.criticalDamage), blackBoardData.level, _playerLevel);
                isCritical = true;
            }
            else
            {
                damage = HitUtility.CalculatePlayerDamage(damage, blackBoardData.level, _playerLevel);
                isCritical = false;
            }

            damageList.Add(new DamageData(damage, isCritical));
            totalDamage += damage;
        }

        if (_armorBreakLevel >= armorLevel) // 아머브레이크시
        {
            staggerTime = Mathf.Max(staggerTime, _staggerTime);
            onHit.Invoke(90);
        }

        hp -= totalDamage;
        OnHpChanged?.Invoke(hp);
        IsDie();

        PopupDamageList(damageList);
    }
    async void PopupDamageList(List<DamageData> _list)
    {
        foreach(DamageData data in _list)
        {
            if (data.isCritical)
            {
                DamageManager.Instance.PopupMonsterCriticalDamage(data.damage, transform.position);
            }
            else
            {
                DamageManager.Instance.PopupMonsterDamage(data.damage, transform.position);
            }
            await UniTask.Delay(100);
        }
    }
    void IsDie()
    {
        if (hp <= 0)
        {
            monsterCollider.enabled = false;
            minimapMark.SetActive(false);
            isDie = true;
            QuestManager.Instance.Hunt(blackBoardData.id);
            onHit.Invoke(99);
        }
    }
    public void ChangeArmorLevel(int _level)
    {
        armorLevel = _level;
    }

    public void ChangeMovePoint()
    {
        ChangeIdleTime();
        Vector2 randomInCircle = UnityEngine.Random.insideUnitCircle;
        movePoint = new Vector3(randomInCircle.x * randomSpawnRange + spawnPointCenter.x, transform.position.y, randomInCircle.y * randomSpawnRange + spawnPointCenter.z);
    }
    public void ChangeReturn(bool _bool)
    {
        isReturn = _bool;
    }
    public float GetSkillCooltime(string _name)
    {
        if(currentCooltimeMap.TryGetValue(_name, out var currentSkillCooltime))
        {
            return currentSkillCooltime;
        }
        return 99999999f;
    }
    public void ResetSkillCooltime(string _name)
    {
        if (cooltimeMap.ContainsKey(_name))
        {
            currentCooltimeMap[_name] = cooltimeMap[_name];
        }
    }
    protected void RegisterSkill(string _name, float _cooltime)
    {
        cooltimeMap[_name] = _cooltime;
        currentCooltimeMap[_name] = 0f;
        skillNameList.Add(_name);
    }
    protected virtual void RegisterSkill()
    {

    }
    void SetSkillData()
    {
        for (int i = 0; i < blackBoardData.AttackDataList.Count; i++)
        {
            attackDataMap[blackBoardData.AttackDataList[i].AttackName] = blackBoardData.AttackDataList[i];
        }
    }
    public void ResetRespawnTime()
    {
        currentRespawnTime = blackBoardData.respawnTime;
    }
    void ChangeIdleTime()
    {
        currentIdleTime = UnityEngine.Random.Range(blackBoardData.minIdleTime, blackBoardData.maxIdleTime);
    }
    protected void ExecuteAttack(AttackDataSO _attackData)
    {
        if (!HitUtility.CheckPlayerHit(transform, player.centerPos.position, _attackData)) return;

        int damage = HitUtility.CalculateMonsterDamage(_attackData.Damage, player.finalStats[StatusType.PhysicalDefensePower]);

        player.Hit(damage);
        DamageManager.Instance.PopupPlayerDamage(damage, player.centerPos.position);
    }
    public virtual void NormalAttack()
    {
    }
    public bool CheckDistance(float _sqrDistance)
    {
        return CustomUtility.CheckSqrDistance(transform.position, player.centerPos.position, _sqrDistance);
    }
    public bool CheckTrackingLimit()
    {
        return !(CustomUtility.CheckSqrDistance(transform.position, spawnPointCenter, blackBoardData.limitTrackingRange) && CheckLimitTrackingHeight());
    }
    public bool CheckAngle(Vector3 _pos, float _angle)
    {
        return CustomUtility.CheckNormalAngle(_angle, transform.forward, _pos, player.centerPos.position);
    }
    public bool CheckHeightDifference(float _yDifference)
    {
        return CustomUtility.CheckHeightDifference(transform.position.y, player.centerPos.position.y, _yDifference);
    }
    public bool CheckLimitTrackingHeight()
    {
        return CustomUtility.CheckHeightDifference(transform.position.y, spawnPointCenter.y, blackBoardData.limitTrackingHeight);
    }
    public bool CheckHeightInRange(float _min, float _max)
    {
        return CustomUtility.CheckHeightInRange(transform.position.y, player.centerPos.position.y, _min, _max);
    }
    public bool IsStand()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    public void Respawn()
    {
        Init();
    }
    public void StartDash()
    {
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * dashData.distance;

        if(NavMesh.Raycast(start, end, out NavMeshHit hit, NavMesh.AllAreas))
        {
            end = hit.position;
        }

        agent.speed = dashData.speed;
        agent.SetDestination(end);
    }
    public void SetDashData(float _speed, float _distance)
    {
        dashData.speed = _speed;
        dashData.distance = _distance;
    }
    public void SetGlobalCooltime(float _cooltime)
    {
        globalCooltime = _cooltime;
    }
    public void DropItem()
    {
        player.Exp += dropTable.exp;
        DropItem temp;

        temp = PoolManager.Instance.dropItemPool.Get();
        temp.transform.position = transform.position;
        temp.Init(0, UnityEngine.Random.Range(dropTable.minGold, dropTable.maxGold + 1));

        for (int i = 0; i < dropTable.dropTables.Count; i++)
        {
            if (UnityEngine.Random.Range(0f, 100f) <= dropTable.dropTables[i].probability)
            {
                temp = PoolManager.Instance.dropItemPool.Get();
                temp.transform.position = transform.position;
                temp.Init(dropTable.dropTables[i].itemNumber, UnityEngine.Random.Range(dropTable.dropTables[i].minAmount, dropTable.dropTables[i].maxAmount + 1));
            }
        }
        DataManager.Instance.SavePlayer();
    }
    public virtual void GetHUD()
    {
        // 일반 몬스터는 그대로 사용
        // 보스 몬스터는 override 하여 보스HUD로 수정 후
        // boss hud collider 에서 세팅할 것
        if (monsterHUD == null && currentRespawnTime <= 0f)
        {
            monsterHUD = PoolManager.Instance.monsterHUDPool.Get();
            monsterHUD.SetTransform(HUDTransform);
            monsterHUD.SetSlider(hp, blackBoardData.maxHp);
            OnHpChanged = monsterHUD.ChangeHp;
        }
    }
    public virtual void ReleaseHUD()
    {
        //보스몬스터는 override 할것
        if (monsterHUD == null) return;

        OnHpChanged = null;
        PoolManager.Instance.monsterHUDPool.Release(monsterHUD);
        monsterHUD = null;
    }
#if UNITY_EDITOR
    public void SetDropTableSO(DropTableSO _data)
    {
        dropTable = _data;
    }
    public void SetBlackBoardDataSO(MonsterBlackBoardSO _data)
    {
        blackBoardData = _data;
    }
    public void SetAgent(NavMeshAgent _agent)
    {
        agent = _agent;
    }
    public void SetSubGameObject()
    {
        minimapMark = transform.Find("MonsterMark").gameObject;
        HUDTransform = transform.Find("HUDPosition");
    }
#endif
}
