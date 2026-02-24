using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace _Game.Editor
{
    [InitializeOnLoad]
    public static class SceneSwitcherEditor
    {
        private static readonly Dictionary<int, string> scenePathByIndex = new Dictionary<int, string>();
        private static string[] sceneNames;

        static SceneSwitcherEditor()
        {
            CacheScenes();
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void CacheScenes()
        {
            scenePathByIndex.Clear();
            scenePathByIndex[0] = null; // Placeholder for label

            int sceneCount = SceneManager.sceneCountInBuildSettings;
            List<string> names = new List<string> { "Scene Switcher" };

            for (int i = 0; i < sceneCount; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                scenePathByIndex[i + 1] = path;
                names.Add(Path.GetFileNameWithoutExtension(path));
            }

            sceneNames = names.ToArray();
        }

        private static void OnToolbarGUI()
        {
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;

            EditorGUI.BeginChangeCheck();
            int selectedIndex = EditorGUILayout.Popup(0, sceneNames, GUILayout.Width(150));
            if (EditorGUI.EndChangeCheck() && selectedIndex > 0)
            {
                string selectedScenePath = scenePathByIndex[selectedIndex];

                if (!string.IsNullOrEmpty(selectedScenePath))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(selectedScenePath, OpenSceneMode.Single);
                    }
                }
            }

            GUI.enabled = true;
        }
    }
}