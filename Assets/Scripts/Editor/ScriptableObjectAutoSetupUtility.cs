#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public static class ScriptableObjectAutoSetupUtility
{
    static string monsterSOPath = "Assets/Scripts/Monster/MonsterBT/Monster";
    static string monsterDTPath = "Assets/Scripts/Monster/MonsterData/MonsterDropTable";
    #region MonsterData
    public static void SetAllMonsterSO()
    {
        // csv to cs가 이루어졌는지 체크
        if(MonsterDataBase.MonsterDB == null || MonsterDataBase.MonsterDB.Count == 0)
        {
            Debug.LogWarning("몬스터DB가 존재하지 않습니다.");
            return;
        }
        

        string[] guids = AssetDatabase.FindAssets("t:MonsterBlackBoardSO", new[] { monsterSOPath });
        Dictionary<int, string> existingSOMap = new Dictionary<int, string>();

        // 생성되어있는 so파일 체크
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MonsterBlackBoardSO so = AssetDatabase.LoadAssetAtPath<MonsterBlackBoardSO>(path);

            if (so != null)
            {
                existingSOMap[so.id] = path;
            }
        }

        // DB상에 존재하는 데이터를 SO에 세팅
        foreach (var kvp in MonsterDataBase.MonsterDB)
        {
            int id = kvp.Key;
            MonsterData data = kvp.Value;

            if (existingSOMap.ContainsKey(id))
            {
                UpdateMonsterDataSO(existingSOMap[id], data);
                existingSOMap.Remove(id);
            }
            else
            {
                CreateMonsterDataSO(id, data);
            }
        }

        // DB상에 존재하지 않는 SO 표기
        foreach (var kvp in existingSOMap)
        {
            Debug.LogWarning($"현재 사용중이지 않은 ID: {kvp.Key}인 몬스터블랙보드SO가 {kvp.Value}에 존재합니다.");
        }
    }
    static void UpdateMonsterDataSO(string _path, MonsterData _data)
    {
        MonsterBlackBoardSO so = AssetDatabase.LoadAssetAtPath<MonsterBlackBoardSO>(_path);

        if (so != null)
        {
            so.id = _data.id;
            so.level = _data.level;
            so.maxHp = _data.maxHp;
            so.trackingRange = _data.trackingRange;
            so.limitTrackingRange = _data.limitTrackingRange;
            so.limitTrackingHeight = _data.limitTrackingHeight;
            so.minIdleTime = _data.minIdleTime;
            so.maxIdleTime = _data.maxIdleTime;
            so.idleMoveSpeed = _data.idleMoveSpeed;
            so.trackingMoveSpeed = _data.trackingMoveSpeed;
            so.returnMoveSpeed = _data.returnMoveSpeed;
            so.respawnTime = _data.respawnTime;

            EditorUtility.SetDirty(so);
        }
    }
    static void CreateMonsterDataSO(int _id, MonsterData _data)
    {
        if (!Directory.Exists($"{monsterSOPath}/Temp"))
        {
            Directory.CreateDirectory($"{monsterSOPath}/Temp");
        }

        MonsterBlackBoardSO so = ScriptableObject.CreateInstance<MonsterBlackBoardSO>();
        so.id = _data.id;
        so.level = _data.level;
        so.maxHp = _data.maxHp;
        so.trackingRange = _data.trackingRange;
        so.limitTrackingRange = _data.limitTrackingRange;
        so.limitTrackingHeight = _data.limitTrackingHeight;
        so.minIdleTime = _data.minIdleTime;
        so.maxIdleTime = _data.maxIdleTime;
        so.idleMoveSpeed = _data.idleMoveSpeed;
        so.trackingMoveSpeed = _data.trackingMoveSpeed;
        so.returnMoveSpeed = _data.returnMoveSpeed;
        so.respawnTime = _data.respawnTime;

        string assetPath = $"{monsterSOPath}/Temp/MonsterData_{_id}.asset";
        AssetDatabase.CreateAsset(so, assetPath);
    }
    #endregion
    #region MonsterDropTable
    public static void SetAllMonsterDropTableSO()
    {
        // csv to cs가 이루어졌는지 체크
        if (MonsterRewardDataBase.MonsterRewardDB == null || MonsterRewardDataBase.MonsterRewardDB.Count == 0 || MonsterDropTableDataBase.monsterDropTableDB == null)
        {
            Debug.LogWarning("몬스터리워드DB 또는 몬스터드랍테이블DB가 존재하지 않습니다.");
            return;
        }


        string[] guids = AssetDatabase.FindAssets("t:DropTableSO", new[] { monsterDTPath });
        Dictionary<int, string> existingSOMap = new Dictionary<int, string>();

        // 생성되어있는 so파일 체크
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DropTableSO so = AssetDatabase.LoadAssetAtPath<DropTableSO>(path);

            if (so != null)
            {
                existingSOMap[so.id] = path;
            }
        }

        // DB상에 존재하는 데이터를 SO에 세팅
        foreach (var kvp in MonsterRewardDataBase.MonsterRewardDB)
        {
            int id = kvp.Key;
            MonsterBaseReward rewardData = kvp.Value;

            MonsterDropTableDataBase.monsterDropTableDB.TryGetValue(id, out var dropTables);

            if (existingSOMap.ContainsKey(id))
            {
                UpdateMonsterDropTableSO(id, existingSOMap[id], rewardData, dropTables);
                existingSOMap.Remove(id);
            }
            else
            {
                CreateMonsterDropTableSO(id, rewardData, dropTables);
            }
        }

        // DB상에 존재하지 않는 SO 표기
        foreach (var kvp in existingSOMap)
        {
            Debug.LogWarning($"현재 사용중이지 않은 ID: {kvp.Key}인 몬스터드랍테이블SO가 {kvp.Value}에 존재합니다.");
        }
    }
    static void UpdateMonsterDropTableSO(int _id, string _path, MonsterBaseReward _data, List<DropTable> _dropTables)
    {
        DropTableSO so = AssetDatabase.LoadAssetAtPath<DropTableSO>(_path);

        if (so != null)
        {
            so.id = _id;
            so.exp = _data.exp;
            so.minGold = _data.minGold;
            so.maxGold = _data.maxGold;

            if(_dropTables != null)
            {
                so.SetTable(_dropTables);
            }
            else
            {
                so.SetTable(null);
            }

            EditorUtility.SetDirty(so);
        }
    }
    static void CreateMonsterDropTableSO(int _id, MonsterBaseReward _data, List<DropTable> _dropTables)
    {
        if (!Directory.Exists(monsterDTPath))
        {
            Directory.CreateDirectory(monsterDTPath);
        }

        DropTableSO so = ScriptableObject.CreateInstance<DropTableSO>();
        so.id = _id;
        so.exp = _data.exp;
        so.minGold = _data.minGold;
        so.maxGold = _data.maxGold;

        if (_dropTables != null)
        {
            so.SetTable(_dropTables);
        }
        else
        {
            so.SetTable(null);
        }

        string assetPath = $"{monsterDTPath}/{_id}DT.asset";
        AssetDatabase.CreateAsset(so, assetPath);
    }
    #endregion
}
#endif