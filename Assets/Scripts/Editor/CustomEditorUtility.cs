#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class CustomEditorUtility : Editor
{
    public static List<T> FindAllComponentsInCurrentScene<T>() where T : Component
    {
        var rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        var result = new List<T>();

        foreach (var root in rootObjects)
        {
            result.AddRange(root.GetComponentsInChildren<T>(true));
        }
        return result;
    }
}

#endif
