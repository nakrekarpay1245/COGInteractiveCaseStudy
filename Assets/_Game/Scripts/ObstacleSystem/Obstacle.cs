using _Game.TileGridSystem;
using _Game.Utilities;
using TriInspector;
using UnityEngine;

namespace _Game.ObstacleSystem
{
    public class Obstacle : MonoBehaviour
    {
        [Title("Obstacle")]
        [SerializeField, ReadOnly] private Vector2 _gridPosition;

        [SerializeField, ReadOnly] private TileGrid _tileGrid;
        [SerializeField, ReadOnly] private Tile _tile;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public void Initialize(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
            SetTileReference();
        }

        private void SetTileReference()
        {
            if (_tileGrid == null) return;

            _tile = _tileGrid.ClosestTile(transform.position);
            if (_tile != null)
            {
                _tile.SetObstacle(this);
            }
        }

#if UNITY_EDITOR
        public void InitializeForEditor()
        {
            if (Application.isPlaying) return;

            RichLogger.Log($"{name} initialized (Editor Mode)!", RichLogger.Color.cyan);
        }
#endif
    }
}
