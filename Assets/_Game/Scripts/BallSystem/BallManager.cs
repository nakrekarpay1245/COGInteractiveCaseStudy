using System.Collections.Generic;
using _Game.Input;
using _Game.TileGridSystem;
using _Game.Utilities;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;

        private BallSpawner _ballSpawner;
        private List<Ball> _spawnedBalls;
        private GridPathfinding _gridPathfinding;

        private void Awake()
        {
            _ballSpawner = BallSpawner.Instance;
            _spawnedBalls = new List<Ball>();
        }

        private void OnEnable()
        {
            if (_playerInput == null) return;

            _playerInput.OnSwipeLeft.AddListener(HandleSwipeLeft);
            _playerInput.OnSwipeRight.AddListener(HandleSwipeRight);
            _playerInput.OnSwipeUp.AddListener(HandleSwipeUp);
            _playerInput.OnSwipeDown.AddListener(HandleSwipeDown);
        }

        private void OnDisable()
        {
            if (_playerInput == null) return;

            _playerInput.OnSwipeLeft.RemoveListener(HandleSwipeLeft);
            _playerInput.OnSwipeRight.RemoveListener(HandleSwipeRight);
            _playerInput.OnSwipeUp.RemoveListener(HandleSwipeUp);
            _playerInput.OnSwipeDown.RemoveListener(HandleSwipeDown);
        }

        private void HandleSwipeLeft()
        {
            RichLogger.Log("Handling swipe left");

            MoveBalls(Vector2.left);
        }

        private void HandleSwipeRight()
        {
            // RichLogger.Log("Handling swipe right");

            MoveBalls(Vector2.right);
        }

        private void HandleSwipeUp()
        {
            // RichLogger.Log("Handling swipe up");

            MoveBalls(Vector2.up);
        }

        private void HandleSwipeDown()
        {
            // RichLogger.Log("Handling swipe down");

            MoveBalls(Vector2.down);
        }

        private void MoveBalls(Vector2 direction)
        {
            foreach (Ball ball in _spawnedBalls)
            {
                // RichLogger.Log($"Moving {ball.name} in direction {direction}");
                ball.Move(direction, _gridPathfinding);
            }
        }

        public void Initialize(List<Vector2Int> positions, TileGrid tileGrid, GridPathfinding gridPathfinding)
        {
            _gridPathfinding = gridPathfinding;
            ClearExistingBalls();
            SpawnBalls(positions, tileGrid);
        }

        private void SpawnBalls(List<Vector2Int> positions, TileGrid tileGrid)
        {
            foreach (Vector2Int position in positions)
            {
                Tile tile = tileGrid.GetTileWithGridPosition(position.x, position.y);
                if (tile == null) continue;

                Vector2 worldPosition = tile.transform.position;
                Ball ball = _ballSpawner.SpawnBall(worldPosition, transform);
                ball.Initialize();
                _spawnedBalls.Add(ball);
            }
        }

        private void ClearExistingBalls()
        {
            foreach (Ball ball in _spawnedBalls)
            {
                Destroy(ball.gameObject);
            }
            _spawnedBalls.Clear();
        }
    }
}
