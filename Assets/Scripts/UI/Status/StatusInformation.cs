using UnityEngine;
using TMPro;

public class StatusInformation : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text informationText;

    [SerializeField] string statusName;
    [field: SerializeField] public StatusType statusType {  get; private set; }
    [field: SerializeField] public StatusInformationType informationType { get; private set; }
    public void Start()
    {
        nameText.text = statusName;
    }
    public void SetText(int _value, int _value2 = 0)
    {
        switch(informationType)
        {
            case StatusInformationType.Normal:
                SetNormalStatusText(_value);
                break;
            case StatusInformationType.Percentage:
                SetPercentStatusText(_value);
                break;
            case StatusInformationType.CurrentMax:
                SetCurrentMaxStatusText(_value, _value2);
                break;
            case StatusInformationType.Total:
                SetTotalStatusText(_value, _value2);
                break;
        }
    }
    void SetNormalStatusText(int _value)
    {
        informationText.text = _value.ToString();
    }
    void SetPercentStatusText(int _value)
    {
        informationText.text = $"{(float)_value / 100:0.##}%";
    }
    void SetCurrentMaxStatusText(int _value, int _value2)
    {
        informationText.text = $"{_value} / {_value2}";
    }
    void SetTotalStatusText(int _value, int _value2)
    {
        if(_value2 == 0)
        {
            SetNormalStatusText(_value);
            return;
        }
        int total = _value + _value2;
        informationText.text = $"{total} ( {_value} + {_value2} )";
    }
}
