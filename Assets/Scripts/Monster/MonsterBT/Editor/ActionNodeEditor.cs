using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

#if UNITY_EDITOR
[CustomEditor(typeof(BT_ActionNode), true)]
public class ActionNodeEditor : Editor
{
    Type[] actionNodeTypes;
    string[] actionNodeNames;
    int selectedIndex = -1;
    private void OnEnable()
    {
        actionNodeTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(BT_ActionNode)) && !type.IsAbstract)
            .ToArray();

        actionNodeNames = actionNodeTypes.Select(type => type.Name).ToArray();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BT_ActionNode node = (BT_ActionNode)target;

        CustomEditorDrawer.DrawLine();
        selectedIndex = EditorGUILayout.Popup("Change Node", selectedIndex, actionNodeNames);
        CustomEditorDrawer.DrawButton("Apply", () => ApplySelection());
    }

    void ApplySelection()
    {
        if (selectedIndex == -1) return;
        BT_ActionNode node = (BT_ActionNode)target;
        Type selectedType = actionNodeTypes[selectedIndex];
        if(selectedType != node.GetType())
        {
            GameObject gameObject = node.gameObject;
            DestroyImmediate(node);
            gameObject.AddComponent(selectedType);
            gameObject.name = selectedType.Name;
        }
    }
}
#endif
