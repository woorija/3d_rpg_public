using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(InterfacesAttribute))]
public class InterfacesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InterfacesAttribute attribute = (InterfacesAttribute)this.attribute;

        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            EditorGUI.LabelField(position, label.text, "Use [Interfaces] with ObjectReference fields.");
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        Object currentObject = property.objectReferenceValue;
        GameObject selectedGameObject = EditorGUI.ObjectField(position, label, currentObject, typeof(GameObject), true) as GameObject;

        if (selectedGameObject != null)
        {
            var validComponents = new List<MonoBehaviour>();
            foreach (var component in selectedGameObject.GetComponents<MonoBehaviour>())
            {
                if (attribute.InterfaceType.IsAssignableFrom(component.GetType()))
                {
                    validComponents.Add(component);
                }
            }

            if (validComponents.Count > 0)
            {
                ShowComponentSelectionMenu(property, validComponents);
            }
            else
            {
                Debug.LogWarning($"No components implementing {attribute.InterfaceType.Name} found in '{selectedGameObject.name}'.");
            }
        }

        EditorGUI.EndProperty();
    }

    private void ShowComponentSelectionMenu(SerializedProperty property, List<MonoBehaviour> components)
    {
        GenericMenu menu = new GenericMenu();

        // "All" 항목 추가
        menu.AddItem(new GUIContent("All"), false, () =>
        {
            AddAllComponentsToList(property, components);
        });

        foreach (var component in components)
        {
            menu.AddItem(new GUIContent(component.GetType().Name), false, () =>
            {
                property.objectReferenceValue = component;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

    private void AddAllComponentsToList(SerializedProperty property, List<MonoBehaviour> components)
    {
        SerializedObject serializedObject = property.serializedObject;

        foreach (var component in components)
        {
            // 배열 속성인지 확인하고 처리
            string rootArrayPath = GetRootArrayPath(property.propertyPath);
            SerializedProperty arrayProperty = serializedObject.FindProperty(rootArrayPath);

            if (arrayProperty != null && arrayProperty.isArray)
            {
                int newIndex = arrayProperty.arraySize;
                arrayProperty.InsertArrayElementAtIndex(newIndex);
                arrayProperty.GetArrayElementAtIndex(newIndex).objectReferenceValue = component;

                Debug.Log($"Added '{component.name}' to the list.");
            }
            else
            {
                Debug.LogError($"Could not find a valid array at path: {rootArrayPath}");
                break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
    private string GetRootArrayPath(string propertyPath)
    {
        int arrayIndex = propertyPath.IndexOf(".Array");
        if (arrayIndex >= 0)
        {
            return propertyPath.Substring(0, arrayIndex + 6); // '.Array'까지 포함
        }
        return propertyPath; // 배열이 아닌 경우 그대로 반환
    }
}