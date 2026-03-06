public class MushroomBlackBoard : BaseBlackBoard
{
    AttackDataSO normalAttackData;
    AttackDataSO jumpSkillData;
    AttackDataSO jumpSkillGroundImpactData;
    AttackDataSO dashSkillData;
    protected override void Init()
    {
        base.Init();

        normalAttackData = attackDataMap[MonsterActionKey.NormalAttack];
        jumpSkillData = attackDataMap[MonsterActionKey.JumpSkill];
        jumpSkillGroundImpactData = attackDataMap[MonsterActionKey.GroundImpact];
        dashSkillData = attackDataMap[MonsterActionKey.DashSkill];

        RegisterSkill(MonsterActionKey.NormalAttack, normalAttackData.Cooltime);
        RegisterSkill(MonsterActionKey.JumpSkill, jumpSkillData.Cooltime);
        RegisterSkill(MonsterActionKey.DashSkill, dashSkillData.Cooltime);

    }
    public override void NormalAttack()
    {
        ExecuteAttack(normalAttackData);
    }
    public void DashSkillAttack()
    {
        ExecuteAttack(dashSkillData);
    }
    public void JumpSkillAttack()
    {
        ExecuteAttack(jumpSkillData);
    }
    public void JumpSkillGroundImpact()
    {
        ExecuteAttack(jumpSkillGroundImpactData);
    }
}
