using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace _Game.UI
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;

        private void Start()
        {
            if (_retryButton != null)
                _retryButton.onClick.AddListener(Retry);
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
            Close();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
