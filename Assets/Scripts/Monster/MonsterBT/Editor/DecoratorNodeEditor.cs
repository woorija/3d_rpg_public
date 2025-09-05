using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework.Interfaces;

#if UNITY_EDITOR
[CustomEditor(typeof(BT_DecoratorNode), true)]
public class DecoratorNodeEditor : Editor
{
    int selectedIndex = 0;
    bool showFoldout = false;
    enum NodeList
    {
        Selector = 1,
        RandomSelector,
        Sequence,
        If,
        Success,
        Failure,
        Action,
        DiePreset,
        StaggerPreset
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        showFoldout = EditorGUILayout.Foldout(showFoldout, "Node Editor");
        if (showFoldout)
        {
            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawCenteredText("추가할 노드 선택");

            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("Selector", null, (int)NodeList.Selector, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("RandomSelector", null, (int)NodeList.RandomSelector, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("Sequence", null, (int)NodeList.Sequence, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("If", null, (int)NodeList.If, ref selectedIndex);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("Success", null, (int)NodeList.Success, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("Failure", null, (int)NodeList.Failure, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("Action", null, (int)NodeList.Action, ref selectedIndex);
            EditorGUILayout.EndHorizontal();

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawCenteredText("프리셋 선택");
            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("Die Preset", null, (int)NodeList.DiePreset, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("Stagger Preset", null, (int)NodeList.StaggerPreset, ref selectedIndex);
            EditorGUILayout.EndHorizontal();

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawButton("SetUp IfNode\n자식노드 3개필요", () => SetUpIfNode());
            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawButton("Reset", () => ResetChildNode());
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    void ApplySelection()
    {
        switch (selectedIndex)
        {
            case <= 7:
                CreateBaseNode(selectedIndex);
                break;
            case 8:
                CreateDiePreset();
                break;
            case 9:
                CreateStaggerPreset();
                break;
            default:
                break;
        }
    }
    void CreateBaseNode(int _index)
    {
        BT_DecoratorNode node = (BT_DecoratorNode)target;
        switch (_index)
        {
            case 1:
                CreateNode<BT_SelectorNode>(node, "SelectorNode");
                break;
            case 2:
                CreateNode<BT_RandomSelectorNode>(node, "RandomSelectorNode");
                break;
            case 3:
                CreateNode<BT_SequenceNode>(node, "SequenceNode");
                break;
            case 4:
                CreateNode<BT_IfNode>(node, "IfNode");
                break;
            case 5:
                CreateNode<BT_SuccessNode>(node, "SuccessNode");
                break;
            case 6:
                CreateNode<BT_FailureNode>(node, "FailureNode");
                break;
            case 7:
                CreateNode<BT_ActionNode>(node, "ActionNode");
                break;
        }
    }
    void ResetChildNode()
    {
        BT_DecoratorNode node = (BT_DecoratorNode)target;
        node.ResetNode();
        for (int i = node.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(node.transform.GetChild(i).gameObject);
        }
    }
    T CreateNode<T>(BT_Node _node, string _childNodeName) where T : BT_Node
    {
        GameObject childObject = new GameObject(_childNodeName);
        childObject.transform.SetParent(_node.transform);
        return childObject.AddComponent<T>();
    }
    void CreateDiePreset()
    {
        BT_IfNode node = (BT_IfNode)target;
        node.SetChildNode(CreateNode<BT_CheckDie>(node, "CheckDie"));
        node.SetSuccessNode(CreateNode<BT_Die>(node, "Die"));
        node.SetFailureNode(CreateNode<BT_IfNode>(node, "If(stagger logic)"));
    }
    void CreateStaggerPreset()
    {
        BT_IfNode node = (BT_IfNode)target;
        node.SetChildNode(CreateNode<BT_CheckStagger>(node, "CheckStagger"));
        node.SetSuccessNode(CreateNode<BT_Stagger>(node, "Stagger"));
        node.SetFailureNode(CreateNode<BT_SelectorNode>(node, "Selector(main logic)"));
    }
    void SetUpIfNode()
    {
        BT_IfNode node = (BT_IfNode)target;
        node.ResetNode();
        Transform child = node.transform.GetChild(0);
        BT_Node childNode = child.GetComponent<BT_Node>();
        node.SetChildNode(childNode);
        Transform success = node.transform.GetChild(1);
        BT_Node successNode = success.GetComponent<BT_Node>();
        node.SetSuccessNode(successNode);
        Transform failure = node.transform.GetChild(2);
        BT_Node failureNode = failure.GetComponent<BT_Node>();
        node.SetFailureNode(failureNode);
    }
}
#endif