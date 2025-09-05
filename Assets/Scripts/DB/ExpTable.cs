using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ExpTable : MonoBehaviour, ICSVRead
{
    public static int[] expTable;
    bool isComplete = false;
    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("ExpDB", CSVLoad);
        await UniTask.WaitUntil(() => isComplete);
    }
    void CSVLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);
        expTable = new int[lines.Length];

        for(int i = 1; i < lines.Length; i++)
        {
            expTable[i] = CSVReader.GetIntData(lines[i]);
        }

        AddressableManager.Instance.ReleaseAsset("ExpDB");
        isComplete = true;
    }
}
