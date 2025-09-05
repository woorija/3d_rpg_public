#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

[CustomEditor(typeof(NpcData))]
public class NPCDataEditor : Editor
{
    [SerializeField] NpcNameTag prefab;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("데이터 세팅", SetData);
    }

    void SetData()
    {
        NpcData data = (NpcData)target;
        string npcId = data.npcId.ToString();
        
        SetTalkSO(data, npcId);
        SetShopSO(data, npcId);
        SetNameTag(data);
        SetCamLookAtTransform(data);
        Debug.Log($"NPC{npcId}의 데이터 세팅");
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void SetCamLookAtTransform(NpcData _data)
    {
        Transform lookAtTransform = _data.transform.Find("LookAt");
        if (lookAtTransform != null)
        {
            _data.SetCamLookAtTransform(lookAtTransform);
        }
    }
    void SetTalkSO(NpcData _data, string _id)
    {
        string path = "Assets/EditorData/DB/TalkDataBase.cs";

        if (!File.Exists(path))
        {
            Debug.Log("TalkDataBase 파일을 찾을 수 없음");
            return;
        }

        string assetPath = $"Assets/Scripts/DB/TalkData/{_id}.asset";
        TalkDataSO so = AssetDatabase.LoadAssetAtPath<TalkDataSO>(assetPath);

        if (so == null)
        {
            so = CreateInstance<TalkDataSO>();
            AssetDatabase.CreateAsset(so, assetPath);
        }

        List<TalkData> talkData = TalkDataBase.TalkDB[int.Parse(_id)];

        so.SetData(talkData);
        _data.SetTalkSO(so);
        EditorUtility.SetDirty(so);
    }
    void SetShopSO(NpcData _data, string _id)
    {
        string path = "Assets/EditorData/DB/ShopDataBase.cs";

        if(!File.Exists(path))
        {
            Debug.Log("ShopDataBase 파일을 찾을 수 없음");
            return;
        }

        int id = int.Parse(_id);
        if (ShopDataBase.ShopDB.ContainsKey(id))
        {
            string assetPath = $"Assets/Scripts/DB/ShopData/{_id}.asset";
            ShopDataSO so = AssetDatabase.LoadAssetAtPath<ShopDataSO>(assetPath);

            if (so == null)
            {
                so = CreateInstance<ShopDataSO>();
                AssetDatabase.CreateAsset(so, assetPath);
            }

            so.SetData(ShopDataBase.ShopDB[id]);
            _data.SetShopSO(so);
            EditorUtility.SetDirty(so);
        }
    }

    void SetNameTag(NpcData _data)
    {
        NpcNameTag nameTag = _data.GetComponentInChildren<NpcNameTag>();
        if (nameTag == null)
        {
            nameTag = (NpcNameTag)PrefabUtility.InstantiatePrefab(prefab);
            nameTag.transform.SetParent(_data.transform, false);
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(NPCDataBase.NPCDB[_data.npcId].name);
        switch (NPCDataBase.NPCDB[_data.npcId].type)
        {
            case 1:
                sb.Append("\n[상점NPC]");
                break;
            case 2:
                sb.Append("\n[합성NPC]");
                break;
            case 3:
                sb.Append("\n[전직NPC]");
                break;
        }
        nameTag.SetNameTag(sb.ToString());
    }
}
#endif