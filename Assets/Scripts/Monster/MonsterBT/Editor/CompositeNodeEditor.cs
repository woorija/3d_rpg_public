using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(BT_CompositeNode), true)]
public class CompositeNodeEditor : BaseNodeEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnInspectorGUIBase();
        OnInspectorGUINode();
        OnInspectorGUIPreset();

        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("SetUp", () => SetUpNode());

        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("Reset", () => ResetChildNode());

        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    protected override void SetUpNode()
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