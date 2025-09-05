using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
public class Skill
{
    public int id; // 8자리
    public int prevSkillId;
    public int prevSkillLevel;
    public int skillType;
    public int initialAcquisitionLevel;
    public int increaseAcquisitionLevel;
    public int masterLevel;
    public int acquisitionSp;
    public int initialMp;
    public float increaseMp;
    public float coolTime;
    public float duration;
    public float interval;
    public List<int> initialSkillMultiplier;
    public List<int> increaseSkillMultiplier;
    public Skill(int _id, int _prevSkillId, int _prevSkillLevel, int _skillType, int _initialAcquisitionLevel, int _increaseAcquisitionLevel, int _masterLevel, int _acquisitionSp, int _initialMp, float _increaseMp, float _coolTime, float _duration, float _interval, List<int> _initialSkillMultipliers, List<int> _increaseSkillMultipliers)
    {
        id = _id;
        prevSkillId = _prevSkillId;
        prevSkillLevel = _prevSkillLevel;
        skillType = _skillType;
        initialAcquisitionLevel = _initialAcquisitionLevel;
        increaseAcquisitionLevel = _increaseAcquisitionLevel;
        masterLevel = _masterLevel;
        acquisitionSp = _acquisitionSp;
        initialMp = _initialMp;
        increaseMp = _increaseMp;
        coolTime = _coolTime;
        duration = _duration;
        interval = _interval;
        initialSkillMultiplier = _initialSkillMultipliers;
        increaseSkillMultiplier = _increaseSkillMultipliers;
    }
}
public class SkillInfomation
{
    public string skillName;
    public string skillDescription;
    public List<string> skillInformations;
    public SkillInfomation(string _skillName,string _skillDescription, List<string> _skillInformations)
    {
        skillName = _skillName;
        skillDescription = _skillDescription;
        skillInformations = _skillInformations;
    }
}

public class SkillDataBase : MonoBehaviour, ICSVRead
{
    const int capacity = 128;
    public static Dictionary<int, Skill> SkillDB { get; private set; } = new Dictionary<int, Skill>(capacity);
    public static Dictionary<int, SkillInfomation> InfoDB { get; private set; } = new Dictionary<int, SkillInfomation>(capacity);
    public static Dictionary<int, List<int>> ClassSkillTable { get; private set; } = new Dictionary<int, List<int>>(32);
    int isComplete = 0;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("SkillDB", SkillCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("SkillInformationDB", InformationCSVLoad);
        await UniTask.WaitUntil(() => isComplete == 2);
    }

    void SkillCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<int> initialSkillMultipliers;
        List<int> increaseSkillMultipliers;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            initialSkillMultipliers = new List<int>();
            increaseSkillMultipliers = new List<int>();

            for (int j = 13; j < values.Length - 1; j += 2)
            {
                if (string.IsNullOrEmpty(values[j])) break;

                initialSkillMultipliers.Add(CSVReader.GetIntData(values[j]));
                increaseSkillMultipliers.Add(CSVReader.GetIntData(values[j + 1]));
            }
            int skillId = CSVReader.GetIntData(values[0]);
            SkillDB.Add(skillId, new Skill(skillId, CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4]), CSVReader.GetIntData(values[5]), CSVReader.GetIntData(values[6]), CSVReader.GetIntData(values[7]), CSVReader.GetIntData(values[8]), CSVReader.GetFloatData(values[9]), CSVReader.GetFloatData(values[10]), CSVReader.GetFloatData(values[11]), CSVReader.GetFloatData(values[12]), initialSkillMultipliers, increaseSkillMultipliers));

            //직업별 스킬 테이블 구성
            int classRank = skillId / 10000;
            if (!ClassSkillTable.ContainsKey(classRank))
            {
                ClassSkillTable[classRank] = new List<int>();
            }
            ClassSkillTable[classRank].Add(skillId);
        }
        AddressableManager.Instance.ReleaseAsset("SkillDB");
        isComplete++;
    }

    void InformationCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<string> informations;
        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            informations = new List<string>();

            for (int j = 3; j < values.Length; j++)
            {
                if (string.IsNullOrEmpty(values[j])) break;

                informations.Add(CSVReader.GetStringData(values[j]));
            }
            InfoDB.Add(CSVReader.GetIntData(values[0]), new SkillInfomation(values[1], values[2], informations));
        }
        AddressableManager.Instance.ReleaseAsset("SkillInformationDB");
        isComplete++;
    }
}
