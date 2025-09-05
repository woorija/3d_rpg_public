using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMonsterBlackBoardData", menuName = "ScriptableObjects/BaseMonsterBlackBoardData")]
public class MonsterBlackBoardSO : ScriptableObject
{
    public int id;

    public int level;
    public int maxHp;

    public float trackingRange;
    public float normalAttackRange;

    public float limitTrackingRange;
    public float limitTrackingHeight;

    public float minIdleTime;
    public float maxIdleTime;

    public float idleMoveSpeed;
    public float trackingMoveSpeed;
    public float returnMoveSpeed;

    public float attackCooltime;
    public float attackRange;
    public int attackDamage;

    public float respawnTime;

}
