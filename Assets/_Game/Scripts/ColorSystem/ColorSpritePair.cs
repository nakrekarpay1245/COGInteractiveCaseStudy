using System;
using UnityEngine;

namespace _Game.ColorSystem
{
    [Serializable]
    public class ColorSpritePair
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Sprite _paintSprite;

        public ColorType ColorType { get => _colorType; }
        public Sprite PaintSprite { get => _paintSprite; }
    }
}
