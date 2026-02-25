using System.Collections.Generic;
using _Game.TileGridSystem;
using _Game.Utilities;
using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace _Game.BallSystem
{
    public class Ball : MonoBehaviour
    {
        [Title("Ball")]
        [SerializeField, ReadOnly] private Vector2 _gridPosition;
        [SerializeField] private float _moveDuration = 0.3f;

        private Tween _moveTween;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public void Initialize()
        {
            // RichLogger.Log($"{name} initialized (Play Mode)!");
        }

        public void Move(Vector2 direction, GridPathfinding gridPathfinding)
        {
            if (gridPathfinding == null) return;

            List<Vector3> path = gridPathfinding.GetPath(transform.position, direction);
            if (path == null || path.Count <= 1) return;

            _moveTween?.Kill();

            Vector3[] pathArray = path.ToArray();
            _moveTween = transform.DOPath(pathArray, _moveDuration * path.Count, PathType.Linear)
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetLink(gameObject);
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
