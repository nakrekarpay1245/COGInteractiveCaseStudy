using UnityEngine;
using TriInspector;
using UnityEditor;
using System.Linq;
using _Game.Utilities;
using _Game.ColorSystem;

namespace _Game.TileGridSystem
{
    public class TileGrid : MonoBehaviour
    {
        [Title("Grid Settings")]
        [Header("Grid Scale")]
        [SerializeField, ReadOnly] private Vector2Int _gridSize = new Vector2Int(2, 2);
        [SerializeField] private bool _centerGridAtOrigin = false;

        public Vector2Int GridSize { get => _gridSize; set => _gridSize = value; }

        private Tile[,] _tileGrid;
        public Tile[,] PublicTileGrid { get => _tileGrid; }

        public Vector2 CenterTileWorldPosition => new Vector2(
             ((float)_gridSize.x) / 2f,
            ((float)_gridSize.y) / 2f
         ) - (Vector2.right * 0.5f);

        [Button]
        public void Initialize(Vector2Int gridSize)
        {
            _gridSize = gridSize;

            ClearTileGrid();
            CreateAllTiles();

            _tileGrid = new Tile[_gridSize.x, _gridSize.y];

            Tile[] tileArray = GetComponentsInChildren<Tile>();

            if (tileArray.Length != _gridSize.x * _gridSize.y)
            {
                RichLogger.LogError($"Tile count ({tileArray.Length}) does not match width*height ({_gridSize.x * _gridSize.y})");
                return;
            }

            foreach (Tile tile in tileArray)
            {
                string tileName = tile.name;  // Expected format: Tile [x,y]

                int x, y;
                if (TryGetTileCoordinates(tileName, out x, out y))
                {
                    _tileGrid[x, y] = tile;
                    tile.SetTileGrid(this);
                    tile.Initialize();
                }
                else
                {
                    RichLogger.LogError($"Tile name '{tileName}' is not in correct format (Tile [x,y])");
                }
            }

            // RichLogger.Log("TileGrid initialized successfully.");
        }

        public void SetPaintColor(ColorType colorType)
        {
            foreach (Tile tile in _tileGrid)
            {
                tile.SetPaintColor(colorType);
            }
        }

        [Button]
        private void ClearTileGrid()
        {
            if (Application.isPlaying)
            {
                Tile[] tileArray = GetComponentsInChildren<Tile>();

                foreach (Tile tile in tileArray)
                {
                    if (tile != null)
                    {
                        DestroyImmediate(tile.gameObject);
                    }
                }

                _tileGrid = null;
                // RichLogger.Log("Tile grid cleared in editor.");
            }
        }

        private void CreateAllTiles()
        {
            _tileGrid = new Tile[_gridSize.x, _gridSize.y];

            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    CreateTile(x, y);
                }
            }

            // Debug.Log("All tiles created successfully.");
        }

        private Tile CreateTile(int x, int y)
        {
            Vector2 tilePosition = GetTilePosition(x, y);

            Tile tile = TileSpawner.Instance.SpawnTile(tilePosition, transform);

            _tileGrid[x, y] = tile;
            tile.name = $"Tile [{x},{y}]";

            return tile;
        }

        private Vector2 GetTilePosition(int x, int y)
        {
            Vector2 basePosition = new Vector2(x, y);

            if (_centerGridAtOrigin)
            {
                Vector2 centerOffset = new Vector2((_gridSize.x - 1) * 0.5f, (_gridSize.y - 1) * 0.5f);
                basePosition -= centerOffset;
            }

            return basePosition + (Vector2)transform.position;
        }

        private const char BRACKET_OPEN = '[';
        private const char BRACKET_CLOSE = ']';
        private const char COMMA = ',';

        private bool TryGetTileCoordinates(string name, out int x, out int y)
        {
            x = 0;
            y = 0;

            if (string.IsNullOrEmpty(name))
                return false;

            // Expected: Tile [x,y]
            int start = name.IndexOf(BRACKET_OPEN);
            int comma = name.IndexOf(COMMA, start);
            int end = name.IndexOf(BRACKET_CLOSE, comma);

            if (start == -1 || comma == -1 || end == -1)
                return false;

            string xStr = name.Substring(start + 1, comma - start - 1);
            string yStr = name.Substring(comma + 1, end - comma - 1);

            return int.TryParse(xStr, out x) && int.TryParse(yStr, out y);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < _gridSize.x && y >= 0 && y < _gridSize.y;
        }

        public Tile GetTileWithGridPosition(int x, int y)
        {
            if (IsWithinBounds(x, y))
            {
                return _tileGrid[x, y];
            }

            // RichLogger.LogWarning($"Requested grid position [{x},{y}] is out of bounds.");
            return null;
        }

        public Tile ClosestTile(Vector2 worldPosition)
        {
            return _tileGrid.Cast<Tile>()
                            .OrderBy(tile => Vector2.SqrMagnitude((Vector2)tile.transform.position - worldPosition))
                            .FirstOrDefault();
        }

        public Vector2Int GetTileGridPosition(Tile tile)
        {
            if (tile == null || _tileGrid == null)
            {
                RichLogger.LogWarning("Tile or _tileGrid is null in GetTileGridPosition.");
                return Vector2Int.one * -1;
            }

            int width = _tileGrid.GetLength(0);
            int height = _tileGrid.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_tileGrid[x, y] == tile)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            RichLogger.LogWarning("Given tile was not found in the grid.");
            return Vector2Int.one * -1;
        }

#if UNITY_EDITOR
        #region EDITOR FUNCTIONS
        [Button]
        public void InitializeForEditor()
        {
            ClearForEditor();
            CreateAllTiles();
        }

        [Button]
        private void ClearForEditor()
        {
            if (!Application.isPlaying)
            {
                foreach (var tile in GetComponentsInChildren<Tile>())
                {
                    if (tile != null)
                    {
                        UnityEngine.Object.DestroyImmediate(tile.gameObject);
                    }
                }

                _tileGrid = null;
                RichLogger.Log("Tile grid cleared in editor.");
                EditorUtility.SetDirty(gameObject);
            }
        }

        private Tile CreateTileInEditor(int x, int y)
        {
            Vector2 tilePosition = GetTilePosition(x, y);

            Tile tile = FindAnyObjectByType<TileSpawner>().SpawnTileInEditor(tilePosition, transform);

            _tileGrid[x, y] = tile;
            tile.name = $"Tile [{x},{y}]";

            if (!Application.isPlaying)
            {
                Undo.RegisterCreatedObjectUndo(tile.gameObject, "Create Tile");
                EditorUtility.SetDirty(tile);
            }
            return tile;
        }

        private void AutoBuildGridFromChildren()
        {
            _tileGrid = new Tile[_gridSize.x, _gridSize.y];

            var tiles = GetComponentsInChildren<Tile>();

            if (tiles.Length != _gridSize.x * _gridSize.y)
            {
                RichLogger.LogError($"Tile count ({tiles.Length}) does not match width*height ({_gridSize.x * _gridSize.y})");
                return;
            }

            foreach (var tile in tiles)
            {
                string tileName = tile.name;  // Expected format: Tile [x,y]

                int x, y;
                if (TryGetTileCoordinates(tileName, out x, out y))
                {
                    _tileGrid[x, y] = tile;
                }
                else
                {
                    RichLogger.LogError($"Tile name '{tileName}' is not in correct format (Tile [x,y])");
                }
            }
        }

        private Tile ClosestTileForEditor(Vector2 worldPosition)
        {
            AutoBuildGridFromChildren();
            return _tileGrid.Cast<Tile>()
                            .OrderBy(tile => Vector2.SqrMagnitude((Vector2)tile.transform.position - worldPosition))
                            .FirstOrDefault();
        }
        #endregion
#endif
    }
}