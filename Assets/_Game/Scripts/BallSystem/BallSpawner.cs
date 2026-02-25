using _Game.Core;
using UnityEditor;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallSpawner : Singleton<BallSpawner>
    {
        [SerializeField]
        private Ball _ballPrefab;

        public Ball SpawnBall(Vector2 position, Transform parentTransform)
        {
            Ball ball = Instantiate(_ballPrefab, position, Quaternion.identity, parentTransform);
            return ball;
        }

        public Ball SpawnBallInEditor(Vector2 position, Transform parentTransform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject ballGO = PrefabUtility.InstantiatePrefab(_ballPrefab.gameObject, parentTransform) as GameObject;
                ballGO.transform.position = position;
                ballGO.transform.rotation = Quaternion.identity;

                Ball ball = ballGO.GetComponent<Ball>();
                return ball;
            }
#endif
            return null;
        }
    }
}
