using _Game.Utilities;
using TriInspector;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class Tile : MonoBehaviour
    {
        [Title("Tile")]
        [SerializeField, ReadOnly] private Vector2 _gridPosition;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        private TileGrid _tileGrid;

        public void Initialize()
        {
            // RichLogger.Log($"{name} initialized (Play Mode)!");
        }

        public void SetTileGrid(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
        }

        public Tile GetNextTile(Vector2 direction)
        {
            if (_tileGrid == null) return null;

            Vector2Int currentGridPos = _tileGrid.GetTileGridPosition(this);
            if (currentGridPos.x == -1) return null;

            Vector2Int nextGridPos = currentGridPos + new Vector2Int((int)direction.x, (int)direction.y);
            return _tileGrid.GetTileWithGridPosition(nextGridPos.x, nextGridPos.y);
        }

        public Tile GetNextTile(Tile previousTile)
        {
            if (_tileGrid == null || previousTile == null) return null;

            Vector2Int currentGridPos = _tileGrid.GetTileGridPosition(this);
            Vector2Int previousGridPos = _tileGrid.GetTileGridPosition(previousTile);

            if (currentGridPos.x == -1 || previousGridPos.x == -1) return null;

            Vector2Int direction = currentGridPos - previousGridPos;
            Vector2Int nextGridPos = currentGridPos + direction;

            return _tileGrid.GetTileWithGridPosition(nextGridPos.x, nextGridPos.y);
        }

#if UNITY_EDITOR
        public void InitializeForEditor()
        {
            if (Application.isPlaying) return;

            RichLogger.Log($"{name} initialized (Editor Mode)!");
        }
#endif
    }
}