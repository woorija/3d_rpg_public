using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.AI;

#if UNITY_EDITOR
[CustomEditor(typeof(BaseBlackBoard), true)]
public class MonsterBlackBoardEditor : Editor
{
    string monsterNameInput;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomEditorDrawer.DrawLine();
        monsterNameInput = EditorGUILayout.TextField("MonsterName", monsterNameInput);
        CustomEditorDrawer.DrawButton("SetUp", () => SetUp());
    }
    void SetUp()
    {
        var blackBoard = (BaseBlackBoard)target;
        SetDropTable();
        SetBlackBoardData();
        SetGameObjectData();
        EditorUtility.SetDirty(blackBoard);
    }
    void SetDropTable()
    {
        var blackBoard = (BaseBlackBoard)target;
        Type type = blackBoard.GetType();

        string typeName = type.Name;
        string monsterName = typeName.Replace("BlackBoard", "");
        string dropTablePath = $"Assets/Scripts/Monster/MonsterData/MonsterDropTable/{monsterName}DT.asset";

        DropTableSO dropTable = AssetDatabase.LoadAssetAtPath<DropTableSO>(dropTablePath);

        if (dropTable != null)
        {
            blackBoard.SetDropTableSO(dropTable);
        }
        else
        {
            dropTablePath = $"Assets/Scripts/Monster/MonsterData/MonsterDropTable/{monsterNameInput}DT.asset";
            dropTable = AssetDatabase.LoadAssetAtPath<DropTableSO>(dropTablePath);
            if (dropTable != null)
            {
                blackBoard.SetDropTableSO(dropTable);
            }
        }
    }
    void SetBlackBoardData()
    {
        var blackBoard = (BaseBlackBoard)target;
        Type type = blackBoard.GetType();

        string typeName = type.Name;
        string monsterName = typeName.Replace("BlackBoard", "");
        string blackBoardDataPath = $"Assets/Scripts/Monster/MonsterBT/Monster/{monsterName}/{monsterName}BlackBoardData.asset";
        
        MonsterBlackBoardSO blackBoardData = AssetDatabase.LoadAssetAtPath<MonsterBlackBoardSO>(blackBoardDataPath);

        if (blackBoardData != null)
        {
            blackBoard.SetBlackBoardDataSO(blackBoardData);
        }
        else
        {
            string subBlackBoardDataPath = $"Assets/Scripts/Monster/MonsterBT/Monster/{monsterNameInput}/{monsterNameInput}BlackBoardData.asset";
            blackBoardData = AssetDatabase.LoadAssetAtPath<MonsterBlackBoardSO>(subBlackBoardDataPath);
            if (blackBoardData != null)
            {
                blackBoard.SetBlackBoardDataSO(blackBoardData);
            }
        }
    }
    void SetGameObjectData()
    {
        var blackBoard = (BaseBlackBoard)target;
        NavMeshAgent agent;
        if (!blackBoard.TryGetComponent(out agent))
        {
            agent = blackBoard.gameObject.AddComponent<NavMeshAgent>();
        }
        blackBoard.SetAgent(agent);
        blackBoard.SetSubGameObject();
    }
}
#endif