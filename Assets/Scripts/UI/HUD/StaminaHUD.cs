using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour
{
    [SerializeField] Slider staminaSlider;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] PlayerStatus playerStatus;
    float fadeDuration = 0.2f;

    public void FadeIn()
    {
        canvasGroup.DOKill();
        float currentAlpha = canvasGroup.alpha;
        float duration = (1 - currentAlpha) * fadeDuration;
        canvasGroup.DOFade(1, duration);
    }
    public void FadeOut()
    {
        canvasGroup.DOKill();
        float currentAlpha = canvasGroup.alpha;
        float duration = currentAlpha * fadeDuration;
        canvasGroup.DOFade(0, duration);
    }
    public void ChangeStatusStamina()
    {
        staminaSlider.value = playerStatus.Stamina;
    }
    public void ChangeStatusMaxStamina()
    {
        staminaSlider.maxValue = playerStatus.finalStats[StatusType.Stamina];
    }
}
