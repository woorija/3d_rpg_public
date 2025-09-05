using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemInformation : MonoBehaviour
{
    CraftItemData itemData;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemCountText;
    public void SetData(CraftItemData _data)
    {
        itemData = _data;
        AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{itemData.itemId}.png", SetIconSprite);
    }
    void SetIconSprite(Sprite _sprite)
    {
        itemImage.sprite = _sprite;
    }
    public void SetText(CraftType _type)
    {
        itemNameText.text = ItemDataBase.GetItemName(itemData.itemId);
        switch(_type)
        {
            case CraftType.Materials:
                itemCountText.text = $"{InventoryData.Instance.GetItemCount(itemData.itemId)} / {itemData.itemAmount}";
                break;
            case CraftType.Result:
                itemCountText.text = $"X {itemData.itemAmount}";
                break;
        }
    }
    public void SetNull()
    {
        AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/ClearImage.png", SetIconSprite);
        itemNameText.text = null;
        itemCountText.text = null;
    }
}
