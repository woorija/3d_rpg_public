using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartSceneEditor
{
    private const string previousScenePath = "PreviousScene";

    static StartSceneEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScene = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString(previousScenePath, currentScene);
            EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            string prevuoisScene = EditorPrefs.GetString(previousScenePath, "");
            if (!string.IsNullOrEmpty(prevuoisScene))
            {
                EditorSceneManager.OpenScene(prevuoisScene);
            }
        }
    }
}