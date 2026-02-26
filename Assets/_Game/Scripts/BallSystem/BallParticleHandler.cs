using _Game.ColorSystem;
using _Game.LevelSystem;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallParticleHandler : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        void Awake()
        {
            if (_particleSystem == null)
            {
                _particleSystem = GetComponent<ParticleSystem>();
            }
        }

        public void SetPaintColor(ColorType colorType)
        {
            if (_particleSystem == null) return;

            ParticleSystem.MainModule mainModule = _particleSystem.main;
            mainModule.startColor = LevelManager.Instance.ColorManager.GetColor(colorType);
        }
    }
}
