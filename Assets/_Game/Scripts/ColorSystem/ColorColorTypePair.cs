using System;
using UnityEngine;

namespace _Game.ColorSystem
{
    [Serializable]
    public class ColorColorTypePair
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Color _color;

        public ColorType ColorType { get => _colorType; }
        public Color Color { get => _color; }
    }
}
