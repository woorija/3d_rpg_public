using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BossHUD : MonoBehaviour
{
    public static BossHUD Instance;

    [SerializeField] Slider hpBar;
    [SerializeField] Image fillImage;
    [SerializeField] Image backgroundImage;

    [SerializeField] TMP_Text bossName;
    [SerializeField] TMP_Text remainingHp;
    [SerializeField] Color[] hpBarColors;
    int divideValue;

    int bossId;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public bool IsChangeHUD(int _id)
    {
        if(bossId == _id && gameObject.activeSelf)
        {
            return false;
        }
        return true;
    }
    public void SetHUD(int _id, int _hp, int _divide)
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        bossId = _id;
        bossName.text = MonsterNameDataBase.monsterNameDB[_id];
        divideValue = _divide;
        hpBar.maxValue = divideValue;
        ChangeHp(_hp);
    }
    public void ReleaseHUD()
    {
        gameObject.SetActive(false);
    }
    public void ChangeHp(int _hp)
    {
        int value = _hp % divideValue;
        int remaining = _hp / divideValue;
        if (value == 0 && remaining != 0)
        {
            hpBar.value = divideValue;
            remaining--;
        }
        else
        {
            hpBar.value = value;
        }
        if(remaining > 0)
        {
            remainingHp.SetText($"x {remaining}");
            fillImage.color = hpBarColors[remaining % 5];
            backgroundImage.color = hpBarColors[(remaining - 1) % 5];
        }
        else
        {
            remainingHp.text = null;
            fillImage.color = hpBarColors[0];
            backgroundImage.color = Color.gray;
        }
    }
}
