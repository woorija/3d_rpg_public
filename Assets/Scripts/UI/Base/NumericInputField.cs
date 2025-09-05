using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class NumericInputField : MonoBehaviour
{
    private TMP_InputField inputField;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(CustomInputManager.Instance.DisablePlayerActionMap);
        inputField.onDeselect.AddListener(CustomInputManager.Instance.EnablePlayerActionMap);
    }
    private void OnEnable()
    {
        ResetValue();
        SetFocusInputFieldAsync().Forget();
    }
    private void OnDisable()
    {
        CustomInputManager.Instance.EnablePlayerActionMap();
    }
    public int GetValue()
    {
        return string.IsNullOrEmpty(inputField.text) ? 0 : int.Parse(inputField.text);
    }
    public void ResetValue()
    {
        inputField.text = string.Empty;
    }
    async UniTaskVoid SetFocusInputFieldAsync()
    {
        await UniTask.NextFrame();
        inputField.Select();
        inputField.ActivateInputField();
        CustomInputManager.Instance.DisablePlayerActionMap();
        GameManager.Instance.ChangeUIMode();
    }
}
