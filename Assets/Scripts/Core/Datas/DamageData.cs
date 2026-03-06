public struct DamageData
{
    public int damage;
    public bool isCritical;
    public DamageData(int _damage, bool _isCritical)
    {
        damage = _damage;
        isCritical = _isCritical;
    }
}