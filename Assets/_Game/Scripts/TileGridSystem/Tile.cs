using _Game.Utilities;
using TriInspector;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class Tile : MonoBehaviour
    {
        [Title("Tile")]
        [SerializeField, ReadOnly] private Vector2 _gridPosition;

        public Vector2 GridPosition { get => _gridPosition; set => _gridPosition = value; }

        public void Initialize()
        {
            RichLogger.Log($"{name} initialized (Play Mode)!");
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