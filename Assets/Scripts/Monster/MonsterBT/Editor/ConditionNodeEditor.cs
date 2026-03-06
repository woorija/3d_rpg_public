#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(BT_ConditionNode), true)]
public class ConditionNodeEditor : Editor
{
    protected Type[] conditionNodeTypes;
    protected string[] conditionNodeNames;
    protected int selectedIndex = -1;
    protected virtual void OnEnable()
    {
        conditionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(BT_ConditionNode)) && !type.IsAbstract)
            .ToArray();

        conditionNodeNames = conditionNodeTypes.Select(type => type.Name).ToArray();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BT_ConditionNode node = (BT_ConditionNode)target;

        CustomEditorDrawer.DrawLine();
        selectedIndex = EditorGUILayout.Popup("Change Node", selectedIndex, conditionNodeNames);
        CustomEditorDrawer.DrawButton("Apply", () => ApplySelection());
    }

    void ApplySelection()
    {
        if (selectedIndex == -1) return;
        BT_ConditionNode node = (BT_ConditionNode)target;
        Type selectedType = conditionNodeTypes[selectedIndex];
        if (selectedType != node.GetType())
        {
            GameObject gameObject = node.gameObject;
            DestroyImmediate(node);
            gameObject.AddComponent(selectedType);
            gameObject.name = selectedType.Name;
        }
    }
}
#endif