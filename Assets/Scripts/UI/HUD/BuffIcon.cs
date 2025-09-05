using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIcon : PoolBehaviour<BuffIcon>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] Slider coolTimeImage;

    int buffId;
    Buff buff;
    public void SetBuff(Buff _buff, int _id)
    {
        buff = _buff;
        buffId = _id;
        SetIconImage(buffId);
        coolTimeImage.maxValue = buff.duration;
    }
    void SetIconImage(int _id)
    {
        switch (CustomUtility.GetDigitCount(_id))
        {
            case 6:
                AddressableManager.Instance.LoadAsset<Sprite>($"SkillIcon/S{_id}.png", SetIconSprite);
                break;
            case 9:
                AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{_id}.png", SetIconSprite);
                break;
        }
    }
    void SetIconSprite(Sprite _sprite)
    {
        icon.sprite = _sprite;
    }
    private void LateUpdate()
    {
        coolTimeImage.value = buff.duration - buff.currentDuration;
    }
    public void ReturnPool()
    {
        buff = null;
        Release(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            switch (CustomUtility.GetDigitCount(buffId))
            {
                case 6:
                    SkillInformationUI.Instance.SetInformation(buffId);
                    break;
                case 9:
                    ItemInformationUI.Instance.SetInformation(ItemDataBase.UseableItemDB[buffId]);
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInformationUI.Instance.InformationClose();
        SkillInformationUI.Instance.InformationClose();
    }
}
