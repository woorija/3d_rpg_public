using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] Slider HpSlider, MpSlider, ExpSlider;
    [SerializeField] ExpHUD expHUD;
    public void ChangeStatusMaxHp()
    {
        HpSlider.maxValue = playerStatus.finalStats[StatusType.Hp];
    }
    public void ChangeStatusHp()
    {
        HpSlider.value = playerStatus.Hp;
    }
    public void ChangeStatusMaxMp()
    {
        MpSlider.maxValue = playerStatus.finalStats[StatusType.Mp];
    }
    public void ChangeStatusMp()
    {
        MpSlider.value = playerStatus.Mp;
    }
    public void ChangeStatusMaxExp()
    {
        ExpSlider.maxValue = playerStatus.MaxExp;
        expHUD.SetExpText(playerStatus.Exp, playerStatus.MaxExp);
    }
    public void ChangeStatusExp()
    {
        ExpSlider.value = playerStatus.Exp;
        expHUD.SetExpText(playerStatus.Exp, playerStatus.MaxExp);
    }
}
