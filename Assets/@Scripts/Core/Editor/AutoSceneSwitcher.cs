#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AutoSceneSwitcher
{
    const string MENU_PATH = "Tools/Always Start From The First Scene";

    static bool IsUse
    {
        get => Menu.GetChecked(MENU_PATH);
        set => Menu.SetChecked(MENU_PATH, value);
    }

    static AutoSceneSwitcher()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem(MENU_PATH)]
    static void Toggle() => IsUse = !IsUse;

    [MenuItem(MENU_PATH, true)]
    static bool Validate()
    {
        Menu.SetChecked(MENU_PATH, IsUse);
        return true;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (!IsUse)
            return;

        const string PREFS_KEY = "Exited_Scene_Path";

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string StartScenePath = SceneUtility.GetScenePathByBuildIndex(0);

            string exitingScenePath = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString(PREFS_KEY, exitingScenePath);
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
            string exitedScenePath = EditorPrefs.GetString(PREFS_KEY);
            EditorSceneManager.OpenScene(exitedScenePath);
        }
    }
}
#endif