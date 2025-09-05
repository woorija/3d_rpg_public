using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikeBlackBoardData", menuName = "ScriptableObjects/SpikeBlackBoardData")]
public class SpikeBlackBoardSO : MonsterBlackBoardSO
{
    public float dashskillCooltime;
    public Vector3 dashskillRange;
    public int dashskillDamage;
    public float jumpSkillCooltime;
    public float jumpSkillInnerRange;
    public float jumpSkillOuterRange;
    public int jumpSkillInnerDamage;
    public int jumpSkillOuterDamage;
}
