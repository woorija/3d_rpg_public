using UnityEngine;
using UnityEditor;
using System;

public class CustomEditorDrawer : Editor
{
    public static void DrawCenteredText(string _text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(_text,EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    public static void DrawText(string _text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(_text, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
    }
    public static void DrawLine()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    public static void DrawButton(string _text, Action _action)
    {
        if (GUILayout.Button(_text))
        {
            _action?.Invoke();
        }
    }
    public static void DrawLabelField(string _text)
    {
        EditorGUILayout.LabelField(_text);
    }

    public static void DrawButtonStyleToggle(string _text, Action _action, int _index, ref int _value)
    {
        bool isSelected = _index == _value;
        bool newSelected = GUILayout.Toggle(isSelected, _text, GUI.skin.button);

        if(newSelected != isSelected)
        {
            _value = _index;
            _action?.Invoke();
        }
    }
}
