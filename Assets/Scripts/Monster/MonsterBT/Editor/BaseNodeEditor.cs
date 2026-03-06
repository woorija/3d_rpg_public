#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class BaseNodeEditor : Editor
{
    protected int selectedPresetIndex = 0;
    protected int selectedNodeIndex = 0;
    protected enum PresetList
    {
        Die = 1,
        Stagger,
        NormalAttack,
        Return,
        Tracking,
        Idle
    }
    protected enum NodeList
    {
        Selector = 1,
        RandomSelector,
        Sequence,
        Inverter,
        Succeeder,
        Failer,
        Action,
        Condition
    }
    protected void ResetChildNode()
    {
        BT_Node node = (BT_Node)target;
        node.ResetNode();
        for (int i = node.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(node.transform.GetChild(i).gameObject);
        }
    }
    protected T CreateNode<T>(BT_Node _node, string _childNodeName) where T : BT_Node
    {
        GameObject childObject = new GameObject(_childNodeName);
        childObject.transform.SetParent(_node.transform);
        return childObject.AddComponent<T>();
    }
    protected virtual void SetUpNode()
    {
        
    }
    protected void CreateDiePreset()
    {
        BT_Node node = (BT_Node)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Die logic)");

        sequenceNode.AddNode(CreateNode<BT_CheckDie>(sequenceNode, "BT_CheckDie"));
        sequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(sequenceNode, "BT_ChangeAnimation"));
        sequenceNode.AddNode(CreateNode<BT_Die>(sequenceNode, "BT_Die"));
        sequenceNode.AddNode(CreateNode<BT_Respawn>(sequenceNode, "BT_Respawn"));
    }
    protected void CreateStaggerPreset()
    {
        BT_Node node = (BT_Node)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Stagger logic)");

        sequenceNode.AddNode(CreateNode<BT_CheckStagger>(sequenceNode, "BT_CheckStagger"));
        sequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(sequenceNode, "BT_ChangeAnimation"));
        sequenceNode.AddNode(CreateNode<BT_Stagger>(sequenceNode, "BT_Stagger"));
    }
    protected void CreateNormalAttackPreset()
    {
        BT_Node node = (BT_Node)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(NormalAttack logic)");

        sequenceNode.AddNode(CreateNode<BT_CheckDistance>(sequenceNode, "BT_CheckDistance"));
        BT_SelectorNode selectorNode = CreateNode<BT_SelectorNode>(sequenceNode, "Selector");
        sequenceNode.AddNode(selectorNode);

        BT_SequenceNode canAttackSequence = CreateNode<BT_SequenceNode>(selectorNode, "Sequence(CanAttack)");
        BT_SequenceNode cantAttackSequence = CreateNode<BT_SequenceNode>(selectorNode, "Sequence(Can'tAttack)");
        selectorNode.AddNode(canAttackSequence);
        selectorNode.AddNode(cantAttackSequence);

        canAttackSequence.AddNode(CreateNode<BT_CheckHeightDifference>(canAttackSequence, "BT_CheckHeightDifference"));
        canAttackSequence.AddNode(CreateNode<BT_CheckAngle>(canAttackSequence, "BT_CheckAngle"));
        canAttackSequence.AddNode(CreateNode<BT_CheckSkillCooltime>(canAttackSequence, "BT_CheckSkillCooltime"));
        canAttackSequence.AddNode(CreateNode<BT_ChangeAnimation>(canAttackSequence, "BT_ChangeAnimation"));
        canAttackSequence.AddNode(CreateNode<BT_ResetSkillCooltime>(canAttackSequence, "BT_ResetSkillCooltime"));
        canAttackSequence.AddNode(CreateNode<BT_PlayAnimationUntilEnd>(canAttackSequence, "BT_PlayAnimationUntilEnd"));

        cantAttackSequence.AddNode(CreateNode<BT_ChangeAnimation>(cantAttackSequence, "BT_ChangeAnimation"));
        cantAttackSequence.AddNode(CreateNode<BT_RotationToPlayer>(cantAttackSequence, "BT_RotationToPlayer"));
    }
    protected void CreateReturnPreset() 
    {
        BT_Node node = (BT_Node)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Return logic)");

        sequenceNode.AddNode(CreateNode<BT_CheckReturn>(sequenceNode, "BT_CheckReturn"));
        sequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(sequenceNode, "BT_ChangeAnimation"));
        sequenceNode.AddNode(CreateNode<BT_ReturnSpawnPoint>(sequenceNode, "BT_ReturnSpawnPoint"));
    }
    protected void CreateTrackingPreset()
    {
        BT_Node node = (BT_Node)target;
        BT_SequenceNode sequenceNode = CreateNode<BT_SequenceNode>(node, "Sequence(Tracking logic)");

        BT_InverterNode inverterNode = CreateNode<BT_InverterNode>(sequenceNode, "BT_InverterNode");
        inverterNode.SetChildNode(CreateNode<BT_CheckTrackingLimitRange>(inverterNode, "BT_CheckTrackingLimitRange"));

        sequenceNode.AddNode(CreateNode<BT_CheckDistance>(sequenceNode, "BT_CheckDistance"));
        sequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(sequenceNode, "BT_ChangeAnimation"));
        sequenceNode.AddNode(CreateNode<BT_TrackingMovement>(sequenceNode, "BT_TrackingMovement"));
    }
    protected void CreateIdlePreset()
    {
        BT_Node node = (BT_Node)target;
        BT_SelectorNode selectorNode = CreateNode<BT_SelectorNode>(node, "Selector(Idle logic)");

        BT_SequenceNode idleAnimationSequenceNode = CreateNode<BT_SequenceNode>(selectorNode, "Sequence(idle Animation)");
        idleAnimationSequenceNode.AddNode(CreateNode<BT_Idle>(idleAnimationSequenceNode, "BT_Idle"));
        idleAnimationSequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(idleAnimationSequenceNode, "BT_ChangeAnimation"));

        BT_SequenceNode moveAnimationSequenceNode = CreateNode<BT_SequenceNode>(selectorNode, "Sequence(move Animation)");
        moveAnimationSequenceNode.AddNode(CreateNode<BT_ChangeAnimation>(moveAnimationSequenceNode, "BT_ChangeAnimation"));
        moveAnimationSequenceNode.AddNode(CreateNode<BT_IdleMovement>(moveAnimationSequenceNode, "BT_IdleMovement"));

        selectorNode.AddNode(idleAnimationSequenceNode);
        selectorNode.AddNode(moveAnimationSequenceNode);
        selectorNode.AddNode(CreateNode<BT_ChangeIdlePosition>(selectorNode, "BT_ChangeIdlePosition"));
    }
    protected void ApplyPreset()
    {
        switch (selectedPresetIndex)
        {
            case 1:
                CreateDiePreset();
                break;
            case 2:
                CreateStaggerPreset();
                break;
            case 3:
                CreateNormalAttackPreset();
                break;
            case 4:
                CreateReturnPreset();
                break;
            case 5:
                CreateTrackingPreset();
                break;
            case 6:
                CreateIdlePreset();
                break;
            default:
                break;
        }
        SetUpNode();
    }
    protected void ApplyNode()
    {
        BT_Node node = (BT_Node)target;
        switch (selectedNodeIndex)
        {
            case 1:
                CreateNode<BT_SelectorNode>(node, "Selector");
                break;
            case 2:
                CreateNode<BT_RandomSelectorNode>(node, "RandomSelector");
                break;
            case 3:
                CreateNode<BT_SequenceNode>(node, "Sequence");
                break;
            case 4:
                CreateNode<BT_InverterNode>(node, "BT_InverterNode");
                break;
            case 5:
                CreateNode<BT_SucceederNode>(node, "BT_SucceederNode");
                break;
            case 6:
                CreateNode<BT_FailerNode>(node, "BT_FailerNode");
                break;
            case 7:
                CreateNode<BT_ActionNode>(node, "BT_ActionNode");
                break;
            case 8:
                CreateNode<BT_ConditionNode>(node, "BT_ConditionNode");
                break;
        }
        SetUpNode();
    }
    protected void OnInspectorGUIBase()
    {
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawCenteredText("Node Editor");
    }
    protected void OnInspectorGUINode()
    {
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawCenteredText("추가할 노드 선택");

        EditorGUILayout.BeginHorizontal();
        CustomEditorDrawer.DrawButtonStyleToggle("Selector", null, (int)NodeList.Selector, ref selectedNodeIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("RandomSelector", null, (int)NodeList.RandomSelector, ref selectedNodeIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("Sequence", null, (int)NodeList.Sequence, ref selectedNodeIndex);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        CustomEditorDrawer.DrawButtonStyleToggle("Inverter", null, (int)NodeList.Inverter, ref selectedNodeIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("Succeeder", null, (int)NodeList.Succeeder, ref selectedNodeIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("Failer", null, (int)NodeList.Failer, ref selectedNodeIndex);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        CustomEditorDrawer.DrawButtonStyleToggle("Action", null, (int)NodeList.Action, ref selectedNodeIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("Condition", null, (int)NodeList.Condition, ref selectedNodeIndex);
        EditorGUILayout.EndHorizontal();

        CustomEditorDrawer.DrawButton("Apply Selection", () => ApplyNode());
    }
    protected void OnInspectorGUIPreset()
    {
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawCenteredText("추가할 프리셋 선택");

        EditorGUILayout.BeginHorizontal();
        CustomEditorDrawer.DrawButtonStyleToggle("사망 프리셋", null, (int)PresetList.Die, ref selectedPresetIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("경직 프리셋", null, (int)PresetList.Stagger, ref selectedPresetIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("공격 프리셋", null, (int)PresetList.NormalAttack, ref selectedPresetIndex);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        CustomEditorDrawer.DrawButtonStyleToggle("리턴 프리셋", null, (int)PresetList.Return, ref selectedPresetIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("추적 프리셋", null, (int)PresetList.Tracking, ref selectedPresetIndex);
        CustomEditorDrawer.DrawButtonStyleToggle("대기 프리셋", null, (int)PresetList.Idle, ref selectedPresetIndex);
        EditorGUILayout.EndHorizontal();

        CustomEditorDrawer.DrawButton("Apply Selection", () => ApplyPreset());
    }
}
#endif