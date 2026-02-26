using PrimeTween;
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

        [Title("Stop Animation Settings")]
        [SerializeField] private float _stopScaleReduction = 0.9f;
        [SerializeField] private float _stopAnimationDuration = 0.15f;

        private Vector3 _initialScale;
        private Sequence _scaleTween;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        public void AnimateMovement(Vector2 direction)
        {
            _scaleTween.Stop();

            Vector3 targetScale = _initialScale;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                targetScale.y = _initialScale.y * _horizontalScaleReduction;
            }
            else
            {
                targetScale.x = _initialScale.x * _verticalScaleReduction;
            }

            _scaleTween = Sequence.Create(Tween.Scale(transform, targetScale, _scaleDuration, _scaleEase));
        }

        public void ResetScale(Vector2 lastDirection)
        {
            _scaleTween.Stop();

            Vector3 squashScale = _initialScale;

            if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                squashScale.x = _initialScale.x * _stopScaleReduction;
            }
            else
            {
                squashScale.y = _initialScale.y * _stopScaleReduction;
            }

            Sequence stopSequence = Sequence.Create()
                .Chain(Tween.Scale(transform, squashScale, _stopAnimationDuration * 0.5f, Ease.OutQuad))
                .Chain(Tween.Scale(transform, _initialScale, _stopAnimationDuration * 0.5f, Ease.OutQuad));

            _scaleTween = stopSequence;
        }

        private void OnDestroy()
        {
            _scaleTween.Stop();
        }
    }
}
