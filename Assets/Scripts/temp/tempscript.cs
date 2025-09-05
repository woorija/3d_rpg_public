using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    [SerializeField] PlayerStatus status;
    public void HpChange(int _value)
    {
        status.Hp += _value;
    }
    public void MpChange(int _value)
    {
        status.Mp += _value;
    }
    public void ExpChange(int _value)
    {
        status.Exp += _value;
    }
    public void MaxHpChange(int _value)
    {
        status.bonusStats[StatusType.Hp] += _value;
    }
    public void MaxMpChange(int _value)
    {
        status.bonusStats[StatusType.Mp] += _value;
    }
    public void MaxExpChange(int _value)
    {
        status.MaxExp += _value;
    }
}
