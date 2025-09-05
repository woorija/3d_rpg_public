#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(KeyRebindUI))]
public class KeyRebindUIEditor : Editor
{
    SerializedProperty actionMapNameProperty;
    SerializedProperty actionNameProperty;
    SerializedProperty bindingIndexProperty;

    SerializedProperty rebindingManagerProperty;
    SerializedProperty keyNameTextProperty;
    SerializedProperty rebindButtonProperty;
    SerializedProperty rebindNameTextProperty;
    SerializedProperty keyNameProperty;
    SerializedProperty isChangeableProperty;
    SerializedProperty keyRebindEventProperty;

    string[] mapNames;
    string[] actionNames;
    string[] bindingDisplayNames;

    int selectedMapIndex;
    int selectedActionIndex;
    int selectedBindingIndex;

    InputActionAsset inputActionAsset;
    private void OnEnable()
    {
        inputActionAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem/Controls.inputactions");

        rebindingManagerProperty = serializedObject.FindProperty("rebindingManager");
        actionMapNameProperty = serializedObject.FindProperty("actionMapName");
        actionNameProperty = serializedObject.FindProperty("actionName");
        bindingIndexProperty = serializedObject.FindProperty("bindingIndex");

        keyNameTextProperty = serializedObject.FindProperty("keyNameText");
        rebindButtonProperty = serializedObject.FindProperty("rebindButton");
        rebindNameTextProperty = serializedObject.FindProperty("rebindNameText");

        keyNameProperty = serializedObject.FindProperty("keyName");
        isChangeableProperty = serializedObject.FindProperty("isChangeable");
        keyRebindEventProperty = serializedObject.FindProperty("keyRebindEvent");

        RefreshActionMapList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(rebindingManagerProperty);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Action Info", EditorStyles.boldLabel);

        int newMapIndex = EditorGUILayout.Popup("Action Map", selectedMapIndex, mapNames);
        if (newMapIndex != selectedMapIndex)
        {
            selectedMapIndex = newMapIndex;
            actionMapNameProperty.stringValue = mapNames[selectedMapIndex];
            selectedActionIndex = 0;
            UpdateActionList();
        }

        // Action Dropdown
        int newActionIndex = EditorGUILayout.Popup("Action", selectedActionIndex, actionNames);
        if (newActionIndex != selectedActionIndex)
        {
            selectedActionIndex = newActionIndex;
            actionNameProperty.stringValue = actionNames[selectedActionIndex];
            selectedBindingIndex = 0;
            UpdateBindingList();
        }

        // Binding Dropdown
        int newBindingIndex = EditorGUILayout.Popup("Binding", selectedBindingIndex, bindingDisplayNames);
        if (newBindingIndex != selectedBindingIndex)
        {
            selectedBindingIndex = newBindingIndex;
            bindingIndexProperty.intValue = selectedBindingIndex;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(keyNameTextProperty);
        EditorGUILayout.PropertyField(rebindButtonProperty);
        EditorGUILayout.PropertyField(rebindNameTextProperty);

        EditorGUILayout.PropertyField(keyNameProperty);
        EditorGUILayout.PropertyField(isChangeableProperty);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(keyRebindEventProperty);

        serializedObject.ApplyModifiedProperties();
    }

    void RefreshActionMapList()
    {
        if (inputActionAsset == null)
        {
            mapNames = new string[0];
            return;
        }

        mapNames = inputActionAsset.actionMaps.Select(map => map.name).ToArray();
        selectedMapIndex = System.Array.IndexOf(mapNames, actionMapNameProperty.stringValue);
        if (selectedMapIndex < 0) selectedMapIndex = 0;

        UpdateActionList();
    }

    void UpdateActionList()
    {
        if (inputActionAsset == null || selectedMapIndex >= inputActionAsset.actionMaps.Count)
        {
            actionNames = new string[0];
            return;
        }

        var actionMap = inputActionAsset.actionMaps[selectedMapIndex];
        actionNames = actionMap.actions.Select(a => a.name).ToArray();

        selectedActionIndex = System.Array.IndexOf(actionNames, actionNameProperty.stringValue);
        if (selectedActionIndex < 0) selectedActionIndex = 0;

        UpdateBindingList();
    }

    void UpdateBindingList()
    {
        if (inputActionAsset == null) return;

        var map = inputActionAsset.actionMaps.FirstOrDefault(m => m.name == mapNames[selectedMapIndex]);
        var action = map?.actions.FirstOrDefault(a => a.name == actionNames[selectedActionIndex]);
        if (action == null)
        {
            bindingDisplayNames = new string[0];
            return;
        }

        var bindings = action.bindings;
        bindingDisplayNames = new string[bindings.Count];
        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];
            var displayName = action.GetBindingDisplayString(i);
            if (binding.isPartOfComposite)
                displayName = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayName}";
            bindingDisplayNames[i] = displayName;
        }

        selectedBindingIndex = Mathf.Clamp(bindingIndexProperty.intValue, 0, bindings.Count - 1);
    }
}
#endif