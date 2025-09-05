#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
public static class SceneAutoSetupUtility
{
    public static void SetAllNpcDataAllScenes()
    {
        string originalScenePath = EditorSceneManager.GetActiveScene().path;
        // 프로젝트 내 모든 씬 경로 찾기
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });

        foreach (string sceneGuid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            Debug.Log(scenePath);

            Scene currentScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            var npcDataComponents = CustomEditorUtility.FindAllComponentsInCurrentScene<NpcData>();

            foreach (NpcData npcData in npcDataComponents)
            {
                // NPCDataEditor의 SetData 메서드 실행
                NPCDataEditor editor = Editor.CreateEditor(npcData, typeof(NPCDataEditor)) as NPCDataEditor;
                if (editor != null)
                {
                    editor.GetType().GetMethod("SetData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(editor, null);
                    UnityEngine.Object.DestroyImmediate(editor);
                }
            }

            // 현재 씬 변경사항 저장
            EditorSceneManager.SaveScene(currentScene);
        }

        // 변경사항 저장 및 새로고침
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 원래 열려있던 씬 다시 로드
        EditorSceneManager.OpenScene(originalScenePath);

        Debug.Log("모든 씬의 NPC 데이터 세팅 완료");
    }
    public static void SetAllMonsterDataAllScenes()
    {
        string originalScenePath = EditorSceneManager.GetActiveScene().path;
        // 프로젝트 내 모든 씬 경로 찾기
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });

        foreach (string sceneGuid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            Debug.Log(scenePath);

            Scene currentScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            var blackboardDataComponents = CustomEditorUtility.FindAllComponentsInCurrentScene<BaseBlackBoard>();

            foreach (BaseBlackBoard blackboardData in blackboardDataComponents)
            {
                // 에디터 내에서만 실행되도록 확인
                if (EditorUtility.IsPersistent(blackboardData.gameObject)) continue;

                MonsterBlackBoardEditor editor = Editor.CreateEditor(blackboardData, typeof(MonsterBlackBoardEditor)) as MonsterBlackBoardEditor;
                if (editor != null)
                {
                    editor.GetType().GetMethod("SetUp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(editor, null);
                    UnityEngine.Object.DestroyImmediate(editor);
                }
                EditorSceneManager.SaveScene(currentScene);
            }
        }

        // 변경사항 저장 및 새로고침
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 원래 열려있던 씬 다시 로드
        EditorSceneManager.OpenScene(originalScenePath);

        Debug.Log("모든 씬의 몬스터 데이터 세팅 완료");
    }
}

#endif