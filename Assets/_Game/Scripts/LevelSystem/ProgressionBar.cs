using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.LevelSystem
{
    public class ProgressionBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _tweenDuration = 0.3f;

        private Tween _progressTween;
        private float _lastProgress;

        public void Initialize(Color fillColor)
        {
            if (_fillImage != null)
            {
                _fillImage.color = fillColor;
            }

            if (_slider != null)
            {
                _slider.value = 0f;
            }
        }

        private void Update()
        {
            if (LevelManager.Instance?.ProgressionManager == null || _slider == null) return;

            float targetProgress = LevelManager.Instance.ProgressionManager.CurrentProgress;

            if (Mathf.Abs(targetProgress - _lastProgress) > 0.001f)
            {
                UpdateProgressBar(targetProgress);
                _lastProgress = targetProgress;
            }
        }

        private void UpdateProgressBar(float targetProgress)
        {
            _progressTween?.Kill();

            _progressTween = DOTween.To(() => _slider.value, x => _slider.value = x, targetProgress, _tweenDuration)
                .SetAutoKill(true)
                .SetLink(gameObject);
        }

        private void OnDestroy()
        {
            _progressTween?.Kill();
        }
    }
}
