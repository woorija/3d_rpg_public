public class BatBlackBoard : BaseBlackBoard
{
    public override void NormalAttack()
    {
        if (CustomUtility.IsInCircularSectorRange(transform.forward, transform.position, player.transform.position, blackBoardData.attackRange * blackBoardData.attackRange, 22.5f, 1.2f))
        {
            int damage = HitUtility.CalculateMonsterDamage(blackBoardData.attackDamage, player.finalStats[StatusType.PhysicalDefensePower]);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
}
