using UnityEngine;

public static class AnimationKey
{
    //플레이어
    public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    public static readonly int ActionSpeed = Animator.StringToHash("ActionSpeed");
    public static readonly int Move = Animator.StringToHash("Move");
    public static readonly int IsRun = Animator.StringToHash("IsRun");
    public static readonly int DirectionX = Animator.StringToHash("Dir_X");
    public static readonly int DirectionY = Animator.StringToHash("Dir_Y");
    public static readonly int IsPlayingSkill = Animator.StringToHash("IsPlayingSkill");
    public static readonly int SkillId = Animator.StringToHash("SkillId");
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int Buff = Animator.StringToHash("Buff");
    public static readonly int Fall = Animator.StringToHash("Fall");
    public static readonly int Land = Animator.StringToHash("Land");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Roll = Animator.StringToHash("Roll");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsHit = Animator.StringToHash("IsHit");
    public static readonly int IsDie = Animator.StringToHash("IsDie");

    //몬스터

    public static readonly int Die = Animator.StringToHash("Die");
    public static readonly int Stagger = Animator.StringToHash("Stagger");
    public static readonly int NormalAttack = Animator.StringToHash("NormalAttack");
    public static readonly int JumpSkill = Animator.StringToHash("JumpSkill");
    public static readonly int DashSkill = Animator.StringToHash("DashSkill");

    //공용
    public static readonly int AnimationEnd = Animator.StringToHash("AnimationEnd");
}
