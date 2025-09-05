using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image iconImage;
    [SerializeField] Image cooltimeBar;
    [SerializeField] TMP_Text itemCountText;
    [SerializeField] TMP_Text slotKeyText;
    [SerializeField] int index; // 슬롯 인덱스는 0~7
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DragManager.Instance.isClick && (DragManager.Instance.dragItemType.Equals(ItemType.Useable) || DragManager.Instance.dragType.Equals(DragUIType.Skill)))
            {
                DragManager.Instance.DragEndToQuickSlot(index);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        { // 사용 가능한 상태에 우클릭 시 슬롯 초기화
            if (QuickSlotData.Instance.IsAvailable(index))
            {
                QuickSlotData.Instance.ResetSlot(index);
                DataManager.Instance.SaveQuickSlot();
            }
        }
    }
    public void UpdateCooltimeBar(float _fillAmount)
    {
        cooltimeBar.fillAmount = _fillAmount;
    }
    public void UpdateCountText(UseType _type, int _id)
    {
        switch(_type)
        {
            case UseType.Item:
                itemCountText.text = InventoryData.Instance.GetItemCount(_id).ToString();
                break;
            case UseType.Skill:
                itemCountText.text = string.Empty;
                break;
        }
    }
    public void UpdateKeyText(string _text)
    {
        slotKeyText.text = _text;
    }
    void TextReset()
    {
        itemCountText.text = string.Empty;
    }
    public void SetSlot(UseType _type, int _id)
    {
        switch (_type)
        {
            case UseType.Item:
                AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{_id}.png", SetIconSprite);
                break;
            case UseType.Skill:
                AddressableManager.Instance.LoadAsset<Sprite>($"SkillIcon/S{_id}.png", SetIconSprite);
                break;

        }
    }
    void SetIconSprite(Sprite _sprite)
    {
        iconImage.sprite = _sprite;
    }
    public void Reset()
    {
        iconImage.sprite = null;
        TextReset();
    }

}
