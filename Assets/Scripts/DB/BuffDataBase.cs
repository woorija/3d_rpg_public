using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
public class BuffData
{
    public int id { get; private set; }
    public int buffKey { get; private set; }
    public BuffData(int _id, int _buffKey)
    {
        id = _id;
        buffKey = _buffKey;
    }
    public void Reset()
    {
        id = 0;
        buffKey = 0;
    }
}
public class ItemBuffData : BuffData
{
    public float coolTime { get; private set; }
    public float interval { get; private set; }
    public float duration { get; private set; }
    public List<int> buffTypes { get; private set; }
    public List<int> buffOptions { get; private set; }
    public ItemBuffData(int _id, int _buffKey, float _coolTime, float _duration, float _interval, List<int> _buffTypes, List<int> _buffOptions) : base(_id, _buffKey)
    {
        coolTime = _coolTime;
        duration = _duration;
        interval = _interval;
        buffTypes = _buffTypes;
        buffOptions = _buffOptions;
    }
}
public class SkillBuffData : BuffData
{
    public List<int> buffTypes { get; private set; }
    public SkillBuffData(int _id, int _buffKey, List<int> _buffTypes) : base(_id, _buffKey)
    {
        buffTypes = _buffTypes;
    }
}
public class BuffDataBase : MonoBehaviour, ICSVRead
{
    public static Dictionary<int, ItemBuffData> itemBuffDB { get; private set; } = new Dictionary<int, ItemBuffData>();
    public static Dictionary<int, SkillBuffData> skillBuffDB { get; private set; } = new Dictionary<int, SkillBuffData>();
    int isComplete = 0;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("ItemBuffDB", ItemCSVLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("SkillBuffDB", SkillCSVLoad);
        await UniTask.WaitUntil(() => isComplete == 2);
    }
    void ItemCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<int> buffTypes;
        List<int> buffOptions;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            buffTypes = new List<int>();
            buffOptions = new List<int>();

            for (int j = 5; j < values.Length - 1; j += 2)
            {
                if (string.IsNullOrEmpty(values[j])) break;
                buffTypes.Add(CSVReader.GetIntData(values[j]));
                buffOptions.Add(CSVReader.GetIntData(values[j + 1]));
            }
            itemBuffDB.Add(CSVReader.GetIntData(values[0]), new ItemBuffData(CSVReader.GetIntData(values[0]), CSVReader.GetIntData(values[1]), CSVReader.GetFloatData(values[2]), CSVReader.GetFloatData(values[3]), CSVReader.GetFloatData(values[4]), buffTypes, buffOptions));
        }
        AddressableManager.Instance.ReleaseAsset("ItemBuffDB");
        isComplete++;
    }

    void SkillCSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<int> buffOptions;

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            buffOptions = new List<int>();

            for (int j = 2; j < values.Length; j++)
            {
                if (string.IsNullOrEmpty(values[j])) break;
                buffOptions.Add(CSVReader.GetIntData(values[j]));
            }
            skillBuffDB.Add(CSVReader.GetIntData(values[0]), new SkillBuffData(CSVReader.GetIntData(values[0]), CSVReader.GetIntData(values[1]), buffOptions));
        }
        AddressableManager.Instance.ReleaseAsset("SkillBuffDB");
        isComplete++;
    }

}
