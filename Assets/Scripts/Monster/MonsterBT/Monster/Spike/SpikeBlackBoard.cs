public class SpikeBlackBoard : BaseBlackBoard
{
    AttackDataSO normalAttackData;
    AttackDataSO dashSkillData;
    AttackDataSO leftSmashData;
    AttackDataSO rightSmashData;
    AttackDataSO rageAttackData;

    protected override void Init()
    {
        base.Init();

        normalAttackData = attackDataMap[MonsterActionKey.NormalAttack];
        dashSkillData = attackDataMap[MonsterActionKey.DashSkill];
        leftSmashData = attackDataMap[MonsterActionKey.LeftSmash];
        rightSmashData = attackDataMap[MonsterActionKey.RightSmash];
        rageAttackData = attackDataMap[MonsterActionKey.RageAttack];

        RegisterSkill(MonsterActionKey.NormalAttack, normalAttackData.Cooltime);
        RegisterSkill(MonsterActionKey.DashSkill, dashSkillData.Cooltime);
        RegisterSkill(MonsterActionKey.LeftSmash, leftSmashData.Cooltime);
        RegisterSkill(MonsterActionKey.RightSmash, rightSmashData.Cooltime);
        RegisterSkill(MonsterActionKey.RageAttack, rageAttackData.Cooltime);
    }
    public override void GetHUD()
    {
        if (BossHUD.Instance.IsChangeHUD(blackBoardData.id))
        {
            BossHUD.Instance.SetHUD(blackBoardData.id, hp, blackBoardData.maxHp / 5);
            OnHpChanged = BossHUD.Instance.ChangeHp;
        }
    }
    public override void ReleaseHUD()
    {
        BossHUD.Instance.ReleaseHUD();
        OnHpChanged = null;
    }
    public override void NormalAttack()
    {
        ExecuteAttack(normalAttackData);
    }
    public void DashSkillAttack()
    {
        ExecuteAttack(dashSkillData);
    }
    public void LeftSmashAttack()
    {
        ExecuteAttack(leftSmashData);
    }
    public void RightSmashAttack()
    {
        ExecuteAttack(rightSmashData);
    }
    public void RageSkillAttack()
    {
        ExecuteAttack(rageAttackData);
    }
}
