using System.Collections.Generic;
using _Game.Utilities;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class GridPathfinding
    {
        private TileGrid _tileGrid;

        public void Initialize(TileGrid tileGrid)
        {
            _tileGrid = tileGrid;
        }

        public List<Vector3> GetPath(Vector3 currentPosition, Vector2 direction)
        {
            List<Vector3> path = new List<Vector3>();

            Tile currentTile = _tileGrid.ClosestTile(currentPosition);
            if (currentTile == null) return path;

            path.Add(currentTile.transform.position);

            Tile nextTile = currentTile.GetNextTile(direction);
            if (nextTile == null) return path;

            path.Add(nextTile.transform.position);

            Tile previousTile = currentTile;
            currentTile = nextTile;

            while (true)
            {
                nextTile = currentTile.GetNextTile(previousTile);
                if (nextTile == null) break;

                path.Add(nextTile.transform.position);

                previousTile = currentTile;
                currentTile = nextTile;
            }

            RichLogger.Log($"Path found with {path.Count} tiles");

            return path;
        }
    }
}
