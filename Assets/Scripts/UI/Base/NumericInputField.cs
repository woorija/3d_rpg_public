using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class NumericInputField : MonoBehaviour
{
    private TMP_InputField inputField;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(_ => CustomInputManager.Instance.DisablePlayerActionMap());
        inputField.onDeselect.AddListener(_ => CustomInputManager.Instance.EnablePlayerActionMap());
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
        return int.TryParse(inputField.text, out int value) ? value : 0;
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
        if (GameManager.Instance.gameMode == GameMode.GamePlay)
        {
            GameManager.Instance.ChangeUIMode();
        }
    }
}
