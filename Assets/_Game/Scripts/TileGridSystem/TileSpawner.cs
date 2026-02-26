using UnityEditor;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class TileSpawner : MonoBehaviour
    {
        [SerializeField]
        private Tile _tilePrefab;

        public Tile SpawnTile(Vector2 position, Transform parentTransform)
        {
            Tile tile = Instantiate(_tilePrefab, position, Quaternion.identity, parentTransform);
            return tile;
        }

        public Tile SpawnTileInEditor(Vector2 position, Transform parentTransform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject tileGO = PrefabUtility.InstantiatePrefab(_tilePrefab.gameObject, parentTransform) as GameObject;
                tileGO.transform.position = position;
                tileGO.transform.rotation = Quaternion.identity;

                Tile tile = tileGO.GetComponent<Tile>();
                return tile;
            }
#endif
            return null;
        }
    }
}