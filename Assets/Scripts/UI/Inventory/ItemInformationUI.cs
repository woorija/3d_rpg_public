using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class ItemInformationUI : MonoBehaviour
{
    public static ItemInformationUI Instance;
    ItemBase item;
    StringBuilder sb;
    static readonly StatusType[] itemStatusTypes = new[]
    {
        StatusType.Strength,
        StatusType.Dexterity,
        StatusType.Vitality,
        StatusType.Intelligence,
        StatusType.Wisdom,
        StatusType.Agility,
        StatusType.PhysicalAttackPower,
        StatusType.MagicAttackPower,
        StatusType.PhysicalDefensePower,
        StatusType.MagicDefensePower,
        StatusType.Accuracy,
        StatusType.Evasion,
        StatusType.Hp,
        StatusType.Mp,
        StatusType.Stamina,
        StatusType.CriticalRate,
        StatusType.CriticalDamage,
        StatusType.AttackSpeed,
        StatusType.MoveSpeed
    };

    static readonly Dictionary<StatusType, string> statusNameMap = new()
    {
        { StatusType.Strength, "STR" },
        { StatusType.Dexterity, "DEX" },
        { StatusType.Vitality, "VIT" },
        { StatusType.Intelligence, "INT" },
        { StatusType.Wisdom, "WIS" },
        { StatusType.Agility, "AGI" },
        { StatusType.PhysicalAttackPower, "물리공격력" },
        { StatusType.MagicAttackPower, "마법공격력" },
        { StatusType.PhysicalDefensePower, "물리방어력" },
        { StatusType.MagicDefensePower, "마법방어력" },
        { StatusType.Accuracy, "명중률" },
        { StatusType.Evasion, "회피율" },
        { StatusType.Hp, "HP" },
        { StatusType.Mp, "MP" },
        { StatusType.Stamina, "스태미너" },
        { StatusType.CriticalRate, "크리티컬확률" },
        { StatusType.CriticalDamage, "크리티컬데미지" },
        { StatusType.AttackSpeed, "공격속도" },
        { StatusType.MoveSpeed, "이동속도" }
    };

    [SerializeField] GameObject informationUI;
    [SerializeField] Image itemIcon;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text equipmentItemInformation;
    RectTransform equipmentItemInformationRT;
    [SerializeField] TMP_Text baseItemInformation;
    RectTransform baseItemInformationRT;

    float screenRatio;
    private void Awake()
    {
        Instance = this;
        equipmentItemInformationRT = equipmentItemInformation.GetComponent<RectTransform>();
        baseItemInformationRT = baseItemInformation.GetComponent<RectTransform>();
    }
    private void Start()
    {
        sb = new StringBuilder();
        InformationClose();
    }
    private void OnRectTransformDimensionsChange()
    {
        screenRatio = Screen.width / 1920f;
    }
    private void LateUpdate()
    {
        if (informationUI.activeSelf)
        {
            Vector3 currentPos = Mouse.current.position.ReadValue();

            float limitX = Screen.width - (500 * screenRatio);
            float limitY = Mathf.Max(baseItemInformationRT.rect.height + 140, equipmentItemInformationRT.rect.height + 260) * screenRatio;
            float tempX = 0;
            float tempY = 0;
            if(currentPos.x > limitX)
            {
                tempX = currentPos.x - limitX;
            }
            if(currentPos.y < limitY)
            {
                tempY = limitY - currentPos.y;
            }
            transform.position = new Vector3(currentPos.x - tempX, currentPos.y + tempY, currentPos.z);
        }
    }
    public void InformationOpen()
    {
        if (!informationUI.activeSelf)
        {
            informationUI.SetActive(true);
        }
    }
    public void InformationClose()
    {
        if (informationUI.activeSelf)
        {
            informationUI.SetActive(false);
        }
    }
    public void SetInformation(ItemBase _item)
    {
        InformationOpen();
        item = _item;
        if(item.itemId < 200000000)
        {
            SetEquipmentInformation();
        }
        else
        {
            SetEmptyEquipmentInformation();
        }
        SetNameAndImage();
        SetBaseInformation();
    }
    void SetNameAndImage()
    {
        AddressableManager.Instance.LoadAsset<Sprite>($"ItemIcon/I{item.itemId}.png", SetSprite);
        itemName.text = item.name;
    }
    void SetSprite(Sprite _sprite)
    {
        itemIcon.sprite = _sprite;
    }
    void SetEmptyEquipmentInformation()
    {
        equipmentItemInformation.text = string.Empty;
    }
    void AppendStatusInformation(string _name, int _value)
    {
        if (sb.Length > 0)
        {
            sb.Append("\n");
        }
        sb.Append(_name);
        sb.Append(" + ");
        sb.Append(_value);
    }
    void SetEquipmentInformation()
    {
        EquipmentItem _item = (EquipmentItem)item;
        sb.Clear();

        foreach(var status in itemStatusTypes)
        {
            if(_item.options.TryGetValue(status, out int value) && value != 0)
            {
                AppendStatusInformation(statusNameMap[status], value);
            }
        }
        equipmentItemInformation.text = sb.ToString();
    }
    void SetBaseInformation()
    {
        baseItemInformation.text = item.description;
    }
}
