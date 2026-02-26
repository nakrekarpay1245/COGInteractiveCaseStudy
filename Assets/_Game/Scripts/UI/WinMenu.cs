using _Game.LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.UI
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;

        private void Start()
        {
            if (_nextButton != null)
                _nextButton.onClick.AddListener(NextLevel);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void NextLevel()
        {
            Close();
            LevelManager.Instance.NextLevel();
        }
    }
}
