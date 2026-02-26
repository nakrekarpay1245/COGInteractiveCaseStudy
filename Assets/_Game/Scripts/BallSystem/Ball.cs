using System.Collections;
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
        [SerializeField, ReadOnly] private float _speed;
        [SerializeField] private Ease _easeType = Ease.Linear;

        [SerializeField, ReadOnly] private TileGrid _tileGrid;
        [SerializeField, ReadOnly] private Tile _tile;

        [Title("Components")]
        [SerializeField] private BallAnimator _ballAnimator;

        private Coroutine _moveCoroutine;
        private Vector2 _lastMoveDirection;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        void Awake()
        {
            if (_ballAnimator == null)
            {
                _ballAnimator = GetComponent<BallAnimator>();
            }
        }

        public void Initialize(TileGrid tileGrid, float speed)
        {
            _tileGrid = tileGrid;
            _speed = speed;
            SetTileReference();
            PaintStartingTile();
        }

        private void SetTileReference()
        {
            if (_tileGrid == null) return;

            _tile = _tileGrid.ClosestTile(transform.position);
            if (_tile != null)
            {
                _tile.SetBall(this);
            }
        }

        private void PaintStartingTile()
        {
            if (_tile != null)
            {
                _tile.Paint();
            }
        }

        public void Move(Vector2 direction, GridPathfinding gridPathfinding)
        {
            if (_moveCoroutine != null) return;
            if (gridPathfinding == null) return;

            List<Vector3> path = gridPathfinding.GetPath(transform.position, direction);
            if (path == null || path.Count <= 1) return;

            _lastMoveDirection = direction;

            if (_tile != null)
            {
                _tile.SetBall(null);
            }
            _tile = null;

            if (_ballAnimator != null)
            {
                _ballAnimator.AnimateMovement(direction);
            }

            Vector3 destinationPosition = path[path.Count - 1];
            Tile destinationTile = _tileGrid.ClosestTile(destinationPosition);
            if (destinationTile != null)
            {
                destinationTile.SetBall(this);
                _tile = destinationTile;
            }

            _moveCoroutine = StartCoroutine(MoveCoroutine(path));
        }

        private IEnumerator MoveCoroutine(List<Vector3> path)
        {
            int currentPathIndex = 0;

            Vector3[] pathArray = path.ToArray();
            float totalDistance = CalculatePathDistance(path);
            float duration = totalDistance / _speed;

            Tween moveTween = null;
            moveTween = transform.DOPath(pathArray, duration, PathType.Linear)
                .SetEase(_easeType)
                .SetAutoKill(true)
                .SetLink(gameObject)
                .OnUpdate(() =>
                {
                    int newPathIndex = Mathf.FloorToInt(moveTween.ElapsedPercentage() * path.Count);
                    if (newPathIndex > currentPathIndex && newPathIndex < path.Count)
                    {
                        Tile currentTile = _tileGrid.ClosestTile(path[newPathIndex]);
                        if (currentTile != null)
                        {
                            currentTile.Paint();
                        }
                        currentPathIndex = newPathIndex;
                    }
                });

            yield return moveTween.WaitForCompletion();

            if (_ballAnimator != null)
            {
                _ballAnimator.ResetScale(_lastMoveDirection);
            }

            _moveCoroutine = null;
        }

        private float CalculatePathDistance(List<Vector3> path)
        {
            float distance = 0f;
            for (int i = 0; i < path.Count - 1; i++)
            {
                distance += Vector3.Distance(path[i], path[i + 1]);
            }
            return distance;
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
