using System.Collections.Generic;
using _Game.TileGridSystem;
using UnityEngine;

namespace _Game.ObstacleSystem
{
    public class ObstacleManager : MonoBehaviour
    {
        private ObstacleSpawner _obstacleSpawner;
        private List<Obstacle> _spawnedObstacles;

        private void Awake()
        {
            _obstacleSpawner = ObstacleSpawner.Instance;
            _spawnedObstacles = new List<Obstacle>();
        }

        public void Initialize(List<Vector2Int> positions, TileGrid tileGrid)
        {
            ClearExistingObstacles();
            SpawnObstacles(positions, tileGrid);
        }

        private void SpawnObstacles(List<Vector2Int> positions, TileGrid tileGrid)
        {
            foreach (Vector2Int position in positions)
            {
                Tile tile = tileGrid.GetTileWithGridPosition(position.x, position.y);
                if (tile == null) continue;

                Vector2 worldPosition = tile.transform.position;
                Obstacle obstacle = _obstacleSpawner.SpawnObstacle(worldPosition, transform);
                obstacle.Initialize();
                _spawnedObstacles.Add(obstacle);
            }
        }

        private void ClearExistingObstacles()
        {
            foreach (Obstacle obstacle in _spawnedObstacles)
            {
                Destroy(obstacle.gameObject);
            }
            _spawnedObstacles.Clear();
        }
    }
}
