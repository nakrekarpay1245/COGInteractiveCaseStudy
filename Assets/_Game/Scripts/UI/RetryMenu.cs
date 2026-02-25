using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace _Game.UI
{
    public class RetryMenu : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            if (_retryButton != null)
                _retryButton.onClick.AddListener(Retry);

            if (_closeButton != null)
                _closeButton.onClick.AddListener(Close);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
