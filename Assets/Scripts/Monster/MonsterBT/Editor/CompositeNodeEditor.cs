using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(BT_CompositeNode), true)]
public class CompositeNodeEditor : Editor
{
    int selectedIndex = 0;
    bool showFoldout = false;
    enum NodeList
    {
        Selector=1,
        RandomSelector,
        Sequence,
        If,
        Action,
        NormalAttackPreset,
        ReturnPreset,
        TrackingPreset,
        IdlePreset
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
            CustomEditorDrawer.DrawButtonStyleToggle("Action", null, (int)NodeList.Action, ref selectedIndex);

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawCenteredText("프리셋 선택");

            EditorGUILayout.BeginHorizontal();
            CustomEditorDrawer.DrawButtonStyleToggle("기본공격 프리셋", null, (int)NodeList.NormalAttackPreset, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("리턴 프리셋", null, (int)NodeList.ReturnPreset, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("추적 프리셋", null, (int)NodeList.TrackingPreset, ref selectedIndex);
            CustomEditorDrawer.DrawButtonStyleToggle("대기 프리셋", null, (int)NodeList.IdlePreset, ref selectedIndex);
            EditorGUILayout.EndHorizontal();

            CustomEditorDrawer.DrawButton("Apply Selection", () => ApplySelection());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawButton("SetUp", () => SetChildNodes());

            CustomEditorDrawer.DrawLine();
            CustomEditorDrawer.DrawButton("Reset", () => ResetChildNode());
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void ResetChildNode()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        node.ResetNode();
        for (int i = node.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(node.transform.GetChild(i).gameObject);
        }
    }
    void ApplySelection()
    {
        switch (selectedIndex)
        {
            case <= 5:
                CreateBaseNode(selectedIndex);
                break;
            case 6:
                CreateNormalAttackPreset();
                break;
            case 7:
                CreateReturnPreset();
                break;
            case 8:
                CreateTrackingPreset();
                break;
            case 9:
                CreateIdlePreset();
                break;
            default:
                break;
        }
    }

    T CreateNode<T>(BT_Node _node, string _childNodeName) where T : BT_Node
    {
        GameObject childObject = new GameObject(_childNodeName);
        childObject.transform.SetParent(_node.transform);
        return childObject.AddComponent<T>();
    }
    void CreateBaseNode(int _index)
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        switch (_index)
        {
            case 1:
                node.AddNode(CreateNode<BT_SelectorNode>(node, "SelectorNode"));
                break;
            case 2:
                node.AddNode(CreateNode<BT_RandomSelectorNode>(node, "RandomSelectorNode"));
                break;
            case 3:
                node.AddNode(CreateNode<BT_SequenceNode>(node, "SequenceNode"));
                break;
            case 4:
                node.AddNode(CreateNode<BT_IfNode>(node, "IfNode"));
                break;
            case 5:
                node.AddNode(CreateNode<BT_ActionNode>(node, "ActionNode"));
                break;
        }
    }
    void CreateNormalAttackPreset()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(NormalAttack logic)");
        node.AddNode(sequenceNode);
        sequenceNode.AddNode(CreateNode<BT_CheckAttackRange>(sequenceNode, "CheckAttackRange"));

        BT_IfNode ifNode = CreateNode<BT_IfNode>(sequenceNode, "If");
        sequenceNode.AddNode(ifNode);

        ifNode.SetChildNode(CreateNode<BT_CheckAngle>(ifNode, "CheckAngle"));

        BT_IfNode childIfNode = CreateNode<BT_IfNode>(ifNode, "If");
        ifNode.SetSuccessNode(childIfNode);
        BT_RotationToPlayer rotationToPlayer = CreateNode<BT_RotationToPlayer>(ifNode, "RotationToPlayer");
        childIfNode.SetChildNode(CreateNode<BT_CheckNormalAttackCooltime>(childIfNode, "CheckNormalAttackCooltime"));
        childIfNode.SetSuccessNode(CreateNode<BT_NormalAttack>(childIfNode, "NormalAttack"));
        childIfNode.SetFailureNode(rotationToPlayer);
        ifNode.SetFailureNode(rotationToPlayer);
    }
    void CreateReturnPreset()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Return logic)");
        node.AddNode(sequenceNode);
        sequenceNode.AddNode(CreateNode<BT_CheckReturn>(sequenceNode, "CheckReturn"));
        sequenceNode.AddNode(CreateNode<BT_ReturnSpawnPoint>(sequenceNode, "ReturnSpawnPoint"));
    }
    void CreateTrackingPreset()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Tracking logic)");
        node.AddNode(sequenceNode);
        sequenceNode.AddNode(CreateNode<BT_CheckTrackingPlayer>(sequenceNode, "CheckTrackingPlayer"));
        sequenceNode.AddNode(CreateNode<BT_TrackingMovement>(sequenceNode, "TrackingMovement"));
    }
    void CreateIdlePreset()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        BT_SelectorNode selectorNode = CreateNode<BT_SelectorNode>(node, "Selector(Idle logic)");
        node.AddNode(selectorNode);
        selectorNode.AddNode(CreateNode<BT_Idle>(selectorNode, "Idle"));
        selectorNode.AddNode(CreateNode<BT_IdleMovement>(selectorNode, "IdleMovement"));
        selectorNode.AddNode(CreateNode<BT_ChangeIdlePosition>(selectorNode, "ChangeIdlePosition"));
    }

    void SetChildNodes()
    {
        BT_CompositeNode node = (BT_CompositeNode)target;
        node.ResetNode();
        for (int i = 0; i < node.transform.childCount; i++)
        {
            Transform child = node.transform.GetChild(i);
            BT_Node childNode = child.GetComponent<BT_Node>();
            node.AddNode(childNode);
        }
    }
}
#endif