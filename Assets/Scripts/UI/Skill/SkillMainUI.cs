using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillMainUI : MonoBehaviour, ICloseable
{
    [SerializeField] GameObject[] taps;
    [SerializeField] SkillSlotUI[] slots;
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] TMP_Text spText;
    private void Awake()
    {
        playerStatus.onClassRankUpEvent += SetTap;
        playerStatus.onLevelUpEvent += SetUI;
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].OnLevelChangeEvent += SetUI;
            slots[i].InitStatus(playerStatus);
        }
    }
    private void Start()
    {
        SetUI(0);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        playerStatus.onClassRankUpEvent?.Invoke();
    }

    public void SetTap()
    {
        int rank = playerStatus.classRank + 1;
        for(int i = 0; i < rank; i++)
        {
            taps[i].SetActive(true);
        }
        for (int i = rank; i < taps.Length; i++)
        {
            taps[i].SetActive(false);
        }
    }
    public void SetUI(int _rank)
    {
        SkillData.Instance.RankFilter(_rank);
        SetUI();
    }
    public void SetUI()
    {
        List<int> ids = SkillData.Instance.currentRankFilterSkills;
        for (int i = 0; i < ids.Count; i++)
        {
            slots[i].SetSlot(ids[i]);
        }
        for (int i = ids.Count; i < slots.Length; i++)
        {
            slots[i].ResetSlot();
        }
    }
    private void OnDestroy()
    {
        playerStatus.onClassRankUpEvent -= SetTap;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].OnLevelChangeEvent -= SetUI;
        }
    }
    public void SetSpText(int _sp)
    {
        spText.text = $"SP {_sp}";
    }

    public void Close()
    {
        SkillInformationUI.Instance.InformationClose();
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
