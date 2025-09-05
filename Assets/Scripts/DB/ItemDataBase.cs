using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemDataBase : MonoBehaviour, ICSVRead
{
    //장비아이템은 데이터상으로 1개
    //소비,기타 아이템은 데이터상으로 0개 시작
    public static Dictionary<int, EquipmentItem> EquipmentItemDB { get; private set; } = new Dictionary<int, EquipmentItem>();
    public static Dictionary<int, UseableItem> UseableItemDB { get; private set; } = new Dictionary<int, UseableItem>();
    public static Dictionary<int, MiscItem> MiscItemDB { get; private set; } = new Dictionary<int, MiscItem>();
    int complete = 0;

    public async UniTask ReadCSV()
    {
        AddressableManager.Instance.LoadAsset<TextAsset>("EquipmentItemDB", EquipmentLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("UseableItemDB", UseableLoad);
        AddressableManager.Instance.LoadAsset<TextAsset>("MiscItemDB", MiscLoad);
        await UniTask.WaitUntil(() => complete == 3);
    }
    public static string GetItemName(int _id)
    {
        string itemName;
        if(_id >= 300000000)
        {
            itemName = MiscItemDB[_id].name;
        }
        else if(_id >= 200000000)
        {
            itemName = UseableItemDB[_id].name;
        }
        else
        {
            itemName = EquipmentItemDB[_id].name;
        }
        return itemName;
    }
    void EquipmentLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            Dictionary<StatusType,int> options = new Dictionary<StatusType,int>();

            for(int j = 4; j + 1 < values.Length; j += 2)
            {
                if (string.IsNullOrEmpty(values[j])) 
                {
                    break;
                }

                int typeToInt = CSVReader.GetIntData(values[j]);
                int optionValue = CSVReader.GetIntData(values[j+1]);

                switch(typeToInt)
                {
                    case 101:
                        options.Add(StatusType.Strength, optionValue);
                        options.Add(StatusType.Dexterity, optionValue);
                        options.Add(StatusType.Vitality, optionValue);
                        options.Add(StatusType.Intelligence, optionValue);
                        options.Add(StatusType.Wisdom, optionValue);
                        options.Add(StatusType.Agility, optionValue);
                        break;
                    default:
                        options.Add((StatusType)typeToInt, optionValue);
                        break;
                }
            }

            EquipmentItemDB.Add(CSVReader.GetIntData(values[0]), new EquipmentItem(CSVReader.GetIntData(values[0]), values[1], values[2], CSVReader.GetIntData(values[3]), options));
        }
        AddressableManager.Instance.ReleaseAsset("EquipmentItemDB");
        complete++;
    }
    void UseableLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            UseableItemDB.Add(CSVReader.GetIntData(values[0]), new UseableItem(CSVReader.GetIntData(values[0]), values[1], values[2], CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4])));
        }
        AddressableManager.Instance.ReleaseAsset("UseableItemDB");
        complete++;
    }
    void MiscLoad(TextAsset _csv)
    {
        var lines = _csv.text.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            MiscItemDB.Add(CSVReader.GetIntData(values[0]), new MiscItem(CSVReader.GetIntData(values[0]), values[1], values[2], CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4])));
        }
        AddressableManager.Instance.ReleaseAsset("MiscItemDB");
        complete++;
    }
}
