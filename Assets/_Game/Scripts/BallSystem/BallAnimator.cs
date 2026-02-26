using System.Collections;
using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace _Game.BallSystem
{
    public class BallAnimator : MonoBehaviour
    {
        [Title("Scale Animation Settings")]
        [SerializeField] private float _horizontalScaleReduction = 0.8f;
        [SerializeField] private float _verticalScaleReduction = 0.8f;
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private Ease _scaleEase = Ease.OutQuad;

        private Vector3 _initialScale;
        private Tween _scaleTween;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        public void AnimateMovement(Vector2 direction)
        {
            _scaleTween?.Kill();

            Vector3 targetScale = _initialScale;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                targetScale.y = _initialScale.y * _horizontalScaleReduction;
            }
            else
            {
                targetScale.x = _initialScale.x * _verticalScaleReduction;
            }

            _scaleTween = transform.DOScale(targetScale, _scaleDuration)
                .SetEase(_scaleEase)
                .SetAutoKill(true)
                .SetLink(gameObject);
        }

        public void ResetScale()
        {
            _scaleTween?.Kill();

            _scaleTween = transform.DOScale(_initialScale, _scaleDuration)
                .SetEase(_scaleEase)
                .SetAutoKill(true)
                .SetLink(gameObject);
        }

        private void OnDestroy()
        {
            _scaleTween?.Kill();
        }
    }
}
