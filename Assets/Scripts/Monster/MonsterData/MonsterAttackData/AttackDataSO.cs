using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Circle,
    Sector,
    Box
}
[CreateAssetMenu(fileName = "AttackDataSO", menuName = "ScriptableObjects/AttackDataSO")]
public class AttackDataSO : ScriptableObject
{
    // 식별자
    [MonsterActionKeyDropdown]
    [SerializeField] private string attackName;
    [SerializeField] private AttackType attackType;
    
    // 몬스터 중심과의 거리차 (Vector3)
    [SerializeField] private Vector3 pos;
    
    // 공통 범위
    [SerializeField] private float yLower;
    [SerializeField] private float yUpper;

    // Circle + Sector 범위
    [SerializeField] private float outerRadius;
    [SerializeField] private float innerRadius;

    // Sector 범위
    [SerializeField] private List<AngleRange> angles;

    // Box 범위
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float front;
    [SerializeField] private float back;

    // 공격 관련 정보
    [SerializeField] private float cooltime;
    [SerializeField] private int damage;
    [SerializeField] private float percentDamage;

    public string AttackName => attackName;
    public AttackType AttackType => attackType;
    public Vector3 Pos => pos;
    public float YLower => yLower;
    public float YUpper => yUpper;

    public float OuterRadius => outerRadius;
    public float InnerRadius => innerRadius;

    public List<AngleRange> Angles => angles;

    public float Left => left;
    public float Right => right;
    public float Front => front;
    public float Back => back;

    public float Cooltime => cooltime;
    public int Damage => damage;
    public float PercentDamage => percentDamage;
}