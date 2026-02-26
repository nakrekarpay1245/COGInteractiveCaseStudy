using System.Collections.Generic;
using System.Linq;
using _Game.ColorSystem;
using _Game.Input;
using _Game.TileGridSystem;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;

        private BallSpawner _ballSpawner;
        private List<Ball> _spawnedBalls;
        private GridPathfinding _gridPathfinding;
        private float _ballSpeed;

        private void Awake()
        {
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
            // RichLogger.Log("Handling swipe left");

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
            IEnumerable<Ball> sortedBalls = direction.x > 0 ? _spawnedBalls.OrderByDescending(ball => ball.transform.position.x) :
                                            direction.x < 0 ? _spawnedBalls.OrderBy(ball => ball.transform.position.x) :
                                            direction.y > 0 ? _spawnedBalls.OrderByDescending(ball => ball.transform.position.y) :
                                            _spawnedBalls.OrderBy(ball => ball.transform.position.y);

            foreach (Ball ball in sortedBalls)
            {
                ball.Move(direction, _gridPathfinding);
            }
        }

        public void Initialize(List<Vector2Int> positions, TileGrid tileGrid, GridPathfinding gridPathfinding, BallSpawner ballSpawner, float ballSpeed)
        {
            _gridPathfinding = gridPathfinding;
            _ballSpawner = ballSpawner;
            _ballSpeed = ballSpeed;
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
                ball.Initialize(tileGrid, _ballSpeed);
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

        public void SetLevelColor(ColorType colorType)
        {
            foreach (Ball ball in _spawnedBalls)
            {
                ball.SetPaintColor(colorType);
            }
        }
    }
}
