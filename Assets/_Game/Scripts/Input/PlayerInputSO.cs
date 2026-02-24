using UnityEngine;
using UnityEngine.Events;

namespace _Game.Input
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject
    {
        [SerializeField] private UnityEvent _onSwipeLeft = new UnityEvent();
        [SerializeField] private UnityEvent _onSwipeRight = new UnityEvent();
        [SerializeField] private UnityEvent _onSwipeUp = new UnityEvent();
        [SerializeField] private UnityEvent _onSwipeDown = new UnityEvent();
        
        public UnityEvent OnSwipeLeft => _onSwipeLeft;
        public UnityEvent OnSwipeRight => _onSwipeRight;
        public UnityEvent OnSwipeUp => _onSwipeUp;
        public UnityEvent OnSwipeDown => _onSwipeDown;
        
        public void SetSwipeLeft()
        {
            _onSwipeLeft?.Invoke();
        }

        public void SetSwipeRight()
        {
            _onSwipeRight?.Invoke();
        }

        public void SetSwipeUp()
        {
            _onSwipeUp?.Invoke();
        }

        public void SetSwipeDown()
        {
            _onSwipeDown?.Invoke();
        }
    }
}