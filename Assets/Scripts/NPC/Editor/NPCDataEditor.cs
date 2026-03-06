#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TMPro;

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

        Undo.RegisterFullObjectHierarchyUndo(
            data.gameObject,
           "Set NPC Data"
        );

        SetTalkSO(data);
        SetShopSO(data);
        SetNameTag(data);
        SetCamLookAtTransform(data);
        data.gameObject.name = $"NPC{data.npcId}";
        Debug.Log($"NPC{data.npcId}의 데이터 세팅");
        EditorUtility.SetDirty(data);
        PrefabUtility.RecordPrefabInstancePropertyModifications(data.gameObject);
    }
    void SetCamLookAtTransform(NpcData _data)
    {
        Transform lookAtTransform = _data.transform.Find("LookAt");
        if (lookAtTransform != null)
        {
            _data.SetCamLookAtTransform(lookAtTransform);
        }
    }
    void SetTalkSO(NpcData _data)
    {
        string path = "Assets/EditorData/DB/TalkDataBase.cs";

        if (!File.Exists(path))
        {
            Debug.Log("TalkDataBase 파일을 찾을 수 없음");
            return;
        }

        string assetPath = $"Assets/Scripts/DB/TalkData/{_data.npcId}.asset";
        TalkDataSO so = AssetDatabase.LoadAssetAtPath<TalkDataSO>(assetPath);

        if (so == null)
        {
            so = CreateInstance<TalkDataSO>();
            AssetDatabase.CreateAsset(so, assetPath);
        }

        List<TalkData> talkData = TalkDataBase.TalkDB[_data.npcId];

        so.SetData(talkData);
        _data.SetTalkSO(so);
        EditorUtility.SetDirty(so);
    }
    void SetShopSO(NpcData _data)
    {
        string path = "Assets/EditorData/DB/ShopDataBase.cs";

        if(!File.Exists(path))
        {
            Debug.Log("ShopDataBase 파일을 찾을 수 없음");
            return;
        }

        if (ShopDataBase.ShopDB.ContainsKey(_data.npcId))
        {
            string assetPath = $"Assets/Scripts/DB/ShopData/{_data.npcId}.asset";
            ShopDataSO so = AssetDatabase.LoadAssetAtPath<ShopDataSO>(assetPath);

            if (so == null)
            {
                so = CreateInstance<ShopDataSO>();
                AssetDatabase.CreateAsset(so, assetPath);
            }

            so.SetData(ShopDataBase.ShopDB[_data.npcId]);
            _data.SetShopSO(so);
            EditorUtility.SetDirty(so);
        }
        else
        {
            _data.SetShopSO(null);
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
                sb.Append("\n[이동NPC]");
                break;
            case 4:
                sb.Append("\n[전직NPC]");
                break;
        }
        nameTag.SetNameTag(sb.ToString());
        EditorUtility.SetDirty(nameTag);

        TMP_Text tmp = nameTag.GetComponent<TMP_Text>();
        EditorUtility.SetDirty(tmp);
    }
}
#endif