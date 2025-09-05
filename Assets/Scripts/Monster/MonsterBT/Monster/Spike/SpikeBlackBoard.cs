using UnityEngine;

public class SpikeBlackBoard : BaseBlackBoard
{
    public float currentDashSkillCooltime { get; protected set; }
    public float currentJumpSkillCooltime { get; protected set; }

    protected override void Init()
    {
        base.Init();
        currentDashSkillCooltime = 0;
        currentJumpSkillCooltime = 0;
    }
    protected override void Update()
    {
        base.Update();
        currentDashSkillCooltime -= Time.deltaTime;
        currentJumpSkillCooltime -= Time.deltaTime;
    }
    public override void GetHUD()
    {
        if (BossHUD.Instance.IsChangeHUD(blackBoardData.id))
        {
            RemoveAllListener();
            BossHUD.Instance.SetHUD(blackBoardData.id, hp, blackBoardData.maxHp / 5);
            AddHUDListener(BossHUD.Instance.ChangeHp);
        }
    }
    public override void ReleaseHUD()
    {
        BossHUD.Instance.ReleaseHUD();
        RemoveAllListener();
    }
    public override void NormalAttack()
    {
        if (CustomUtility.IsInCircularSectorRange(transform.forward, transform.position, player.transform.position, blackBoardData.attackRange * blackBoardData.attackRange, 22.5f, 2f))
        {
            int damage = HitUtility.CalculateMonsterDamage(blackBoardData.attackDamage, player.finalStats[StatusType.PhysicalDefensePower]);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
    public void ResetDashSkillCooltime()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        currentDashSkillCooltime = MushroomBlackBoardData.dashskillCooltime;
    }
    public void ResetJumpSkillCooltime()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        currentJumpSkillCooltime = MushroomBlackBoardData.jumpSkillCooltime;
    }
    public void DashSkillAttack()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        if (HitUtility.IsInBoxRangeToPlayer(transform.position + (transform.forward * MushroomBlackBoardData.dashskillRange.z), MushroomBlackBoardData.dashskillRange, transform.localRotation))
        {
            int damage = HitUtility.CalculateMonsterDamage(MushroomBlackBoardData.dashskillDamage, player.finalStats[StatusType.PhysicalDefensePower]);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }

    }
    public void JumpSkillAttack()
    {
        MushroomBlackBoardSO MushroomBlackBoardData = (MushroomBlackBoardSO)blackBoardData;
        if (CheckDistance(MushroomBlackBoardData.jumpSkillInnerRange))
        {
            int damage = HitUtility.CalculateMonsterDamage(MushroomBlackBoardData.jumpSkillInnerDamage, player.finalStats[StatusType.PhysicalDefensePower]);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
        else if (CheckDistance(MushroomBlackBoardData.jumpSkillOuterRange))
        {
            int damage = HitUtility.CalculateMonsterDamage(MushroomBlackBoardData.jumpSkillOuterDamage, player.finalStats[StatusType.PhysicalDefensePower]);
            player.Hit(damage);
            DamageManager.Instance.PopupPlayerDamage(damage, player.transform.position);
        }
    }
}
