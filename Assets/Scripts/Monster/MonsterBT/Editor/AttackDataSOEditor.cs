using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackDataSO))]
public class AttackDataSOEditor : Editor
{
    SerializedProperty attackName;
    SerializedProperty attackType;

    SerializedProperty pos;
    SerializedProperty yLower;
    SerializedProperty yUpper;

    SerializedProperty outerRadius;
    SerializedProperty innerRadius;

    SerializedProperty angles;

    SerializedProperty left;
    SerializedProperty right;
    SerializedProperty front;
    SerializedProperty back;

    SerializedProperty cooltime;
    SerializedProperty damage;
    SerializedProperty percentDamage;

    private void OnEnable()
    {
        attackName = serializedObject.FindProperty("attackName");
        attackType = serializedObject.FindProperty("attackType");

        pos = serializedObject.FindProperty("pos");
        yLower = serializedObject.FindProperty("yLower");
        yUpper = serializedObject.FindProperty("yUpper");

        outerRadius = serializedObject.FindProperty("outerRadius");
        innerRadius = serializedObject.FindProperty("innerRadius");
        angles = serializedObject.FindProperty("angles");

        left = serializedObject.FindProperty("left");
        right = serializedObject.FindProperty("right");
        front = serializedObject.FindProperty("front");
        back = serializedObject.FindProperty("back");

        cooltime = serializedObject.FindProperty("cooltime");
        damage = serializedObject.FindProperty("damage");
        percentDamage = serializedObject.FindProperty("percentDamage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(attackName);
        EditorGUILayout.PropertyField(attackType);

        AttackType type = (AttackType)attackType.enumValueIndex;

        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawCenteredText("공격 범위");
        CustomEditorDrawer.DrawLine();
        EditorGUILayout.PropertyField(pos);
        switch (type)
        {
            case AttackType.Circle:
                EditorGUILayout.PropertyField(outerRadius);
                EditorGUILayout.PropertyField(innerRadius);
                angles.ClearArray();
                break;
            case AttackType.Sector:
                EditorGUILayout.PropertyField(outerRadius);
                EditorGUILayout.PropertyField(innerRadius);
                EditorGUILayout.PropertyField(angles);
                break;
            case AttackType.Box:
                EditorGUILayout.PropertyField(left);
                EditorGUILayout.PropertyField(right);
                EditorGUILayout.PropertyField(front);
                EditorGUILayout.PropertyField(back);
                angles.ClearArray();
                break;
        }
        EditorGUILayout.PropertyField(yLower);
        EditorGUILayout.PropertyField(yUpper);

        EditorGUILayout.Space();
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawCenteredText("그 외 정보");
        CustomEditorDrawer.DrawLine();
        EditorGUILayout.PropertyField(cooltime);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(percentDamage);

        serializedObject.ApplyModifiedProperties();
    }
}
