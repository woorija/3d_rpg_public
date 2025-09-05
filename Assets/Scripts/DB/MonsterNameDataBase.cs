using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNameDataBase : MonoBehaviour, ICSVRead
{
    bool complete = false;
    public static Dictionary<int, string> monsterNameDB {  get; private set; } = new Dictionary<int, string>();
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("MonsterNameDB", CSVLoad);
        await UniTask.WaitUntil(() => complete);
    }

    void CSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            monsterNameDB.Add(CSVReader.GetIntData(values[0]), values[1]);
        }
        AddressableManager.Instance.ReleaseAsset("MonsterNameDB");
        complete = true;
    }
}
