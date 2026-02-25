using _Game.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Toggle _muteToggle;

        private void Start()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(Close);

            if (_muteToggle != null)
                _muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnMuteToggleChanged(bool isMuted)
        {
            if (isMuted)
                AudioManager.Instance.Mute();
            else
                AudioManager.Instance.UnMute();
        }

        public void Mute()
        {
            if (_muteToggle != null)
                _muteToggle.isOn = true;
        }

        public void UnMute()
        {
            if (_muteToggle != null)
                _muteToggle.isOn = false;
        }
    }
}
