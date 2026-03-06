#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(GetBlackBoardDataAttribute))]
public class GetBlackBoardDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (GetBlackBoardDataAttribute)attribute;

        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 4, position.width, EditorGUIUtility.singleLineHeight);

        var nodeMono = property.serializedObject.targetObject as MonoBehaviour;
        if (nodeMono == null)
        {
            EditorGUI.LabelField(dropdownRect, "Node Not Found");
            EditorGUI.PropertyField(valueRect, property, GUIContent.none);
            return;
        }

        var blackBoard = nodeMono.GetComponentInParent<BaseBlackBoard>();
        var dataSO = blackBoard != null ? blackBoard.blackBoardData : null;

        if (dataSO == null)
        {
            EditorGUI.LabelField(dropdownRect, "DataSO Not Found");
            EditorGUI.PropertyField(valueRect, property, GUIContent.none);
            return;
        }

        if (GUI.Button(dropdownRect, label.text, EditorStyles.popup))
        {
            ShowMenu(property, nodeMono, dataSO);
        }

        EditorGUI.PropertyField(valueRect, property, GUIContent.none);
    }

    private void ShowMenu(SerializedProperty property, MonoBehaviour nodeMono, MonsterBlackBoardSO dataSO)
    {
        GenericMenu menu = new GenericMenu();

        // ----------------------------
        // 1. Basic (BlackBoardDataSO)
        // ----------------------------
        var baseFields = dataSO.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => IsSupportedType(f.FieldType));

        foreach (var f in baseFields)
        {
            object value = f.GetValue(dataSO);

            menu.AddItem(
                new GUIContent($"Basic/{f.Name}"),
                false,
                () => AssignValue(property, nodeMono, value)
            );
        }

        // ----------------------------
        // 2. AttackData
        // ----------------------------
        var attackListField = dataSO.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<AttackDataSO>));

        if (attackListField != null)
        {
            var attackList = attackListField.GetValue(dataSO) as List<AttackDataSO>;
            if (attackList != null)
            {
                foreach (var attack in attackList)
                {
                    string attackRoot = attack.AttackName;

                    // ---- 기본 필드 ----
                    var attackFields = attack.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => IsSupportedType(f.FieldType));

                    foreach (var f in attackFields)
                    {
                        object value = f.GetValue(attack);

                        menu.AddItem(
                            new GUIContent($"{attackRoot}/{f.Name}"),
                            false,
                            () => AssignValue(property, nodeMono, value)
                        );
                    }

                    // ---- AngleRange ----
                    var angleField = attack.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .FirstOrDefault(f => f.FieldType == typeof(List<AngleRange>));

                    if (angleField != null)
                    {
                        var angles = angleField.GetValue(attack) as List<AngleRange>;
                        if (angles != null)
                        {
                            for (int i = 0; i < angles.Count; i++)
                            {
                                int index = i;

                                menu.AddItem(
                                    new GUIContent($"{attackRoot}/AngleRange[{i}]/Min"),
                                    false,
                                    () => AssignValue(property, nodeMono, angles[index].minAngle)
                                );

                                menu.AddItem(
                                    new GUIContent($"{attackRoot}/AngleRange[{i}]/Max"),
                                    false,
                                    () => AssignValue(property, nodeMono, angles[index].maxAngle)
                                );
                            }
                        }
                    }
                }
            }
        }

        menu.ShowAsContext();
    }

    // ==================================================
    // Assign
    // ==================================================
    private void AssignValue(SerializedProperty property, MonoBehaviour nodeMono, object value)
    {
        if (value == null) return;

        Undo.RecordObject(nodeMono, "Assign Blackboard Value");
        SetPropertyValue(property, value);
        property.serializedObject.ApplyModifiedProperties();
    }

    // ===== Utility =====

    private bool IsSupportedType(System.Type t)
    {
        return t == typeof(float) || t == typeof(int) || t == typeof(bool) || t == typeof(string);
    }

    private object GetPropertyValue(SerializedProperty prop)
    {
        return prop.propertyType switch
        {
            SerializedPropertyType.Float => prop.floatValue,
            SerializedPropertyType.Integer => prop.intValue,
            SerializedPropertyType.Boolean => prop.boolValue,
            //SerializedPropertyType.String => prop.stringValue,
            _ => null
        };
    }

    private void SetPropertyValue(SerializedProperty prop, object value)
    {
        if (value == null) return;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Float:
                prop.floatValue = (float)value; 
                break;
            case SerializedPropertyType.Integer:
                prop.intValue = (int)value;
                break;
            case SerializedPropertyType.Boolean:
                prop.boolValue = (bool)value; 
                break;
                /*
            case SerializedPropertyType.String:
                prop.stringValue = (string)value; 
                break;*/
        }
    }
}
#endif