using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

#if UNITY_EDITOR
[CustomEditor(typeof(DropTableSO))]
public class DropTableSOEditor : Editor
{
    int monsterId;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawLabelField("MonsterId를 입력 후 SetUp버튼을 누르시오.");
        EditorGUILayout.Space(10);
        monsterId = EditorGUILayout.IntField("MonsterId", monsterId);
        CustomEditorDrawer.DrawButton("SetUp", () => SetUp());
    }

    void SetUp()
    {
        var currentData = (DropTableSO)target;
        currentData.Reset();
        SetUpReward(currentData);
        SetUpDropTable(currentData);
        EditorUtility.SetDirty(currentData);
    }

    void SetUpReward(DropTableSO _data)
    {
        string rewardPath = "Assets/EditorData/CSV/MonsterRewardDB.csv";
        if (!File.Exists(rewardPath)) return;
        string csv = File.ReadAllText(rewardPath);
        var lines = csv.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;
            if (CSVReader.GetIntData(values[0]) == monsterId)
            {
                _data.exp = CSVReader.GetIntData(values[1]);
                _data.minGold = CSVReader.GetIntData(values[2]);
                _data.maxGold = CSVReader.GetIntData(values[3]);
                return;
            }
        }
    }
    void SetUpDropTable(DropTableSO _data)
    {
        string dropTablePath = "Assets/EditorData/CSV/MonsterDropTable.csv";
        if (!File.Exists(dropTablePath)) return;


        string csv = File.ReadAllText(dropTablePath);
        var lines = csv.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);

        List<DropTable> dropTables = new List<DropTable>();

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;
            if (CSVReader.GetIntData(values[0]) == monsterId)
            {
                DropTable dropTable = new DropTable()
                {
                    itemNumber = CSVReader.GetIntData(values[1]),
                    minAmount = CSVReader.GetIntData(values[2]),
                    maxAmount = CSVReader.GetIntData(values[3]),
                    probability = CSVReader.GetFloatData(values[4])
                };
                dropTables.Add(dropTable);
            }
        }
        _data.dropTables = dropTables.ToArray();
    }
}
#endif
