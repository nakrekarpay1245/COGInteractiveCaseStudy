using UnityEngine;
using TriInspector;

namespace _Game.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Title("Level Configuration")]
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2, 2);
        [SerializeField] private float _ballSpeed = 5f;
        [SerializeField] private ColorType _levelColor = ColorType._0None;
        
        public Vector2Int GridSize { get => _gridSize; private set => _gridSize = value; }
        public float BallSpeed { get => _ballSpeed; private set => _ballSpeed = value; }
        public ColorType LevelColor { get => _levelColor; private set => _levelColor = value; }
    }
}
