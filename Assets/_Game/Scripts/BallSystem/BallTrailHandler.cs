using _Game.ColorSystem;
using _Game.LevelSystem;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallTrailHandler : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        void Awake()
        {
            if (_trailRenderer == null)
            {
                _trailRenderer = GetComponent<TrailRenderer>();
            }
        }

        public void SetPaintColor(ColorType colorType)
        {
            if (_trailRenderer == null) return;

            Color color = LevelManager.Instance.ColorManager.GetColor(colorType);
            _trailRenderer.startColor = color;
            _trailRenderer.endColor = color;
        }
    }
}
