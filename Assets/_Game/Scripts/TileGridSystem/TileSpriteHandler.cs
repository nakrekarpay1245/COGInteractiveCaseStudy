using _Game.ColorSystem;
using _Game.LevelSystem;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class TileSpriteHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _default;
        [SerializeField] private SpriteRenderer _paint;

        public void Initialize()
        {
            ResetSpriteStates();
        }

        private void ResetSpriteStates()
        {
            _default.enabled = true;
            _paint.enabled = false;
        }

        public void SetPaintColor(ColorType colorType)
        {
            _paint.color = LevelManager.Instance.ColorManager.GetColor(colorType);
        }

        public void Paint()
        {
            _default.enabled = false;
            _paint.enabled = true;
        }
    }
}
