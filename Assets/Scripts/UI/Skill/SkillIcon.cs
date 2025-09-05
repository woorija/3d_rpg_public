using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] Image iconImage;
    int skillId;
    public void SetIcon(int _id)
    {
        AddressableManager.Instance.LoadAsset<Sprite>($"SkillIcon/S{_id}.png", SetIconSprite);
        skillId = _id;
    }
    void SetIconSprite(Sprite _sprite)
    {
        iconImage.sprite = _sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if (!DragManager.Instance.isClick)
                {
                    if (SkillData.Instance.acquiredSkillLevels[skillId] != 0 && SkillDataBase.SkillDB[skillId].skillType != 1)
                    {
                        DragManager.Instance.DragStart(DragUIType.Skill, skillId, iconImage.sprite);
                    }
                }
                break;
            case PointerEventData.InputButton.Right:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DragManager.Instance.isClick)
        {
            SkillInformationUI.Instance.SetInformation(skillId);
        }
    }
}
