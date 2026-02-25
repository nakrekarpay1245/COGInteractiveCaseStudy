using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using _Game.LevelSystem;

namespace _Game.Editor
{
    [InitializeOnLoad]
    public static class LevelSelectorToolbar
    {
        private static int _selectedLevelIndex = 0;

        static LevelSelectorToolbar()
        {
            UnityToolbarExtender.ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            LevelManager levelManager = GetLevelManager();
            if (levelManager == null || levelManager.LevelList == null || levelManager.LevelList.Count == 0)
            {
                return;
            }

            List<LevelDataSO> levels = levelManager.LevelList;
            string[] levelNames = new string[levels.Count];
            
            for (int i = 0; i < levels.Count; i++)
            {
                levelNames[i] = levels[i] != null ? levels[i].name : $"Level {i}";
            }

            GUILayout.Label("Level:", GUILayout.Width(40));
            
            int newIndex = EditorGUILayout.Popup(_selectedLevelIndex, levelNames, GUILayout.Width(150));
            
            if (newIndex != _selectedLevelIndex)
            {
                _selectedLevelIndex = newIndex;
                LevelManager.EditorStartLevelIndex = _selectedLevelIndex;
            }
        }

        private static LevelManager GetLevelManager()
        {
            LevelManager levelManager = Object.FindObjectOfType<LevelManager>();
            return levelManager;
        }
    }
}
