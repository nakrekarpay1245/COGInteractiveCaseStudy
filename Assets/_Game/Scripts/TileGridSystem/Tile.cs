using _Game.BallSystem;
using _Game.ColorSystem;
using _Game.ObstacleSystem;
using _Game.Utilities;
using TriInspector;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class Tile : MonoBehaviour
    {
        [Title("Tile")]
        [SerializeField, ReadOnly] private Vector2 _gridPosition;

        [SerializeField, ReadOnly] private TileGrid _tileGrid;
        [SerializeField, ReadOnly] private Obstacle _obstacle;
        [SerializeField, ReadOnly] private Ball _ball;
        [SerializeField] private TileSpriteHandler _tileSpriteHandler;
        
        private bool _isPainted;

        public Obstacle Obstacle { get => _obstacle; }
        public Ball Ball { get => _ball; }
        public bool IsPainted { get => _isPainted; }

        void Awake()
        {
            if(_tileSpriteHandler== null)
            {
                _tileSpriteHandler = GetComponent<TileSpriteHandler>();
            }
        }

        public void Initialize()
        {
            _tileSpriteHandler.Initialize();
        }

        public void SetTileGrid(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
        }

        public void SetObstacle(Obstacle obstacle)
        {
            _obstacle = obstacle;
        }

        public void SetBall(Ball ball)
        {
            _ball = ball;
        }

        public void SetPaintColor(ColorType colorType)
        {
            _tileSpriteHandler.SetPaintColor(colorType);
        }

        public void Paint()
        {
            _isPainted = true;
            _tileSpriteHandler.Paint();
            _tileGrid?.NotifyTilePainted(this);
        }

        public Tile GetNextTile(Vector2 direction)
        {
            if (_tileGrid == null) return null;

            Vector2Int currentGridPos = _tileGrid.GetTileGridPosition(this);
            if (currentGridPos.x == -1) return null;

            Vector2Int nextGridPos = currentGridPos + new Vector2Int((int)direction.x, (int)direction.y);
            Tile nextTile = _tileGrid.GetTileWithGridPosition(nextGridPos.x, nextGridPos.y);

            if (nextTile != null && nextTile.Obstacle != null) return null;
            if (nextTile != null && nextTile.Ball != null) return null;

            return nextTile;
        }

        public Tile GetNextTile(Tile previousTile)
        {
            if (_tileGrid == null || previousTile == null) return null;

            Vector2Int currentGridPos = _tileGrid.GetTileGridPosition(this);
            Vector2Int previousGridPos = _tileGrid.GetTileGridPosition(previousTile);

            if (currentGridPos.x == -1 || previousGridPos.x == -1) return null;

            Vector2Int direction = currentGridPos - previousGridPos;
            Vector2Int nextGridPos = currentGridPos + direction;

            Tile nextTile = _tileGrid.GetTileWithGridPosition(nextGridPos.x, nextGridPos.y);

            if (nextTile != null && nextTile.Obstacle != null) return null;
            if (nextTile != null && nextTile.Ball != null) return null;

            return nextTile;
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