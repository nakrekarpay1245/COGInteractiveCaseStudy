using System.Collections.Generic;
using _Game.ColorSystem;
using _Game.LevelSystem;
using TriInspector;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallSpriteHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<ColorSpritePair> _colorSpritePairs;
        [SerializeField, ReadOnly] private ColorType _colorType;

        void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        public void SetPaintColor(ColorType colorType)
        {
            if (_spriteRenderer == null) return;

            _colorType = colorType;

            foreach (ColorSpritePair pair in _colorSpritePairs)
            {
                if (pair.ColorType == _colorType)
                {
                    _spriteRenderer.sprite = pair.PaintSprite;
                    break;
                }
            }
        }
    }
}
