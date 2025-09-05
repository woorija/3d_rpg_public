using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour, ICloseable
{
    [SerializeField] EquipmentItemSlot[] slots;
    private void Start()
    {
        gameObject.SetActive(false);
        SetAllSlot();
    }
    public void SetAllSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlot();
        }
    }
    public void SetSlot(int _index)
    {
        slots[_index].SetSlot();
    }

    public void Close()
    {
        ItemInformationUI.Instance.InformationClose();
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
