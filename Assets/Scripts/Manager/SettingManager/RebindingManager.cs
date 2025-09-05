using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingManager : MonoBehaviour
{
    private static readonly HashSet<string> excludedInputs = new HashSet<string>
    {
        // 키보드 제외 키
        "<Keyboard>/f1",
        "<Keyboard>/f2",
        "<Keyboard>/f3",
        "<Keyboard>/f4",
        "<Keyboard>/f5",
        "<Keyboard>/f6",
        "<Keyboard>/f7",
        "<Keyboard>/f8",
        "<Keyboard>/f9",
        "<Keyboard>/f10",
        "<Keyboard>/f11",
        "<Keyboard>/f12",
        "<Keyboard>/printScreen",
        "<Keyboard>/scrollLock",
        "<Keyboard>/pauseBreak",
        
        // 마우스 제외 입력
        "<Mouse>/position",
        "<Mouse>/delta",
        "<Mouse>/scroll",
        "<Mouse>/scroll/up",
        "<Mouse>/scroll/down",
        "<Mouse>/scroll/left",
        "<Mouse>/scroll/right"
    };
    private HashSet<string> usedInputs = new HashSet<string>();

    public Controls inputActions {  get; private set; }
    [SerializeField] string[] actionMapNames;
    [SerializeField] KeyRebindUI[] rebindUIs;
    [SerializeField] PopupUI popupUI;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    

    string prevPath;
    public Action bindingLoadEvent;

    private void OnDestroy()
    {
        bindingLoadEvent = null;
    }
    public void Init()
    {
        InitUI();
        SetUsedInputs();
    }
    void InitUI()
    {
        inputActions = CustomInputManager.Instance.customInputActionAsset;
        for (int i = 0; i < rebindUIs.Length; i++)
        {
            rebindUIs[i].Init();
        }
    }
    public void StartRebinding(InputAction _action,int _bindingIndex, string _keyName, Action _successAction)
    {
        if(rebindingOperation != null)
        {
            rebindingOperation.Cancel();
        }
        prevPath = _action.bindings[_bindingIndex].effectivePath;
        popupUI.PopupMessage($"{_keyName}에 대한\n새로운 키를 입력하세요\n(ESC: 취소)");
        _action.Disable();
        rebindingOperation = _action.PerformInteractiveRebinding(_bindingIndex)
            .OnPotentialMatch(operation => OnPotentialKeyMatch())
            .OnComplete(operation => CompleteRebinding(_action, _successAction))
            .OnCancel(operation => CancelRebinding(_action));

        rebindingOperation.Start();
    }
    void OnPotentialKeyMatch()
    {
        var currentKey = GetControlPath(rebindingOperation.selectedControl);;
        
        if (excludedInputs.Contains(currentKey))
        {
            rebindingOperation.Cancel();
            popupUI.PopupMessage("사용할 수 없는 키입니다.", true);
        }
        else if (usedInputs.Contains(currentKey))
        {
            rebindingOperation.Cancel();
            popupUI.PopupMessage("이미 사용중인 키입니다.", true);
        }
    }
    void CompleteRebinding(InputAction _action, Action _succeseAction)
    {
        usedInputs.Remove(prevPath);
        usedInputs.Add(GetControlPath(rebindingOperation.selectedControl));
        rebindingOperation?.Dispose();
        rebindingOperation = null;
        DataManager.Instance.SaveKey();
        popupUI.Close();
        _action.Enable();
        _succeseAction?.Invoke();
    }
    void CancelRebinding(InputAction _action)
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;
        popupUI.Close();
        _action.Enable();
    }
    private void SetUsedInputs()
    {
        var asset = inputActions.asset;
        foreach (string actionMapName in actionMapNames)
        {
            var actionMap = asset.FindActionMap(actionMapName);

            foreach (var action in actionMap)
            {
                foreach (var binding in action.bindings)
                {
                    usedInputs.Add(binding.effectivePath);
                }
            }
        }
        bindingLoadEvent?.Invoke();
    }

    public void ResetAllBinding()
    {
        var asset = inputActions.asset;

        asset.Disable();

        foreach (string actionMapName in actionMapNames)
        {
            var actionMap = asset.FindActionMap(actionMapName);
            actionMap.RemoveAllBindingOverrides();
        }

        usedInputs.Clear();
        SetUsedInputs();

        asset.Enable();
        DataManager.Instance.SaveKey();
    }

    private string GetControlPath(InputControl _inputControl)
    {
        return $"<{_inputControl.device.layout}>/{_inputControl.name}";
    }

    public string AssetToJson()
    {
        return inputActions.asset.SaveBindingOverridesAsJson();
    }
    public void AssetLoadFromJson(string _json)
    {
        InitUI();
        var asset = inputActions.asset;
        asset.Disable();
        asset.LoadBindingOverridesFromJson(_json);
        
        SetUsedInputs();
        asset.Enable();
    }
}
