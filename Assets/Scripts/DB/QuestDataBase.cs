using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;

public class QuestDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, QuestData> QuestDB { get; private set; } = new Dictionary<int, QuestData>();
    public static Dictionary<int, QuestRewards> RewardDB { get; private set; } = new Dictionary<int, QuestRewards>();
    public static Dictionary<int, QuestSpecialRewards> SepcialRewardDB { get; private set; } = new Dictionary<int, QuestSpecialRewards>();
    public static Dictionary<int, QuestInformation> InfoDB { get; private set; } = new Dictionary<int, QuestInformation>();
    int isComplete = 0;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("QuestDB", QuestCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("QuestRewardDB", RewardCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("QuestSpecialRewardDB", SpecialRewardCSVLoad);
        await UniTask.WaitUntil(() => isComplete == 3);
    }
    void QuestCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<int> ids;
        List<int> counts;
        HashSet<int> preQuestIds;
        HashSet<int> incompatibleQuestIds;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            ids = new List<int>();
            counts = new List<int>();
            preQuestIds = new HashSet<int>();
            incompatibleQuestIds = new HashSet<int>();

            for (int j = 10; j < values.Length - 1; j += 2)
            {
                if (values[j] == "") break;
                ids.Add(CSVReader.GetIntData(values[j]));
                counts.Add(CSVReader.GetIntData(values[j+1]));
            }

            if (!values[7].Equals("0"))
            {
                var preQuestIdArray = values[7].Split('|');
                for (int j = 0; j < preQuestIdArray.Length; j++)
                {
                    preQuestIds.Add(CSVReader.GetIntData(preQuestIdArray[j]));
                }
            }

            if (!values[8].Equals("0"))
            {
                var incompatibleQuestIdArray = values[8].Split('|');
                for (int j = 0; j < incompatibleQuestIdArray.Length; j++)
                {
                    incompatibleQuestIds.Add(CSVReader.GetIntData(incompatibleQuestIdArray[j]));
                }
            }
            InfoDB.Add(CSVReader.GetIntData(values[0]), new QuestInformation(values[1], values[2], values[3], values[4]));
            QuestDB.Add(CSVReader.GetIntData(values[0]), new QuestData(CSVReader.GetIntData(values[0]), (QuestType)CSVReader.GetIntData(values[5]), CSVReader.GetIntData(values[6]), preQuestIds, incompatibleQuestIds, CSVReader.GetIntData(values[9]), ids, counts));
        }
        AddressableManager.Instance.ReleaseAsset("QuestDB");
        isComplete++;
    }

    void RewardCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<int> ids;
        List<int> counts;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            ids = new List<int>();
            counts = new List<int>();

            for (int j = 3; j < values.Length - 1; j += 2)
            {
                if (string.IsNullOrEmpty(values[j])) break;
                ids.Add(CSVReader.GetIntData(values[j]));
                counts.Add(CSVReader.GetIntData(values[j + 1]));
            }
            RewardDB.Add(CSVReader.GetIntData(values[0]), new QuestRewards(CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]), ids, counts));
        }
        AddressableManager.Instance.ReleaseAsset("QuestRewardDB");
        isComplete++;
    }

    void SpecialRewardCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;

            SepcialRewardDB.Add(CSVReader.GetIntData(values[0]), new QuestSpecialRewards(CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2])));
        }
        AddressableManager.Instance.ReleaseAsset("QuestSpecialRewardDB");
        isComplete++;
    }
}
