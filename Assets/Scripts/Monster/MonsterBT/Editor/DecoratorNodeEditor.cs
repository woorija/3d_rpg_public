using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(BT_DecoratorNode), true)]
public class DecoratorNodeEditor : BaseNodeEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnInspectorGUIBase();
        OnInspectorGUINode();

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
        BT_DecoratorNode node = (BT_DecoratorNode)target;
        node.ResetNode();
        for (int i = node.transform.childCount - 2; i >= 0; i--)
        {
            DestroyImmediate(node.transform.GetChild(i).gameObject);
        }
        Transform child = node.transform.GetChild(0);
        BT_Node childNode = child.GetComponent<BT_Node>();
        node.SetChildNode(childNode);
    }
}
#endif