using UnityEngine;

public class DamageCoefficient
{
    public float weaponMastery { get; private set; }

    public float criticalRate { get; private set; }
    public float criticalDamage { get; private set; } = 1.5f;

    public float physicalMultiplier { get; private set; }
    public int maxPhysicalDamage { get; private set; }
    public int minPhysicalDamage { get; private set; }

    public float magicalMultiplier { get; private set; }
    public int maxMagicalDamage { get; private set; }
    public int minMagicalDamage { get; private set; }

    public int accuracy { get; private set; }

    public void SetWeaponMastery(int _value)
    {
        weaponMastery = _value * 0.0001f;
        SetMinPhysicalDamage();
        SetMinMagicalDamage();
    }
    public void SetAccuracy(int _value)
    {
        //세부계산식 필요
        accuracy = _value;
    }
    public void SetCriticalRate(float _value)
    {
        criticalRate = _value * 0.01f;
    }
    public void SetCriticalDamage(float _value) 
    {
        criticalDamage = 1.5f + (_value * 0.01f);
    }
    public void SetMaxPhysicalDamage(int _value)
    {
        maxPhysicalDamage = _value;
        SetMinPhysicalDamage();
    }
    public void SetMinPhysicalDamage()
    {
        minPhysicalDamage = (int)(maxPhysicalDamage * weaponMastery);
    }
    public void SetMaxMagicalDamage(int _value)
    {
        maxMagicalDamage = _value;
        SetMinMagicalDamage();
    }
    public void SetMinMagicalDamage()
    {
        minMagicalDamage = (int)(maxMagicalDamage * weaponMastery);
    }
    public void SetPhysicalMultiplier(int _value)
    {
        physicalMultiplier = _value * 0.01f;
        magicalMultiplier = 0f;
    }
    public void SetMagicalMultiplier(int _value)
    {
        magicalMultiplier = _value * 0.01f;
        physicalMultiplier = 0f;
    }
    public void SetMixMultiplier(int _physicalValue, int _magicalValue)
    {
        physicalMultiplier = _physicalValue * 0.01f;
        magicalMultiplier = _magicalValue * 0.01f;
    }
    public int GetDamage()
    {
        int damage = Random.Range(minPhysicalDamage, maxPhysicalDamage + 1) + Random.Range(minMagicalDamage, maxMagicalDamage + 1);
        return damage;
    }
}
