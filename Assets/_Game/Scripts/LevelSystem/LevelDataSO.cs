using System.Collections.Generic;
using _Game.ColorSystem;
using UnityEngine;
using TriInspector;

namespace _Game.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Title("Level Configuration")]
        [Header("Grid Configuration")]
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2, 2);
        [Header("Ball Configuration")]
        [SerializeField] private float _ballSpeed = 5f;
        [Header("Paint Configuration")]
        [SerializeField] private ColorType _levelColor = ColorType._0None;
        [Header("Camera Settings")]
        [SerializeField] private float _cameraOrthoSize = 5f;
        [Header("Floor Settings")]
        [SerializeField] private Vector2 _floorSize = new Vector2(10f, 10f);
        
        [Title("Positions")]
        [SerializeField] private List<Vector2Int> _ballPositions;
        [SerializeField] private List<Vector2Int> _obstaclePositions;
        
        public Vector2Int GridSize { get => _gridSize; private set => _gridSize = value; }
        public float BallSpeed { get => _ballSpeed; private set => _ballSpeed = value; }
        public ColorType LevelColor { get => _levelColor; private set => _levelColor = value; }
        public float CameraOrthoSize { get => _cameraOrthoSize; private set => _cameraOrthoSize = value; }
        public Vector2 FloorSize { get => _floorSize; private set => _floorSize = value; }
        public List<Vector2Int> BallPositions { get => _ballPositions; }
        public List<Vector2Int> ObstaclePositions { get => _obstaclePositions; }
    }
}
