using UnityEngine;

public class HitUtility : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerController controller;

    private static Collider[] monsterColliders = new Collider[20];

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
    public static bool CheckPlayerHit(Transform _monsterTransform, Vector3 _playerPos, AttackDataSO _attackData)
    {
        switch (_attackData.AttackType)
        {
            case AttackType.Circle:
                return CheckCircleHit(_monsterTransform, _playerPos, _attackData);
            case AttackType.Sector:
                return CheckSectorHit(_monsterTransform, _playerPos, _attackData);
            case AttackType.Box:
                return CheckBoxHit(_monsterTransform, _playerPos, _attackData);
        }
        return false;
    }
    private static bool CheckCircleHit(Transform _monsterTransform, Vector3 _playerPos, AttackDataSO _attackData)
    {
        Vector3 attackCenter = _monsterTransform.position + _attackData.Pos;
        if (!CustomUtility.CheckHeightInRange(attackCenter.y, _playerPos.y, _attackData.YLower, _attackData.YUpper)) return false;
        if (!CustomUtility.CheckSqrDistance(attackCenter, _playerPos, _attackData.OuterRadius * _attackData.OuterRadius, _attackData.InnerRadius * _attackData.InnerRadius)) return false;
        return true;
    }
    private static bool CheckSectorHit(Transform _monsterTransform, Vector3 _playerPos, AttackDataSO _attackData)
    {
        Vector3 attackCenter = _monsterTransform.position + _attackData.Pos;
        if (!CustomUtility.CheckHeightInRange(attackCenter.y, _playerPos.y, _attackData.YLower, _attackData.YUpper)) return false;
        if (!CustomUtility.CheckSqrDistance(attackCenter, _playerPos, _attackData.OuterRadius * _attackData.OuterRadius, _attackData.InnerRadius * _attackData.InnerRadius)) return false;

        float angle = CustomUtility.GetAngle(_monsterTransform.forward, attackCenter, _playerPos);
        foreach(var range in _attackData.Angles)
        {
            if (CustomUtility.CheckAngle(range.minAngle, range.maxAngle, angle)) return true;
        }

        return false;
    }

    private static bool CheckBoxHit(Transform _monsterTransform, Vector3 _playerPos, AttackDataSO _attackData)
    {
        Vector3 attackCenter = _monsterTransform.position + _attackData.Pos;
        if (!CustomUtility.CheckHeightInRange(attackCenter.y, _playerPos.y, _attackData.YLower, _attackData.YUpper)) return false;

        Vector3 localPos = _monsterTransform.InverseTransformPoint(_playerPos) - _monsterTransform.InverseTransformPoint(attackCenter);
        bool isInBox = localPos.x >= -_attackData.Left && localPos.x <= _attackData.Right && localPos.z >= -_attackData.Back && localPos.z <= _attackData.Front;
        return isInBox;
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
    public void CircularSectorHit(Vector3 _centerPos, Vector3 _forward, float _radius, float _yLower, float _yUpper, float _minAngle, float _maxAngle,int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InCircularSectorRangeToMonsterCount(_centerPos, _forward, _radius, _minAngle, _maxAngle, _yLower, _yUpper, ref monsterColliders);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public void CircularHit(Vector3 _centerPos, float _radius, float _yLower, float _yUpper, int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InCircleRangeToMonsterCount(_centerPos, _radius, _yLower, _yUpper, ref monsterColliders);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public void BoxHit(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, int _maxCount, int _hitCount, float _staggerTime)
    {
        int monsterCount = InBoxRangeToMonsterCount(_centerPos, _half, _rotate, ref monsterColliders);
        MonsterHit(monsterCount, _maxCount, _hitCount, _staggerTime);
    }
    public static int InBoxRangeToMonsterCount(Vector3 _centerPos, Vector3 _half, Quaternion _rotate, ref Collider[] colliders)
    {
        int isInMonster = Physics.OverlapBoxNonAlloc(_centerPos, _half, colliders, _rotate, LayerMasks.Monster);
        return isInMonster;
    }
    public static int InCircleRangeToMonsterCount(Vector3 _centerPos, float _radius, float _yLower, float _yUpper, ref Collider[] colliders)
    {
        Vector3 p0 = _centerPos + new Vector3(0, _yLower, 0);
        Vector3 p1 = _centerPos + new Vector3(0, _yUpper, 0);
        int totalColliders = Physics.OverlapCapsuleNonAlloc(p0, p1, _radius, colliders, LayerMasks.Monster);
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (CustomUtility.CheckHeightInRange(_centerPos.y, otherPos.y, _yLower, _yUpper))
            {
                colliders[count++] = colliders[i];
            }
        }
        return count;
    }
    public static int InCircularSectorRangeToMonsterCount(Vector3 _centerPos, Vector3 _forward, float _radius, float _minAngle, float _maxAngle, float _yLower, float _yUpper, ref Collider[] colliders)
    {
        Vector3 p0 = _centerPos + new Vector3(0, _yLower, 0);
        Vector3 p1 = _centerPos + new Vector3(0, _yUpper, 0);
        int totalColliders = Physics.OverlapCapsuleNonAlloc(p0, p1, _radius, colliders, LayerMasks.Monster);
        int count = 0;

        for (int i = 0; i < totalColliders; i++)
        {
            Vector3 otherPos = colliders[i].transform.position;
            if (CustomUtility.IsInCircularSectorAngle(_forward, _centerPos, otherPos, _minAngle, _maxAngle, _yLower, _yUpper))
            {
                colliders[count++] = colliders[i];
            }
        }

        return count;
    }
}
