using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using _Game.LevelSystem;

namespace _Game.Editor.LevelEditor
{
    public class LevelManagerWindow : EditorWindow
    {
        private const string LEVELS_PATH = "Assets/_Game/Resources/_Data/Levels";
        private const string LEVEL_INDEX_KEY = "LevelIndex";
        
        private List<LevelDataSO> _levels = new List<LevelDataSO>();
        private Vector2 _scrollPosition;
        private Vector2 _editorScrollPosition;
        private LevelDataSO _selectedLevel;
        private UnityEditor.Editor _levelEditor;
        private bool _showSolvabilityResult = false;
        private bool _isSolvable = false;
        
        [MenuItem("Window/Level Manager")]
        public static void ShowWindow()
        {
            LevelManagerWindow window = GetWindow<LevelManagerWindow>("Level Manager");
            window.minSize = new Vector2(800, 600);
        }
        
        private void OnEnable()
        {
            LoadAllLevels();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            DrawLeftPanel();
            DrawRightPanel();
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(250));
            
            EditorGUILayout.LabelField("Level List", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            if (GUILayout.Button("Refresh Levels", GUILayout.Height(30)))
            {
                LoadAllLevels();
            }
            
            if (GUILayout.Button("Create New Level", GUILayout.Height(30)))
            {
                CreateNewLevel();
            }
            
            if (GUILayout.Button("Reset to Default", GUILayout.Height(30)))
            {
                if (_selectedLevel != null)
                {
                    if (EditorUtility.DisplayDialog("Reset to Default", "This will reset the selected level to LevelData_Base configuration. Continue?", "Yes", "No"))
                    {
                        ResetSelectedLevelToDefault();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("No Level Selected", "Please select a level first.", "OK");
                }
            }
            
            if (GUILayout.Button("Check Solvability", GUILayout.Height(30)))
            {
                if (_selectedLevel != null)
                {
                    _showSolvabilityResult = true;
                    _isSolvable = CheckSolvability(_selectedLevel);
                }
                else
                {
                    EditorUtility.DisplayDialog("No Level Selected", "Please select a level first.", "OK");
                }
            }
            
            EditorGUILayout.Space(10);
            
            int currentLevelIndex = PlayerPrefs.GetInt(LEVEL_INDEX_KEY, 0);
            EditorGUILayout.LabelField($"Current Level Index: {currentLevelIndex}", EditorStyles.helpBox);
            
            if (GUILayout.Button("Clear PlayerPrefs", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("Clear PlayerPrefs", "Are you sure you want to clear all PlayerPrefs?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();
                    Debug.Log("PlayerPrefs cleared");
                }
            }
            
            EditorGUILayout.Space(10);
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_levels[i] == null) continue;
                
                EditorGUILayout.BeginHorizontal("box");
                
                bool isSelected = _selectedLevel == _levels[i];
                Color originalColor = GUI.backgroundColor;
                if (isSelected) GUI.backgroundColor = Color.cyan;
                
                if (GUILayout.Button(_levels[i].name, GUILayout.Height(25)))
                {
                    SelectLevel(_levels[i]);
                }
                
                GUI.backgroundColor = originalColor;
                
                if (GUILayout.Button("D", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    DuplicateLevel(_levels[i]);
                }
                
                if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    DeleteLevel(_levels[i]);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }
        
        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical();
            
            if (_selectedLevel != null)
            {
                EditorGUILayout.LabelField($"Editing: {_selectedLevel.name}", EditorStyles.boldLabel);
                EditorGUILayout.Space(10);
                
                if (_showSolvabilityResult)
                {
                    Color resultColor = _isSolvable ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.8f, 0.2f, 0.2f);
                    string icon = _isSolvable ? "✔" : "×";
                    
                    GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                    boxStyle.fontSize = 16;
                    boxStyle.fontStyle = FontStyle.Bold;
                    boxStyle.alignment = TextAnchor.MiddleCenter;
                    boxStyle.normal.textColor = Color.white;
                    
                    GUI.backgroundColor = resultColor;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Box($"{icon}  {(_isSolvable ? "SOLVABLE" : "NOT SOLVABLE")}  {icon}", boxStyle, GUILayout.Height(40), GUILayout.MinWidth(300));
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space(10);
                }
                
                _editorScrollPosition = EditorGUILayout.BeginScrollView(_editorScrollPosition);
                
                if (_levelEditor == null || _levelEditor.target != _selectedLevel)
                {
                    _levelEditor = UnityEditor.Editor.CreateEditor(_selectedLevel);
                }
                
                if (_levelEditor != null)
                {
                    _levelEditor.OnInspectorGUI();
                }
                
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Select a level to edit", EditorStyles.centeredGreyMiniLabel);
            }
            
            EditorGUILayout.EndVertical();
        }
        
        private void LoadAllLevels()
        {
            _levels.Clear();
            
            if (!Directory.Exists(LEVELS_PATH))
            {
                Directory.CreateDirectory(LEVELS_PATH);
                AssetDatabase.Refresh();
            }
            
            string[] guids = AssetDatabase.FindAssets("t:LevelDataSO", new[] { LEVELS_PATH });
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LevelDataSO level = AssetDatabase.LoadAssetAtPath<LevelDataSO>(path);
                if (level != null)
                {
                    _levels.Add(level);
                }
            }
            
            _levels.Sort((a, b) => string.Compare(a.name, b.name));
        }
        
        private void SelectLevel(LevelDataSO level)
        {
            _selectedLevel = level;
            if (_levelEditor != null)
            {
                DestroyImmediate(_levelEditor);
            }
            _levelEditor = UnityEditor.Editor.CreateEditor(_selectedLevel);
        }
        
        private void CreateNewLevel()
        {
            int levelNumber = _levels.Count + 1;
            string fileName = $"LevelData_{levelNumber:D2}.asset";
            string path = Path.Combine(LEVELS_PATH, fileName);
            
            while (File.Exists(path))
            {
                levelNumber++;
                fileName = $"LevelData_{levelNumber:D2}.asset";
                path = Path.Combine(LEVELS_PATH, fileName);
            }
            
            LevelDataSO newLevel = CreateInstance<LevelDataSO>();
            AssetDatabase.CreateAsset(newLevel, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            LoadAllLevels();
            SelectLevel(newLevel);
            
            Debug.Log($"Created new level: {fileName}");
        }
        
        private void DuplicateLevel(LevelDataSO level)
        {
            string originalPath = AssetDatabase.GetAssetPath(level);
            string directory = Path.GetDirectoryName(originalPath);
            string fileName = Path.GetFileNameWithoutExtension(originalPath);
            string extension = Path.GetExtension(originalPath);
            
            int copyNumber = 1;
            string newPath = Path.Combine(directory, $"{fileName}_Copy{extension}");
            
            while (File.Exists(newPath))
            {
                copyNumber++;
                newPath = Path.Combine(directory, $"{fileName}_Copy{copyNumber}{extension}");
            }
            
            AssetDatabase.CopyAsset(originalPath, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            LoadAllLevels();
            
            Debug.Log($"Duplicated level: {Path.GetFileName(newPath)}");
        }
        
        private void DeleteLevel(LevelDataSO level)
        {
            if (EditorUtility.DisplayDialog("Delete Level", $"Are you sure you want to delete {level.name}?", "Yes", "No"))
            {
                string path = AssetDatabase.GetAssetPath(level);
                
                if (_selectedLevel == level)
                {
                    _selectedLevel = null;
                    if (_levelEditor != null)
                    {
                        DestroyImmediate(_levelEditor);
                        _levelEditor = null;
                    }
                }
                
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                LoadAllLevels();
                
                Debug.Log($"Deleted level: {level.name}");
            }
        }
        
        private void ResetSelectedLevelToDefault()
        {
            LevelDataSO baseLevel = Resources.Load<LevelDataSO>("_Data/Levels/LevelData_Base");
            if (baseLevel == null)
            {
                Debug.LogWarning("LevelData_Base not found in Resources/_Data/Levels/");
                return;
            }
            
            SerializedObject serializedLevel = new SerializedObject(_selectedLevel);
            
            serializedLevel.FindProperty("_gridSize").vector2IntValue = baseLevel.GridSize;
            serializedLevel.FindProperty("_ballSpeed").floatValue = baseLevel.BallSpeed;
            serializedLevel.FindProperty("_levelColor").enumValueIndex = (int)baseLevel.LevelColor;
            serializedLevel.FindProperty("_cameraOrthoSize").floatValue = baseLevel.CameraOrthoSize;
            serializedLevel.FindProperty("_floorSize").vector2Value = baseLevel.FloorSize;
            
            SerializedProperty ballPositionsProp = serializedLevel.FindProperty("_ballPositions");
            ballPositionsProp.ClearArray();
            if (baseLevel.BallPositions != null)
            {
                foreach (Vector2Int pos in baseLevel.BallPositions)
                {
                    ballPositionsProp.arraySize++;
                    ballPositionsProp.GetArrayElementAtIndex(ballPositionsProp.arraySize - 1).vector2IntValue = pos;
                }
            }
            
            SerializedProperty obstaclePositionsProp = serializedLevel.FindProperty("_obstaclePositions");
            obstaclePositionsProp.ClearArray();
            if (baseLevel.ObstaclePositions != null)
            {
                foreach (Vector2Int pos in baseLevel.ObstaclePositions)
                {
                    obstaclePositionsProp.arraySize++;
                    obstaclePositionsProp.GetArrayElementAtIndex(obstaclePositionsProp.arraySize - 1).vector2IntValue = pos;
                }
            }
            
            serializedLevel.ApplyModifiedProperties();
            EditorUtility.SetDirty(_selectedLevel);
            AssetDatabase.SaveAssets();
            
            if (_levelEditor != null)
            {
                DestroyImmediate(_levelEditor);
                _levelEditor = UnityEditor.Editor.CreateEditor(_selectedLevel);
            }
            
            Debug.Log($"Reset {_selectedLevel.name} to default configuration");
        }
        
        private void OnDestroy()
        {
            if (_levelEditor != null)
            {
                DestroyImmediate(_levelEditor);
            }
        }
        
        private bool CheckSolvability(LevelDataSO level)
        {
            Vector2Int gridSize = level.GridSize;
            HashSet<Vector2Int> obstacles = new HashSet<Vector2Int>(level.ObstaclePositions ?? new List<Vector2Int>());
            HashSet<Vector2Int> balls = new HashSet<Vector2Int>(level.BallPositions ?? new List<Vector2Int>());
            HashSet<Vector2Int> paintedTiles = new HashSet<Vector2Int>();
            
            int totalTiles = gridSize.x * gridSize.y - obstacles.Count - balls.Count;
            
            foreach (Vector2Int ballPos in balls)
            {
                SimulateBallMovement(ballPos, gridSize, obstacles, balls, paintedTiles);
            }
            
            return paintedTiles.Count == totalTiles;
        }
        
        private void SimulateBallMovement(Vector2Int startPos, Vector2Int gridSize, HashSet<Vector2Int> obstacles, HashSet<Vector2Int> balls, HashSet<Vector2Int> paintedTiles)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            queue.Enqueue(startPos);
            visited.Add(startPos);
            
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            
            while (queue.Count > 0)
            {
                Vector2Int currentPos = queue.Dequeue();
                
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int checkPos = currentPos + dir;
                    
                    if (!IsValidPosition(checkPos, gridSize) || obstacles.Contains(checkPos) || balls.Contains(checkPos))
                        continue;
                    
                    Vector2Int nextPos = checkPos;
                    paintedTiles.Add(nextPos);
                    
                    while (true)
                    {
                        Vector2Int furtherPos = nextPos + dir;
                        if (!IsValidPosition(furtherPos, gridSize) || obstacles.Contains(furtherPos) || balls.Contains(furtherPos))
                            break;
                        
                        nextPos = furtherPos;
                        paintedTiles.Add(nextPos);
                    }
                    
                    if (!visited.Contains(nextPos))
                    {
                        visited.Add(nextPos);
                        queue.Enqueue(nextPos);
                    }
                }
            }
        }
        
        private bool IsValidPosition(Vector2Int pos, Vector2Int gridSize)
        {
            return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
        }
    }
}
