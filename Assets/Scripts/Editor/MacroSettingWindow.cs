#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class MacroSettingWindow : EditorWindow
{
    [MenuItem("Tools/MacroSetting")]
    public static void ShowWindow()
    {
        GetWindow<MacroSettingWindow>();
    }
    private void OnGUI()
    {
        GUILayout.Space(5);
        CustomEditorDrawer.DrawLabelField("csv 데이터 매크로");
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("대사 딕셔너리 세팅", () =>
        {
            TalkDataBaseGenerator generator = new TalkDataBaseGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawButton("상점 딕셔너리 세팅", () =>
        {
            ShopDataBaseGenerator generator = new ShopDataBaseGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawButton("NPC 이름 딕셔너리 세팅", () =>
        {
            NPCNameDataBaseGenerator generator = new NPCNameDataBaseGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawButton("몬스터 딕셔너리 세팅", () =>
        {
            MonsterDataBaseGenerator generator = new MonsterDataBaseGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawButton("몬스터 리워드 딕셔너리 세팅", () =>
        {
            MonsterRewardDBGenerator generator = new MonsterRewardDBGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawButton("몬스터 드랍테이블 딕셔너리 세팅", () =>
        {
            MonsterDropTableDataBaseGenerator generator = new MonsterDropTableDataBaseGenerator();
            generator.ReadCSV();
        });
        CustomEditorDrawer.DrawLine();
        GUILayout.Space(5);

        CustomEditorDrawer.DrawLabelField("스크립터블 오브젝트 데이터 매크로");
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("몬스터 블랙보드SO 세팅", ScriptableObjectAutoSetupUtility.SetAllMonsterSO);
        CustomEditorDrawer.DrawButton("몬스터 드랍테이블SO 세팅", ScriptableObjectAutoSetupUtility.SetAllMonsterDropTableSO);
        CustomEditorDrawer.DrawLine();
        GUILayout.Space(5);

        CustomEditorDrawer.DrawLabelField("씬 데이터 매크로");
        CustomEditorDrawer.DrawLine();
        CustomEditorDrawer.DrawButton("NPC 세팅", SceneAutoSetupUtility.SetAllNpcDataAllScenes);
        CustomEditorDrawer.DrawButton("몬스터 세팅", SceneAutoSetupUtility.SetAllMonsterDataAllScenes);
        CustomEditorDrawer.DrawButton("풀 매니저 세팅", () =>
        {
            PoolManagerAutoGenerator generator = new PoolManagerAutoGenerator();
            generator.GenerateClassCode();
        });
    }
}
#endif