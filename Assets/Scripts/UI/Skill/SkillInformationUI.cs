using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;

public class SkillInformationUI : MonoBehaviour
{
    public static SkillInformationUI Instance;

    int skillId;
    float screenRatio;

    [SerializeField] GameObject informationUI;
    [SerializeField] Image skillIcon;
    [SerializeField] TMP_Text skillName;
    [SerializeField] TMP_Text skillDescription;
    [SerializeField] TMP_Text skillInformation;
    [SerializeField] RectTransform skillDescriptionBGRT;
    StringBuilder sb;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        sb = new StringBuilder(512);
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

            float limitX = Screen.width - (500 * screenRatio); ;
            float limitY = (skillDescriptionBGRT.rect.height + 100) * screenRatio;
            float tempX = 0;
            float tempY = 0;
            if (currentPos.x > limitX)
            {
                tempX = currentPos.x - limitX;
            }
            if (currentPos.y < limitY)
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
    public void SetInformation(int _id)
    {
        InformationOpen();
        if (skillId != _id)
        {
            skillId = _id;
            SetNameAndImage();
            SetDescription();
            SetSkillInfomations();
        }
    }
    void SetNameAndImage()
    {
        if (skillId == 0) return;
        AddressableManager.Instance.LoadAsset<Sprite>($"SkillIcon/S{skillId}.png", SetSprite);
        skillName.text = SkillDataBase.InfoDB[skillId].skillName;
    }
    void SetSprite(Sprite _sprite)
    {
        skillIcon.sprite = _sprite;
    }
    void SetDescription()
    {
        skillDescription.text = SkillDataBase.InfoDB[skillId].skillDescription;
    }
    public void SetSkillInfomations()
    {
        sb.Clear();
        var infoDB = SkillDataBase.InfoDB[skillId];
        var skillDB = SkillDataBase.SkillDB[skillId];

        if(skillDB.initialMp != 0)
        {
            sb.Append("MP ");
            sb.Append(SkillData.Instance.GetSkillUseMp(skillId));
            sb.Append(" 소비,");
        }

        sb.Append(infoDB.skillInformations[0]);
        for(int i = 0;i < skillDB.initialSkillMultiplier.Count; i++)
        {
            sb.Append($" {SkillData.Instance.GetSkillMultiplier(skillId, i)}{infoDB.skillInformations[i + 1]}");
        }
        skillInformation.text = sb.ToString();
    }
}
