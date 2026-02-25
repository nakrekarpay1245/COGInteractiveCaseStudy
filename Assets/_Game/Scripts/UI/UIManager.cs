using _Game.Core;
using _Game.LevelSystem;
using _Game.ProgressionSystem;
using UnityEngine;

namespace _Game.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Play Time UI")]
        [SerializeField] private PlayTimeUI _playTimeUI;
        [SerializeField] private ProgressionBar _progressionBar;
        
        [Header("Menus")]
        [SerializeField] private WinMenu _winMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private RetryMenu _retryMenu;

        private void Start()
        {
            InitializeUI();
            LevelManager.OnWin += ShowWinMenu;
        }

        private void OnDestroy()
        {
            LevelManager.OnWin -= ShowWinMenu;
        }

        private void InitializeUI()
        {
            if (_winMenu != null)
                _winMenu.gameObject.SetActive(false);

            if (_settingsMenu != null)
                _settingsMenu.gameObject.SetActive(false);

            if (_retryMenu != null)
                _retryMenu.gameObject.SetActive(false);

            if (_playTimeUI != null)
                _playTimeUI.gameObject.SetActive(true);
        }

        public void OpenSettingsMenu()
        {
            if (_settingsMenu != null)
                _settingsMenu.Open();
        }

        public void OpenRestartMenu()
        {
            if (_retryMenu != null)
                _retryMenu.Open();
        }

        public void ClosePlayTimeButtons()
        {
            if (_playTimeUI != null)
                _playTimeUI.HideButtons();
        }

        public void CloseProgressBar()
        {
            if (_progressionBar != null)
                _progressionBar.gameObject.SetActive(false);
        }

        public void OpenProgressBar()
        {
            if (_progressionBar != null)
                _progressionBar.gameObject.SetActive(true);
        }

        public void ShowWinMenu()
        {
            if (_winMenu != null)
                _winMenu.Open();
        }
        
        public void ShowRetryMenu()
        {
            if (_retryMenu != null)
                _retryMenu.Open();
        }
    }
}
