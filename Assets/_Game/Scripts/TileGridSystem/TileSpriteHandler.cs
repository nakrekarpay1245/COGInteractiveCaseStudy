using _Game.ColorSystem;
using _Game.LevelSystem;
using DG.Tweening;
using UnityEngine;

namespace _Game.TileGridSystem
{
    public class TileSpriteHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _default;
        [SerializeField] private SpriteRenderer _paint;
        [SerializeField] private float _paintDuration = 0.2f;
        [SerializeField] private float _paintScale = 0.6f;
        [SerializeField] private Ease _paintEase = Ease.OutBack;

        private ColorType _colorType;

        public void Initialize()
        {
            ResetSpriteStates();
        }

        private void ResetSpriteStates()
        {
            _default.transform.localScale = Vector3.one * _paintScale;
            _paint.transform.localScale = Vector3.zero;
        }

        public void SetPaintColor(ColorType colorType)
        {
            _colorType = colorType;
            _paint.color = LevelManager.Instance.ColorManager.GetColor(colorType);
        }

        public void Paint()
        {
            _paint.transform.DOScale(_paintScale, _paintDuration).SetEase(_paintEase).SetLink(gameObject).SetAutoKill(true);
        }
    }
}
