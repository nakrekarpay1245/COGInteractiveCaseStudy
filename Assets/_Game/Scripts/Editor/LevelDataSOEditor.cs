using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using _Game.LevelSystem;

namespace _Game.Editor.LevelEditor
{
    [CustomEditor(typeof(LevelDataSO))]
    public class LevelDataSOEditor : UnityEditor.Editor
    {
        private LevelDataSO _levelData;
        private SerializedProperty _ballPositionsProp;
        private SerializedProperty _obstaclePositionsProp;
        private PlacementMode _currentMode = PlacementMode.None;
        private bool _isDragging = false;
        private Vector2Int _lastPlacedTile = new Vector2Int(-1, -1);
        
        private Stack<EditorState> _undoStack = new Stack<EditorState>();
        private Stack<EditorState> _redoStack = new Stack<EditorState>();
        
        private const float TILE_SIZE = 40f;
        private const float BUTTON_SIZE = 50f;
        private const float SPACING = 10f;
        
        private static readonly Color GRID_COLOR = new Color(128f / 255f, 128f / 255f, 128f / 255f);
        private static readonly Color BALL_COLOR = new Color(255f / 255f, 175f / 255f, 0f / 255f);
        private static readonly Color OBSTACLE_COLOR = new Color(75f / 255f, 0f / 255f, 0f / 255f);
        
        private void OnEnable()
        {
            _levelData = (LevelDataSO)target;
            _ballPositionsProp = serializedObject.FindProperty("_ballPositions");
            _obstaclePositionsProp = serializedObject.FindProperty("_obstaclePositions");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            DrawToolbar();
            EditorGUILayout.Space(10);
            DrawGrid();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_levelData);
            }
        }
        
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal();
            
            GUI.enabled = _undoStack.Count > 0;
            if (GUILayout.Button("←", GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
            {
                Undo();
            }
            GUI.enabled = true;
            
            GUI.enabled = _redoStack.Count > 0;
            if (GUILayout.Button("→", GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
            {
                Redo();
            }
            GUI.enabled = true;
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Reset to Default", GUILayout.Width(BUTTON_SIZE * 2), GUILayout.Height(BUTTON_SIZE)))
            {
                ResetToDefault();
            }
            
            GUILayout.FlexibleSpace();
            
            Color ballButtonColor = _currentMode == PlacementMode.Ball ? BALL_COLOR : Color.white;
            GUI.backgroundColor = ballButtonColor;
            if (GUILayout.Button("●", GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
            {
                _currentMode = _currentMode == PlacementMode.Ball ? PlacementMode.None : PlacementMode.Ball;
            }
            GUI.backgroundColor = Color.white;
            
            Color obstacleButtonColor = _currentMode == PlacementMode.Obstacle ? OBSTACLE_COLOR : Color.white;
            GUI.backgroundColor = obstacleButtonColor;
            if (GUILayout.Button("■", GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
            {
                _currentMode = _currentMode == PlacementMode.Obstacle ? PlacementMode.None : PlacementMode.Obstacle;
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawGrid()
        {
            Vector2Int gridSize = _levelData.GridSize;
            
            for (int y = gridSize.y - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector2Int tilePos = new Vector2Int(x, y);
                    DrawTile(tilePos);
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void DrawTile(Vector2Int position)
        {
            bool hasBall = _levelData.BallPositions != null && _levelData.BallPositions.Contains(position);
            bool hasObstacle = _levelData.ObstaclePositions != null && _levelData.ObstaclePositions.Contains(position);
            
            Color tileColor = GRID_COLOR;
            if (hasBall) tileColor = BALL_COLOR;
            if (hasObstacle) tileColor = OBSTACLE_COLOR;
            
            GUI.backgroundColor = tileColor;
            
            if (GUILayout.Button("", GUILayout.Width(TILE_SIZE), GUILayout.Height(TILE_SIZE)))
            {
                if (_currentMode != PlacementMode.None)
                {
                    PlaceTile(position);
                }
            }
            
            Rect rect = GUILayoutUtility.GetLastRect();
            Event currentEvent = Event.current;
            
            if (rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && _currentMode != PlacementMode.None)
                {
                    _isDragging = true;
                    _lastPlacedTile = new Vector2Int(-1, -1);
                }
                else if (currentEvent.type == EventType.MouseDrag && _isDragging && _currentMode != PlacementMode.None)
                {
                    if (_lastPlacedTile != position)
                    {
                        PlaceTile(position);
                    }
                }
            }
            
            if (currentEvent.type == EventType.MouseUp)
            {
                _isDragging = false;
                _lastPlacedTile = new Vector2Int(-1, -1);
            }
            
            GUI.backgroundColor = Color.white;
        }
        
        private void PlaceTile(Vector2Int position)
        {
            SaveState();
            _redoStack.Clear();
            
            serializedObject.Update();
            
            if (_currentMode == PlacementMode.Ball)
            {
                RemovePosition(_obstaclePositionsProp, position);
                TogglePosition(_ballPositionsProp, position);
            }
            else if (_currentMode == PlacementMode.Obstacle)
            {
                RemovePosition(_ballPositionsProp, position);
                TogglePosition(_obstaclePositionsProp, position);
            }
            
            serializedObject.ApplyModifiedProperties();
            _lastPlacedTile = position;
        }
        
        private void SaveState()
        {
            EditorState state = new EditorState
            {
                BallPositions = new List<Vector2Int>(_levelData.BallPositions ?? new List<Vector2Int>()),
                ObstaclePositions = new List<Vector2Int>(_levelData.ObstaclePositions ?? new List<Vector2Int>())
            };
            _undoStack.Push(state);
        }
        
        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            
            EditorState currentState = new EditorState
            {
                BallPositions = new List<Vector2Int>(_levelData.BallPositions ?? new List<Vector2Int>()),
                ObstaclePositions = new List<Vector2Int>(_levelData.ObstaclePositions ?? new List<Vector2Int>())
            };
            _redoStack.Push(currentState);
            
            EditorState previousState = _undoStack.Pop();
            ApplyState(previousState);
        }
        
        private void Redo()
        {
            if (_redoStack.Count == 0) return;
            
            SaveState();
            
            EditorState nextState = _redoStack.Pop();
            ApplyState(nextState);
        }
        
        private void TogglePosition(SerializedProperty property, Vector2Int position)
        {
            int index = FindPositionIndex(property, position);
            if (index >= 0)
            {
                property.DeleteArrayElementAtIndex(index);
            }
            else
            {
                property.arraySize++;
                SerializedProperty element = property.GetArrayElementAtIndex(property.arraySize - 1);
                element.vector2IntValue = position;
            }
        }
        
        private void RemovePosition(SerializedProperty property, Vector2Int position)
        {
            int index = FindPositionIndex(property, position);
            if (index >= 0)
            {
                property.DeleteArrayElementAtIndex(index);
            }
        }
        
        private int FindPositionIndex(SerializedProperty property, Vector2Int position)
        {
            for (int i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).vector2IntValue == position)
                {
                    return i;
                }
            }
            return -1;
        }
        
        private void ApplyState(EditorState state)
        {
            serializedObject.Update();
            
            _ballPositionsProp.ClearArray();
            foreach (Vector2Int pos in state.BallPositions)
            {
                _ballPositionsProp.arraySize++;
                _ballPositionsProp.GetArrayElementAtIndex(_ballPositionsProp.arraySize - 1).vector2IntValue = pos;
            }
            
            _obstaclePositionsProp.ClearArray();
            foreach (Vector2Int pos in state.ObstaclePositions)
            {
                _obstaclePositionsProp.arraySize++;
                _obstaclePositionsProp.GetArrayElementAtIndex(_obstaclePositionsProp.arraySize - 1).vector2IntValue = pos;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void ResetToDefault()
        {
            SaveState();
            _redoStack.Clear();
            
            serializedObject.Update();
            
            _ballPositionsProp.ClearArray();
            _obstaclePositionsProp.ClearArray();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private enum PlacementMode
        {
            None,
            Ball,
            Obstacle
        }
        
        private class EditorState
        {
            public List<Vector2Int> BallPositions;
            public List<Vector2Int> ObstaclePositions;
        }
    }
}
