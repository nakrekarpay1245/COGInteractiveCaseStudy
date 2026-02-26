using System;
using UnityEngine;

namespace _Game.ColorSystem
{
    [Serializable]
    public class ColorTexturePair
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Texture _particleTexture;

        public ColorType ColorType { get => _colorType; }
        public Texture ParticleTexture { get => _particleTexture; }
    }
}
