using System.Collections;
using System.Collections.Generic;
using _Game.Core;
using _Game.TileGridSystem;
using TriInspector;
using UnityEngine;

namespace _Game.LevelSystem
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Title("References")]
        [SerializeField] private TileGrid _tileGrid;
        [SerializeField] private TileGridFrame _tileGridFrame;
        
        [Title("Level Data")]
        [SerializeField] private List<LevelDataSO> _levelList;
        
        private int _currentLevelIndex;
        public TileGrid TileGrid { get => _tileGrid; set => _tileGrid = value; }

        private void Awake()
        {
            if (_tileGrid == null)
            {
                Debug.LogError("TileGrid reference is missing!");
            }
            
            if (_tileGridFrame == null)
            {
                Debug.LogError("TileGridFrame reference is missing!");
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
            _tileGridFrame.Initialize(_tileGrid);
        }
    }
}
