using _Game.Utilities;
using UnityEngine;

namespace _Game.Input
{
    public class SwipeInputTester : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO _playerInput;

        private void OnEnable()
        {
            _playerInput.OnSwipeLeft.AddListener(OnSwipeLeft);
            _playerInput.OnSwipeRight.AddListener(OnSwipeRight);
            _playerInput.OnSwipeUp.AddListener(OnSwipeUp);
            _playerInput.OnSwipeDown.AddListener(OnSwipeDown);
        }

        private void OnDisable()
        {
            _playerInput.OnSwipeLeft.RemoveListener(OnSwipeLeft);
            _playerInput.OnSwipeRight.RemoveListener(OnSwipeRight);
            _playerInput.OnSwipeUp.RemoveListener(OnSwipeUp);
            _playerInput.OnSwipeDown.RemoveListener(OnSwipeDown);
        }

        private void OnSwipeLeft()
        {
            RichLogger.Log("Swipe LEFT detected");
        }

        private void OnSwipeRight()
        {
            RichLogger.Log("Swipe RIGHT detected");
        }

        private void OnSwipeUp()
        {
            RichLogger.Log("Swipe UP detected");
        }

        private void OnSwipeDown()
        {
            RichLogger.Log("Swipe DOWN detected");
        }
    }
}
