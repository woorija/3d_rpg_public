using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class KeyRebindUI : MonoBehaviour
{
    [SerializeField] RebindingManager rebindingManager;

    [Header("Action Info")]
    [SerializeField] string actionMapName;
    [SerializeField] string actionName;
    [SerializeField] int bindingIndex;

    [Header("UI")]
    [SerializeField] TMP_Text keyNameText;
    [SerializeField] Button rebindButton;
    [SerializeField] TMP_Text rebindNameText;

    [Header("Data")]
    [SerializeField] string keyName;
    [SerializeField] bool isChangeable;
    [SerializeField] UnityEvent<string> keyRebindEvent;

    InputAction action;
    public void Init()
    {
        keyNameText.text = keyName;
        rebindingManager.bindingLoadEvent += UpdateRebindNameText;

        if (isChangeable)
        {
            rebindButton.onClick.AddListener(StartRebind);
        }
        else
        {
            rebindButton.interactable = false;
            rebindButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }

        action = rebindingManager.inputActions.asset.FindActionMap(actionMapName)?.FindAction(actionName);
    }
    private void OnDestroy()
    {
        rebindButton.onClick.RemoveAllListeners();
        rebindingManager.bindingLoadEvent -= UpdateRebindNameText;
    }
    public void UpdateRebindNameText()
    {
        if(action == null || bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            return;
        }

        var displayString = action.GetBindingDisplayString(bindingIndex);

        rebindNameText.text = displayString;
        keyRebindEvent.Invoke(displayString);
    }

    private void StartRebind()
    {
        rebindingManager.StartRebinding(action, bindingIndex, keyName, UpdateRebindNameText);
    }
}
