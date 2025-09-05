#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusCorrectionTableSO))]
public class StatusCorrectionTableSOEditor : Editor
{
    private SerializedProperty classNameProp;
    private SerializedProperty statusListProp;
    private SerializedProperty levelUpStatusProp;

    private readonly string[] levelUplabels = new string[]
    {
        "Strength",
        "Dexterity",
        "Vitality",
        "Intelligence",
        "Wisdom",
        "Agility",
        "Remaining",
        "HP",
        "MP"
    };

    private void OnEnable()
    {
        classNameProp = serializedObject.FindProperty("className");
        levelUpStatusProp = serializedObject.FindProperty("levelUpStatus");
        statusListProp = serializedObject.FindProperty("statusList");
        StatusCorrectionTableSO so = (StatusCorrectionTableSO)target;
        so.EditorInit();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(classNameProp);
        EditorGUILayout.LabelField("Level Up Status List", EditorStyles.boldLabel);
        for (int i=0; i< levelUpStatusProp.arraySize; i++)
        {
            EditorGUILayout.PropertyField(levelUpStatusProp.GetArrayElementAtIndex(i),new GUIContent(levelUplabels[i]));
        }

        EditorGUILayout.LabelField("Correction Status List", EditorStyles.boldLabel);

        for (int i = 0; i < statusListProp.arraySize; i++)
        {
            EditorGUILayout.PropertyField(statusListProp.GetArrayElementAtIndex(i));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif