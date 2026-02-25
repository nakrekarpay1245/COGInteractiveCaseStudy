using _Game.Core;
using UnityEditor;
using UnityEngine;

namespace _Game.ObstacleSystem
{
    public class ObstacleSpawner : Singleton<ObstacleSpawner>
    {
        [SerializeField]
        private Obstacle _obstaclePrefab;

        public Obstacle SpawnObstacle(Vector2 position, Transform parentTransform)
        {
            Obstacle obstacle = Instantiate(_obstaclePrefab, position, Quaternion.identity, parentTransform);
            return obstacle;
        }

        public Obstacle SpawnObstacleInEditor(Vector2 position, Transform parentTransform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject obstacleGO = PrefabUtility.InstantiatePrefab(_obstaclePrefab.gameObject, parentTransform) as GameObject;
                obstacleGO.transform.position = position;
                obstacleGO.transform.rotation = Quaternion.identity;

                Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();
                return obstacle;
            }
#endif
            return null;
        }
    }
}
