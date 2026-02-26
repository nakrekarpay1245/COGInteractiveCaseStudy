using UnityEngine;

namespace _Game.CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera _gameCamera;

        public void SetOrthoSize(float orthoSize)
        {
            if (_gameCamera != null)
            {
                _gameCamera.orthographicSize = orthoSize;
            }
        }
    }
}
