using UnityEngine;

namespace _Game.FloorSystem
{
    public class FloorManager : MonoBehaviour
    {
        [SerializeField] private Transform _floorTransform;

        public void Initialize(Vector2 floorSize)
        {
            if (_floorTransform != null)
            {
                _floorTransform.localScale = new Vector3(floorSize.x, floorSize.y, 1f);
            }
        }
    }
}
