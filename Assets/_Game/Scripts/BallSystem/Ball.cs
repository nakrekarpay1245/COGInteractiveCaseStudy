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
        [SerializeField] private float _moveDuration = 0.3f;

        [SerializeField, ReadOnly] private TileGrid _tileGrid;
        [SerializeField, ReadOnly] private Tile _tile;

        private Coroutine _moveCoroutine;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public void Initialize(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
            SetTileReference();
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

        public void Move(Vector2 direction, GridPathfinding gridPathfinding)
        {
            if (_moveCoroutine != null) return;
            if (gridPathfinding == null) return;

            List<Vector3> path = gridPathfinding.GetPath(transform.position, direction);
            if (path == null || path.Count <= 1) return;

            if (_tile != null)
            {
                _tile.SetBall(null);
            }
            _tile = null;

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
            Vector3[] pathArray = path.ToArray();
            Tween moveTween = transform.DOPath(pathArray, _moveDuration * path.Count, PathType.Linear)
                .SetEase(Ease.Linear)
                .SetAutoKill(true)
                .SetLink(gameObject);

            yield return moveTween.WaitForCompletion();

            _moveCoroutine = null;
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
