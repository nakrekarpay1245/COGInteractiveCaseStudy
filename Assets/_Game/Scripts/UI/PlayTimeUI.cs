using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI
{
    public class PlayTimeUI : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _restartButton;

        private void Start()
        {
            if (_settingsButton != null)
                _settingsButton.onClick.AddListener(() => UIManager.Instance.OpenSettingsMenu());

            if (_restartButton != null)
                _restartButton.onClick.AddListener(() => UIManager.Instance.OpenRestartMenu());
        }

        public void HideButtons()
        {
            if (_settingsButton != null)
                _settingsButton.gameObject.SetActive(false);

            if (_restartButton != null)
                _restartButton.gameObject.SetActive(false);
        }

        public void ShowButtons()
        {
            if (_settingsButton != null)
                _settingsButton.gameObject.SetActive(true);

            if (_restartButton != null)
                _restartButton.gameObject.SetActive(true);
        }
    }
}
