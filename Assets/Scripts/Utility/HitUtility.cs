using UnityEngine;

public class HitUtility : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerController controller;

    private static Collider[] monsterColliders = new Collider[10];
    private static Collider[] playerCollider = new Collider[1];

    private static int playerLayerMask;
    private static int monsterLayerMask;
    private void Awake()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        monsterLayerMask = LayerMask.GetMask("Monster");
    }

    /// <summary>
    /// 레벨차에 따른 대미지 계수를 계산하는 유틸리티 함수
    /// </summary>
    /// <param name="_monsterlvl">몬스터 레벨</param>
    /// <param name="_playerlvl">플레이어 레벨</param>
    /// <returns>대미지 계수</returns>
    public static float CalculateLevelBonusDamage(int _monsterLevel, int _playerLevel)
    {
        int levelDifference = _monsterLevel - _playerLevel;

        if (levelDifference >= 0)
        {
            return Mathf.Max(0, 1f - (levelDifference * 0.05f));
        }
        if (levelDifference >= -4)
        {
            return 1f - levelDifference * 0.04f;
        }
        return 1.2f;
    }

    /// <summary>
    /// 몬스터가 플레이어에게 주는 대미지를 계산하는 유틸리티 함수
    /// </summary>
    /// <param name="_damage">몬스터가 주는 대미지</param>
    /// <param name="_playerDef">플레이어 방어력</param>
    /// <returns></returns>
    public static int CalculateMonsterDamage(int _damage, int _playerDef)
    {
        float temp = _damage * (1f - (_playerDef / (_playerDef + 300f)));
        return (int)temp;
    }

    /// <summary>
    /// 플레이어가 몬스터에게 주는 대미지를 계산하는 유틸리티 함수
    /// </summary>
    /// <param name="_damage">대미지</param>
    /// <param name="_monsterLevel">몬스터 레벨</param>
    /// <param name="_playerLevel">플레이어 레벨</param>
    /// <returns>최종 대미지</returns>
    public static int CalculatePlayerDamage(int _damage, int _monsterLevel, int _playerLevel)
    {
        float levelBonus = CalculateLevelBonusDamage(_monsterLevel, _playerLevel);
        int temp = (int)(_damage * levelBonus);
        return temp;
    }
    public void SetSkillPhysicalMultiplier(int _attackIndex)
    {
        int multiplier = SkillData.Instance.GetSkillMultiplier(controller.currentPlaySkillId, _attackIndex);
        status.damageCoefficient.SetPhysicalMultiplier(multiplier);
    }
    public void SetSkillMagicalMultiplier(int _attackIndex)
    {
        int multiplier = SkillData.Instance.GetSkillMultiplier(controller.currentPlaySkillId, _attackIndex);
        status.damageCoefficient.SetMagicalMultiplier(multiplier);
    }
    public void SetSkillMixMultiplier(int _physicalIndex, int _magicalIndex)
    {
        int physicalMultiplier = SkillData.Instance.GetSkillMultiplier(controller.currentPlaySkillId, _physicalIndex);
        int magicalMultiplier = SkillData.Instance.GetSkillMultiplier(controller.currentPlaySkillId, _magicalIndex);
        status.damageCoefficient.SetMixMultiplier(physicalMultiplier, magicalMultiplier);
    }
    public static bool IsInBoxRangeToPlayer(Vector3 _centerPos, Vector3 _half, Quaternion _rotate)
    {
        int isInPlayer = Physics.OverlapBoxNonAlloc(_centerPos, _half, playerCollider, _rotate, playerLayerMask);
        if (isInPlayer == 0) return false;
        return true;
    }
    public void MonsterHit(int _monsterCount, int _maxCount, int _hitCount, float _staggerTime)
    {
        int maxCount = _monsterCount > _maxCount ? _maxCount : _monsterCount;
        for (int i = 0; i < maxCount; i++)
        {
            BaseBlackBoard blackBoard = monsterColliders[i].GetComponent<BaseBlackBoard>();
            blackBoard.Hit(status.damageCoefficient, _hitCount, status.Level, status.ArmorBreakLevel, _staggerTime);
            
            blackBoard.GetHUD();
        }
    }
    public void CircularSectorHit(Vector3 _centerPos, Vector3 _forward, float _radius, float _halfSectorAngle, float _yposLimit,int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InCircularSectorRangeToMonsterCount(_centerPos, _forward, _radius, _halfSectorAngle, ref monsterColliders, _yposLimit);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public void CircularHit(Vector3 _centerPos, float _radius, float _yposLimit, int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InCircleRangeToMonsterCount(_centerPos, _radius, ref monsterColliders, _yposLimit);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public void BoxHit(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InBoxRangeToMonsterCount(_centerPos, _half, _rotate, ref monsterColliders);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public static int InBoxRangeToMonsterCount(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, ref Collider[] colliders)
    {
        int isInMonster = Physics.OverlapBoxNonAlloc(_centerPos, _half, colliders, _rotate, monsterLayerMask);
        return isInMonster;
    }
    public static int InCircleRangeToMonsterCount(Vector3 _centerPos, float _radius, ref Collider[] colliders, float _yposLimit)
    {
        int totalColliders = Physics.OverlapSphereNonAlloc(_centerPos, _radius, colliders, monsterLayerMask);
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (CustomUtility.CheckHeightDifference(_centerPos.y, otherPos.y, _yposLimit))
            {
                colliders[count++] = colliders[i];
            }
        }

        return count;
    }
    public static int InCircularSectorRangeToMonsterCount(Vector3 _centerPos, Vector3 _forward, float _radius, float _halfSectorAngle, ref Collider[] colliders, float _yposLimit)
    {
        int totalColliders = Physics.OverlapSphereNonAlloc(_centerPos, _radius, colliders, monsterLayerMask);
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (CustomUtility.IsInCircularSectorAngle(_forward, _centerPos, otherPos, _halfSectorAngle, _yposLimit))
            {
                colliders[count++] = colliders[i];
            }
        }

        return count;
    }
}
