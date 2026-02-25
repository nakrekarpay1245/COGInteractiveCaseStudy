using System;
using _Game.TileGridSystem;
using UnityEngine;

namespace _Game.ProgressionSystem
{
    public class ProgressionManager : MonoBehaviour
    {
        private float _currentProgress;
        private Color _levelColor;
        private TileGrid _tileGrid;

        public float CurrentProgress { get => _currentProgress; }
        public event Action OnProgressCompleted;

        public void Initialize(Color levelColor, TileGrid tileGrid)
        {
            _levelColor = levelColor;
            _tileGrid = tileGrid;

            if (_tileGrid != null)
            {
                _tileGrid.OnTilePainted += OnTilePainted;
            }
        }

        private void OnDisable()
        {
            if (_tileGrid != null)
            {
                _tileGrid.OnTilePainted -= OnTilePainted;
            }
        }

        private void OnTilePainted(int paintedCount, int totalCount)
        {
            if (totalCount == 0) return;

            _currentProgress = (float)paintedCount / totalCount;

            if (_currentProgress >= 1f)
            {
                OnProgressCompleted?.Invoke();
            }
        }
    }
}
