using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class StartSceneEditor : MonoBehaviour
{
    static StartSceneEditor()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);

        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}
