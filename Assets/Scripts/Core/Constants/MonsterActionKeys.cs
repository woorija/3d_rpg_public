public static class MonsterActionKey
{
    //모든 몬스터가 사용하는 일반적인 기본공격
    public const string NormalAttack = "NormalAttack";

    //몬스터 별 하나씩은 가질 수 있는 큰 카테고리의 공격
    public const string DashSkill = "DashSkill";
    public const string JumpSkill = "JumpSkill";
    public const string GroundImpact = "GroundImpact";
    public const string RageAttack = "RageAttack";

    //몇몇 몬스터만 가지고 있을 수도 있는 세부적인 공격
    public const string LeftSmash = "LeftSmash";
    public const string RightSmash = "RightSmash";

}
