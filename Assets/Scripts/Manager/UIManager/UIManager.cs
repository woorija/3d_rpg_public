using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class UIManager : SingletonBehaviour<UIManager>, IInputBindable
{
    [SerializeField] SettingUI settingUI;

    private List<ICloseable> closeList;

    public int nextSortOrder = 5;
    public int maxSortOrder { get; private set; } = 16383;
    public Action SortReset;

    public UnityEvent InventoryToggle;
    public UnityEvent EquipmentToggle;
    public UnityEvent StatusToggle;
    public UnityEvent SkillToggle;
    public UnityEvent QuestToggle;
    protected override void Awake()
    {
        base.Awake();
        closeList = new List<ICloseable>();
        CustomSceneManager.Instance.onUICloseHandler += CloseAllUI;
    }
    private void Start()
    {
        settingUI.StartClose();
        InputInit();
    }
    private void OnDisable()
    {
        UnbindAllInputActions();
        CustomSceneManager.Instance.onUICloseHandler -= CloseAllUI;
    }
    private void InputInit()
    {
        SortReset += SortOrderReset;

        BindAllInputActions();
    }
    public void SettingUIClose()
    {
        settingUI.gameObject.SetActive(false);
    }
    public void OpenUI(ICloseable _ui)
    {
        if (_ui != null)
        {
            closeList.Add(_ui);
        }
    }
    public void CloseUI(ICloseable _ui)
    {
        if(closeList.Contains(_ui))
        {
            closeList.Remove(_ui);
            _ui.Close();
            CheckUIEmptyAndNotify();
        }
    }
    public void CloseLastUI(InputAction.CallbackContext context)
    {
        if(closeList.Count > 0)
        {
            int lastIndex = closeList.Count - 1;
            ICloseable ui = closeList[lastIndex];
            closeList.RemoveAt(lastIndex);
            ui.Close();
            CheckUIEmptyAndNotify();
        }
        else
        {
            if (TalkManager.Instance.isTalking) return;
            settingUI.OpenUI();
        }
    }
    public void CloseAllUI()
    {
        for(int i = 0; i < closeList.Count; i++)
        {
            closeList[i].Close();
        }
        closeList.Clear();
        CheckUIEmptyAndNotify();
    }
    void CheckUIEmptyAndNotify()
    {
        if (closeList.Count > 0) return;
        if (TalkManager.Instance.isTalking) return;
        GameManager.Instance.GameModeChange(GameMode.GamePlay);
    }
    public void AllUISortReset()
    {
        SortReset?.Invoke();
    }
    public void SortOrderReset()
    {
        nextSortOrder = 5;
    }
    public void ChangeSortOrder(Canvas _canvas)
    {
        if (_canvas.sortingOrder != nextSortOrder)
        {
            _canvas.sortingOrder = ++nextSortOrder;

            if (nextSortOrder >= maxSortOrder)
            {
                AllUISortReset();
            }
        }
    }
    private void PerformedInventoryToggle(InputAction.CallbackContext context)
    {
        InventoryToggle?.Invoke();
    }
    private void PerformedEquipmentToggle(InputAction.CallbackContext context)
    {
        EquipmentToggle?.Invoke();
    }
    private void PerformedStatusToggle(InputAction.CallbackContext context)
    {
        StatusToggle?.Invoke();
    }
    private void PerformedSkillToggle(InputAction.CallbackContext context)
    {
        SkillToggle?.Invoke();
    }
    private void PerformedQuestToggle(InputAction.CallbackContext context)
    {
        QuestToggle?.Invoke();
    }

    public void InitInputHandlers()
    {
    }

    public void BindAllInputActions()
    {
        var UIAction = CustomInputManager.Instance.UI;

        UIAction.InventoryUIToggle.performed += PerformedInventoryToggle;
        UIAction.EquipmentUIToggle.performed += PerformedEquipmentToggle;
        UIAction.StatusUIToggle.performed += PerformedStatusToggle;
        UIAction.SkillUIToggle.performed += PerformedSkillToggle;
        UIAction.QuestUIToggle.performed += PerformedQuestToggle;
        UIAction.ExitUI.performed += CloseLastUI;
    }

    public void UnbindAllInputActions()
    {
        var UIAction = CustomInputManager.Instance.UI;

        UIAction.InventoryUIToggle.performed -= PerformedInventoryToggle;
        UIAction.EquipmentUIToggle.performed -= PerformedEquipmentToggle;
        UIAction.StatusUIToggle.performed -= PerformedStatusToggle;
        UIAction.SkillUIToggle.performed -= PerformedSkillToggle;
        UIAction.QuestUIToggle.performed -= PerformedQuestToggle;
        UIAction.ExitUI.performed -= CloseLastUI;
    }
}
