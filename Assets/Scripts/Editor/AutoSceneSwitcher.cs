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