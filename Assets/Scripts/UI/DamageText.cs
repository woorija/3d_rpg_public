using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : PoolBehaviour<DamageText>
{
    [SerializeField] TMP_Text text;

    private Sequence mainSequence;
    Tweener moveTweener;

    float moveYpos;
    Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
        moveTweener = transform.DOMove(Vector3.zero, 0.3f).SetEase(Ease.OutCubic).SetAutoKill(false).Pause();
        mainSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(transform.DOScale(1f, 0.3f).SetEase(Ease.OutCubic))
            .Join(text.DOFade(1f, 0.3f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(0.2f, 0.6f).SetEase(Ease.InQuad))
            .Join(text.DOFade(0.2f, 0.6f).SetEase(Ease.InQuad))
            .OnComplete(ReturnPool)
            .Pause();
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }
    private void LateUpdate()
    {
        transform.rotation = mainCam.transform.rotation;
    }
    public void SetPos(Vector3 _pos)
    {
        transform.position = _pos + Vector3.up;
        moveYpos = transform.position.y + 0.6f;
        moveTweener.ChangeStartValue(transform.position);
        moveTweener.ChangeEndValue(new Vector3(transform.position.x, moveYpos, transform.position.z));
    }
    public void SetText(int _damage)
    {
        text.text = _damage.ToString();
        SetText();
    }
    public void SetText(int _damage, string _addText)
    {
        text.text = string.Format("{0}{1}", _damage.ToString(), _addText);
        SetText();
    }
    void SetText()
    {
        text.ForceMeshUpdate();
        text.alpha = 0;
        moveTweener.Restart();
        mainSequence.Restart();
    }
    void ReturnPool()
    {
        Release(this);
    }
}
