using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image iconImage;

    protected void ChangeIcon(int _num)
    {
        if (_num == 0)
        {
            iconImage.sprite = null;
            return;
        }
        AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{_num}.png", SetIconSprite);
    }
    void SetIconSprite(Sprite _sprite)
    {
        iconImage.sprite = _sprite;
    }
    protected virtual void ChangeSlotData(ItemBase _item)
    {
        ChangeIcon(_item.itemId);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInformationUI.Instance.InformationClose();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
    }
}
