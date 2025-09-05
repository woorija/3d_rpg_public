using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MushroomBlackBoardData", menuName = "ScriptableObjects/MushroomBlackBoardData")]
public class MushroomBlackBoardSO : MonsterBlackBoardSO
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
