using System.Collections.Generic;
using _Game.TileGridSystem;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallManager : MonoBehaviour
    {
        private BallSpawner _ballSpawner;
        private List<Ball> _spawnedBalls;

        private void Awake()
        {
            _ballSpawner = BallSpawner.Instance;
            _spawnedBalls = new List<Ball>();
        }

        public void Initialize(List<Vector2Int> positions, TileGrid tileGrid)
        {
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
