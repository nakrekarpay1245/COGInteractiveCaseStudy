using System;
using System.Collections;
using System.Collections.Generic;
using _Game.BallSystem;
using _Game.CameraSystem;
using _Game.ColorSystem;
using _Game.Core;
using _Game.FloorSystem;
using _Game.ObstacleSystem;
using _Game.ProgressionSystem;
using _Game.SaveSystem;
using _Game.TileGridSystem;
using _Game.Utilities;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Title("References")]
        [Header("Grid")]
        [SerializeField] private TileGrid _tileGrid;
        [SerializeField] private TileGridFrame _tileGridFrame;
        [SerializeField] private TileSpawner _tileSpawner;
        
        [Header("Ball")]
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private BallSpawner _ballSpawner;
        
        [Header("Obstacle")]
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        
        [Header("Managers")]
        [SerializeField] private ColorManager _colorManager;
        [SerializeField] private ProgressionManager _progressionManager;
        [SerializeField] private ProgressionBar _progressionBar;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private FloorManager _floorManager;
        
        [Title("Level Data")]
        [SerializeField] private List<LevelDataSO> _levelList;
        [SerializeField] private bool _isRandomizeLevelOnLoop;
        
        private int _currentLevelIndex;
        private GridPathfinding _gridPathfinding;
        private bool _isLevelCompleted;

        public static event Action OnWin;

        public TileGrid TileGrid { get => _tileGrid; }
        public List<LevelDataSO> LevelList { get => _levelList; }
        public GridPathfinding GridPathfinding { get => _gridPathfinding; }
        public ColorManager ColorManager { get => _colorManager; }
        public ProgressionManager ProgressionManager { get => _progressionManager; }

#if UNITY_EDITOR
        public static int EditorStartLevelIndex = 0;
#endif

        private void Awake()
        {
            _currentLevelIndex = SaveManager.Load();
            
            if (_currentLevelIndex >= _levelList.Count)
            {
                _currentLevelIndex = _isRandomizeLevelOnLoop ? UnityEngine.Random.Range(0, _levelList.Count) : 0;
                SaveManager.Save(_currentLevelIndex);
            }
            
            RichLogger.Log($"Current level index: {_currentLevelIndex}", RichLogger.Color.purple);

            if (_tileGrid == null)
            {
                RichLogger.LogError("TileGrid reference is missing!");
            }
            
            if (_tileGridFrame == null)
            {
                RichLogger.LogError("TileGridFrame reference is missing!");
            }
            
            if (_ballManager == null)
            {
                RichLogger.LogError("BallManager reference is missing!");
            }
            
            if (_obstacleManager == null)
            {
                RichLogger.LogError("ObstacleManager reference is missing!");
            }
            
            if (_levelList == null || _levelList.Count == 0)
            {
                RichLogger.LogError("LevelList is empty or missing!");
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            yield return null;
            InitializeGrid();
        }

        private void InitializeGrid()
        {            
            LevelDataSO currentLevel = _levelList[_currentLevelIndex];
            ColorType levelColor = currentLevel.LevelColor;
            
            RichLogger.Log($"Initializing level {_currentLevelIndex} - Grid: {currentLevel.GridSize}", RichLogger.Color.cyan);
            
            if (_cameraManager != null)
            {
                _cameraManager.SetOrthoSize(currentLevel.CameraOrthoSize);
            }
            
            if (_floorManager != null)
            {
                _floorManager.Initialize(currentLevel.FloorSize);
            }
            
            _tileGrid.Initialize(currentLevel.GridSize, currentLevel.LevelColor, _tileSpawner);
            _tileGrid.SetLevelColor(levelColor);
            _tileGridFrame.Initialize(_tileGrid);

            _gridPathfinding = new GridPathfinding();
            _gridPathfinding.Initialize(_tileGrid);

            _obstacleManager.Initialize(currentLevel.ObstaclePositions, _tileGrid, _obstacleSpawner);

            _tileGrid.UpdateFreeTiles();

            _progressionManager.Initialize(_colorManager.GetColor(levelColor), _tileGrid);
            _progressionBar.Initialize(_colorManager.GetColor(levelColor));

            _progressionManager.OnProgressCompleted += OnProgressCompleted;

            _ballManager.Initialize(currentLevel.BallPositions, _tileGrid, _gridPathfinding, _ballSpawner, currentLevel.BallSpeed);
            _ballManager.SetLevelColor(levelColor);
        }

        private void OnDisable()
        {
            if (_progressionManager != null)
            {
                _progressionManager.OnProgressCompleted -= OnProgressCompleted;
            }
        }

        private void OnProgressCompleted()
        {
            if (_isLevelCompleted) return;
            
            _isLevelCompleted = true;
            Win();
        }

        public void Win()
        {
            OnWin?.Invoke();
        }

        public void NextLevel()
        {
            _currentLevelIndex++;
            
            if (_currentLevelIndex >= _levelList.Count)
            {
                _currentLevelIndex = _isRandomizeLevelOnLoop ? UnityEngine.Random.Range(0, _levelList.Count) : 0;
                RichLogger.Log($"All levels completed! Loop mode: {(_isRandomizeLevelOnLoop ? "Random" : "Sequential")}", RichLogger.Color.yellow);
            }
            
            RichLogger.Log($"NextLevel called - New index: {_currentLevelIndex}", RichLogger.Color.green);
            SaveManager.Save(_currentLevelIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
