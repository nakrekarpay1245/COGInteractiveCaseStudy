using UnityEngine;
using System.Collections.Generic;
using TriInspector;

namespace _Game.TileGridSystem
{
    public class TileGridFrame : MonoBehaviour
    {
        [Title("TileGridFrame Settings")]
        [Header("References")]
        [SerializeField] private TileGrid _tileGrid;

        [Header("Frame Prefabs")]
        [SerializeField] private GameObject _cornerPrefab;
        [SerializeField] private GameObject _edgePrefab;
        [SerializeField] private bool _generateOnStart = false;

        private readonly List<GameObject> _cornerFrames = new();
        private readonly List<GameObject> _edgeFrames = new();

        private void Start()
        {
            if (_generateOnStart)
            {
                Initialize();
            }
        }

        [Button("Generate Frame (Runtime)")]
        public void Initialize()
        {
            // TileGrid tileGrid = LevelManager.Instance.CurrentLevel.TileGrid;

            if (_tileGrid == null)
            {
                Debug.LogError("TileGrid reference is missing. Cannot generate frame.");
                return;
            }

            ClearExistingFrames();
            GenerateFrame(_tileGrid);
        }

        private void GenerateFrame(TileGrid tileGrid)
        {
            // Vector2Int gridSize = LevelManager.Instance.CurrentLevel.GridSize;
            Vector2Int gridSize = _tileGrid.GridSize;

            // Corners
            CreateCorner(0, 0, 270);                         // Bottom-left
            CreateCorner(gridSize.x - 1, 0, 0);           // Bottom-right
            CreateCorner(0, gridSize.y - 1, 180);          // Top-left
            CreateCorner(gridSize.x - 1, gridSize.y - 1, 90); // Top-right

            // Bottom & Top edges (left to right, excluding corners)
            for (int x = 1; x < gridSize.x - 1; x++)
            {
                CreateEdge(x, 0, 180);                        // Bottom
                CreateEdge(x, gridSize.y - 1, 0);         // Top
            }

            // Left & Right edges (bottom to top, excluding corners)
            for (int y = 1; y < gridSize.y - 1; y++)
            {
                CreateEdge(0, y, 90);                      // Left
                CreateEdge(gridSize.x - 1, y, 270);          // Right
            }
        }

        private void CreateCorner(int x, int y, float rotation)
        {
            // Tile tile = LevelManager.Instance.CurrentLevel.TileGrid.GetTileWithGridPosition(x, y);
            Tile tile = _tileGrid.GetTileWithGridPosition(x, y);
            if (tile == null) return;

            Vector3 pos = tile.transform.position;
            GameObject instance = Instantiate(_cornerPrefab, pos, Quaternion.Euler(0, 0, rotation), transform);
            instance.name = $"CornerFrame_{x}_{y}";
            _cornerFrames.Add(instance);
        }

        private void CreateEdge(int x, int y, float rotation)
        {
            // Tile tile = LevelManager.Instance.CurrentLevel.TileGrid.GetTileWithGridPosition(x, y);
            Tile tile = _tileGrid.GetTileWithGridPosition(x, y);
            if (tile == null) return;

            Vector3 pos = tile.transform.position;
            GameObject instance = Instantiate(_edgePrefab, pos, Quaternion.Euler(0, 0, rotation), transform);
            instance.name = $"EdgeFrame_{x}_{y}";
            _edgeFrames.Add(instance);
        }

        public void ClearExistingFrames()
        {
            foreach (var frame in _cornerFrames)
            {
                if (frame != null)
                {
                    if (Application.isPlaying) Destroy(frame);
                    else DestroyImmediate(frame);
                }
            }
            _cornerFrames.Clear();

            foreach (var frame in _edgeFrames)
            {
                if (frame != null)
                {
                    if (Application.isPlaying) Destroy(frame);
                    else DestroyImmediate(frame);
                }
            }
            _edgeFrames.Clear();
        }
    }
}