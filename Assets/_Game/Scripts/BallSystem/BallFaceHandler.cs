using System.Collections.Generic;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallFaceHandler : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _ballSprites;
        [SerializeField] private SpriteRenderer _ballSpriteRenderer;

        void Start()
        {
            if (_ballSprites != null && _ballSprites.Count > 0 && _ballSpriteRenderer != null)
            {
                int randomIndex = Random.Range(0, _ballSprites.Count);
                _ballSpriteRenderer.sprite = _ballSprites[randomIndex];
            }
        }
    }
}
