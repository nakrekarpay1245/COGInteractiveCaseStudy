using _Game.Audio;
using _Game.ColorSystem;
using PrimeTween;
using System.Collections.Generic;
using TriInspector;
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
        [SerializeField] private List<ColorSpritePair> _colorSpritePairs;

        [SerializeField, ReadOnly] private ColorType _colorType;
        private bool _isPainted;

        [SerializeField] private string _paintAudioKey = "Pop";

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

            foreach (ColorSpritePair pair in _colorSpritePairs)
            {
                if (pair.ColorType == _colorType)
                {
                    _paint.sprite = pair.PaintSprite;
                    break;
                }
            }
        }

        public void Paint()
        {
            if (_isPainted) return;
            
            _isPainted = true;
            AudioManager.Instance.PlayAudio(_paintAudioKey);
            Tween.Scale(_paint.transform, _paintScale, _paintDuration, _paintEase);
        }
    }
}
