using UnityEngine;

public class UIOpenButton : MonoBehaviour
{
    [SerializeField]
    [Interfaces(typeof(ICloseable))] MonoBehaviour uiObject;
    public ICloseable ui => uiObject as ICloseable;

    public void UIToggle()
    {
        if (ui.IsActive())
        {
            UIManager.Instance.CloseUI(ui);
        }
        else
        {
            uiObject.gameObject.SetActive(true);
            UIManager.Instance.OpenUI(ui);
            SkillInformationUI.Instance.InformationClose();
            ItemInformationUI.Instance.InformationClose();
            GameManager.Instance.GameModeChange(GameMode.UIMode);
        }
    }
}
