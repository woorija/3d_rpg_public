using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
[CustomEditor(typeof(MonsterBlackBoardSO), true)]
public class BlackBoardDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("SetUp", () => SetUp());
    }

    void SetUp()
    {
        var currentData = (MonsterBlackBoardSO)target;
        if(currentData.id <= 0) return;

        string path = "Assets/EditorData/CSV/MonsterDB.csv";
        if (!File.Exists(path)) return;

        string csv = File.ReadAllText(path);
        var lines = csv.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.None);
        string[] headers = lines[0].Split(',');
        for(int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 0 || values[0] == "") continue;

            if (CSVReader.GetIntData(values[0]) == currentData.id)
            {
                SetUpDataCsvToSo(currentData, headers, values);
                EditorUtility.SetDirty(currentData);
                AssetDatabase.SaveAssets();
                return;
            }
        }
    }
    void SetUpDataCsvToSo(MonsterBlackBoardSO _data, string[] _headers, string[] _values)
    {
        var dataType = _data.GetType();

        for (int i = 0; i < _headers.Length; i++)
        {
            var field = dataType.GetField(_headers[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                object convertedValue = Convert.ChangeType(_values[i], field.FieldType);
                field.SetValue(_data, convertedValue);
            }
        }
    }
}
#endif
