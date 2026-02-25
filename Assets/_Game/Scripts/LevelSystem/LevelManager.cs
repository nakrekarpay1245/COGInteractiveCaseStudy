using System.Collections;
using System.Collections.Generic;
using _Game.BallSystem;
using _Game.ColorSystem;
using _Game.Core;
using _Game.ObstacleSystem;
using _Game.TileGridSystem;
using TriInspector;
using UnityEngine;

namespace _Game.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Title("References")]
        [Header("Grid")]
        [SerializeField] private TileGrid _tileGrid;
        [SerializeField] private TileGridFrame _tileGridFrame;
        
        [Header("Managers")]
        [SerializeField] private BallManager _ballManager;
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private ColorManager _colorManager;
        
        [Title("Level Data")]
        [SerializeField] private List<LevelDataSO> _levelList;
        
        private int _currentLevelIndex;
        private GridPathfinding _gridPathfinding;

        public TileGrid TileGrid { get => _tileGrid; }
        public List<LevelDataSO> LevelList { get => _levelList; }
        public GridPathfinding GridPathfinding { get => _gridPathfinding; }
        public ColorManager ColorManager { get => _colorManager; }

#if UNITY_EDITOR
        public static int EditorStartLevelIndex = 0;
#endif

        private void Awake()
        {
#if UNITY_EDITOR
            _currentLevelIndex = EditorStartLevelIndex;
#endif

            if (_tileGrid == null)
            {
                Debug.LogError("TileGrid reference is missing!");
            }
            
            if (_tileGridFrame == null)
            {
                Debug.LogError("TileGridFrame reference is missing!");
            }
            
            if (_ballManager == null)
            {
                Debug.LogError("BallManager reference is missing!");
            }
            
            if (_obstacleManager == null)
            {
                Debug.LogError("ObstacleManager reference is missing!");
            }
            
            if (_levelList == null || _levelList.Count == 0)
            {
                Debug.LogError("LevelList is empty or missing!");
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
            
            _tileGrid.Initialize(currentLevel.GridSize);
            _tileGrid.SetPaintColor(currentLevel.LevelColor);
            _tileGridFrame.Initialize(_tileGrid);

            _gridPathfinding = new GridPathfinding();
            _gridPathfinding.Initialize(_tileGrid);

            _ballManager.Initialize(currentLevel.BallPositions, _tileGrid, _gridPathfinding);
            _obstacleManager.Initialize(currentLevel.ObstaclePositions, _tileGrid);
        }
    }
}
