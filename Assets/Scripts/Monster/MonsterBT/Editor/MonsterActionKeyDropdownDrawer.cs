using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MonsterActionKeyDropdownAttribute))]
public class MonsterActionKeyDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var constFields = typeof(MonsterActionKey)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .ToArray();

        var names = constFields.Select(f => f.GetRawConstantValue().ToString()).ToList();

        if (string.IsNullOrEmpty(property.stringValue) && names.Count > 0)
        {
            property.stringValue = names[0];
        }
        // 현재 선택된 값
        int index = Mathf.Max(0, names.IndexOf(property.stringValue));

        int newIndex = EditorGUI.Popup(position, label.text, index, names.ToArray());

        if (newIndex != index)
        {
            property.stringValue = names[newIndex];
        }
    }
}
