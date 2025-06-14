#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class AutoSceneSwitcher
{
    static Scenes startScene = Scenes.TrailerScene;

    static AutoSceneSwitcher()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        string StartScenePath = "Assets/Scenes/" + startScene.ToString() + ".unity";
        string PrefsKey = "Exited_Scene_Path";

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // 확인 팝업
            bool shouldSwitch = EditorUtility.DisplayDialog("시작 씬 변경", $"\"{startScene}\"으로 씬 전환", "예", "아니오");
            if (!shouldSwitch)
                return;

            string exitingScenePath = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString(PrefsKey, exitingScenePath);
            if (exitingScenePath != StartScenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(StartScenePath);
                else
                    EditorApplication.isPlaying = false;
            }
        }
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            string exitedScenePath = EditorPrefs.GetString(PrefsKey);
            EditorSceneManager.OpenScene(exitedScenePath);
        }
    }
}
#endif