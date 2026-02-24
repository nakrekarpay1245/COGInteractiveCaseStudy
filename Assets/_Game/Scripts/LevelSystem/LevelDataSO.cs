using UnityEngine;
using TriInspector;

namespace _Game.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Title("Level Configuration")]
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2, 2);
        
        public Vector2Int GridSize { get => _gridSize; private set => _gridSize = value; }
    }
}
