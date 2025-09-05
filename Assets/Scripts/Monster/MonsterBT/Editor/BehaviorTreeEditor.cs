using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(BehaviorTree), true)]
public class BehaviorTreeEditor : Editor
{
    int selectedIndex = 0;
    bool showFoldout = false;
    enum NodeList
    {
        Selector = 1,
        RandomSelector,
        Sequence,
        If,
        BaseLogic
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        showFoldout = EditorGUILayout.Foldout(showFoldout, "BT Editor");
        if (showFoldout)
        {
            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawCenteredText("메인 노드 선택");

            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("Selector", null, (int)NodeList.Selector, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("RandomSelector", null, (int)NodeList.RandomSelector, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("Sequence", null, (int)NodeList.Sequence, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("If", null, (int)NodeList.If, ref selectedIndex);
            EditorGUILayout.EndHorizontal();

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawCenteredText("템플릿 선택");

            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("기본 로직", null, (int)NodeList.BaseLogic, ref selectedIndex);
            EditorGUILayout.EndHorizontal();

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();

            CustomEditorDrawer.DrawButton("SetUp RootNode", () => SetUpRootNode());

            CustomEditorDrawer.DrawLine();

            CustomEditorDrawer.DrawButton("Reset", () => RemoveNodes());

        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    void ApplySelection()
    {
        BehaviorTree BT = (BehaviorTree)target;

        RemoveNodes();

        switch (selectedIndex)
        {
            case 1:
                CreateNode<BT_SelectorNode>(BT, "RootNode (Selector)");
                break;
            case 2:
                CreateNode<BT_RandomSelectorNode>(BT, "RootNode (RandomSelector)");
                break;
            case 3:
                CreateNode<BT_SequenceNode>(BT, "RootNode (Sequence)");
                break;
            case 4:
                CreateNode<BT_IfNode>(BT, "RootNode (If)");
                break;
            case 5:
                CreateMainLogicPreset();
                break;
            default:
                break;
        }
    }
    void RemoveNodes()
    {
        BehaviorTree BT = (BehaviorTree)target;
        BT.GetRootNode(null);
        for (int i = BT.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(BT.transform.GetChild(i).gameObject);
        }
    }
    void CreateNode<T>(BehaviorTree _BT, string _childNodeName) where T : BT_Node
    {
        GameObject childObject = new GameObject(_childNodeName);
        childObject.transform.SetParent(_BT.transform);
        _BT.GetRootNode(childObject.AddComponent<T>());
    }
    void CreateMainLogicPreset()
    {
        BehaviorTree BT = (BehaviorTree)target;

        BT_IfNode preset = AssetDatabase.LoadAssetAtPath<BT_IfNode>("Assets/Prefabs/MonsterBT/RootNode (If).prefab");
        BT_IfNode instance = (BT_IfNode)PrefabUtility.InstantiatePrefab(preset, BT.transform);
        instance.transform.SetParent(BT.transform);
        BT.GetRootNode(instance);
    }
    void SetUpRootNode()
    {
        BehaviorTree BT = (BehaviorTree)target;
        BT.GetRootNode(BT.transform.GetChild(0).GetComponent<BT_Node>());
    }
}
#endif

