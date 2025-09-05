using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour, ICloseable
{
    [SerializeField] StatusInformation[] InformationDatas;
    [SerializeField] PlayerStatus status;
    [SerializeField] GameObject detailStatusUI;
    [SerializeField] TMP_Text remainingApText;
    [SerializeField] GameObject[] StatAllocateButtons;

    Dictionary<StatusType, StatusInformation> InfoDict;
    bool isChanged;
    private void Awake()
    {
        isChanged = false;
        InfoDict = new Dictionary<StatusType, StatusInformation>();
        for(int i = 0; i < InformationDatas.Length; i++)
        {
            InfoDict[InformationDatas[i].statusType] = InformationDatas[i];
        }
    }
    private void OnEnable()
    {
        if(isChanged)
        {
            SetAllInformation();
            isChanged = false;
        }
    }
    private void Start()
    {
        SetAllInformation();
        status.AddStatusEvent(SetInformation);
        status.onRemainingAPChangeEvent += SetRemainingAPText;
        status.onRemainingAPChangeEvent += SetStatAllocateButtons;
        status.onClassRankUpEvent += SetAllInformation;
        gameObject.SetActive(false);
        detailStatusUI.SetActive(false);
    }
    public void SetInformation(StatusType _type)
    {
        isChanged = true;
        
        if (!gameObject.activeSelf) return;
        
        StatusInformation info;
        InfoDict.TryGetValue(_type, out info);
        switch (_type)
        {
            case StatusType.Level:
                info.SetText(status.Level);
                break;
            case StatusType.Hp:
                info.SetText(status.Hp, status.finalStats[_type]);
                break;
            case StatusType.Mp:
                info.SetText(status.Mp, status.finalStats[_type]);
                break;
            case StatusType.Strength:
            case StatusType.Dexterity:
            case StatusType.Vitality:
            case StatusType.Intelligence:
            case StatusType.Wisdom:
            case StatusType.Agility:
                info.SetText(status.baseStats[_type], status.bonusStats[_type]);
                break;
            case StatusType.HpRegen:
            case StatusType.MpRegen:
            case StatusType.Stamina:
                break;
            case StatusType.MoveSpeed:
                info.SetText((int)(status.MoveSpeedMultiplier * 10000));
                break;
            case StatusType.AttackSpeed:
                info.SetText((int)(status.ActionSpeedMultiplier * 10000));
                break;
            case StatusType.CooltimeReduce:
                info.SetText((int)(status.ActionSpeedMultiplier * 10000));
                break;
            default:
                info.SetText(status.finalStats[_type]);
                break;
        }
    }
    void SetAllInformation()
    {
        foreach(StatusType _type in InfoDict.Keys)
        {
            SetInformation(_type);
        }
    }
    public void SetRemainingAPText(int _value)
    {
        remainingApText.SetText(_value.ToString());
    }
    public void SetStatAllocateButtons(int _value)
    {
        if (_value == 0)
        {
            for(int i = 0; i < StatAllocateButtons.Length; i++)
            {
                StatAllocateButtons[i].SetActive(false);
            }
        }
        else
        {
            if (!StatAllocateButtons[0].activeSelf)
            {
                for (int i = 0; i < StatAllocateButtons.Length; i++)
                {
                    StatAllocateButtons[i].SetActive(true);
                }
            }
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
