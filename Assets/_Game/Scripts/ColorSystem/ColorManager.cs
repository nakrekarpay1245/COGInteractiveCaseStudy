using System.Collections.Generic;
using UnityEngine;

namespace _Game.ColorSystem
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private List<ColorColorTypePair> _colorPairs;

        public Color GetColor(ColorType colorType)
        {
            foreach (ColorColorTypePair pair in _colorPairs)
            {
                if (pair.ColorType == colorType)
                {
                    return pair.Color;
                }
            }

            return Color.white;
        }
    }
}
