public class BatBlackBoard : BaseBlackBoard
{
    AttackDataSO normalAttackData;
    protected override void RegisterSkill()
    {
        normalAttackData = attackDataMap[MonsterActionKey.NormalAttack];

        RegisterSkill(MonsterActionKey.NormalAttack, normalAttackData.Cooltime);
    }
    public override void NormalAttack()
    {
        ExecuteAttack(normalAttackData);
    }
}