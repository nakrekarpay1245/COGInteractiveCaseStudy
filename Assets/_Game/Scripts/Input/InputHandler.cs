using UnityEngine;
using _Game.Core;

namespace _Game.Input
{
    public class InputHandler : Singleton<InputHandler>
    {
        [Header("Input Settings")] 
        [SerializeField, Tooltip("Scriptable object for managing player input.")]
        private PlayerInputSO _playerInput;
        
        [SerializeField, Tooltip("Minimum swipe distance to register as swipe.")]
        private float _minSwipeDistance = 50f;

        private bool _isInputLocked = false;
        private Vector2 _startTouchPosition;
        private bool _isSwiping = false;

        public bool IsInputLocked
        {
            get => _isInputLocked;
            private set => _isInputLocked = value;
        }

        private void Update()
        {
            if (_isInputLocked) 
                return;
            
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startTouchPosition = UnityEngine.Input.mousePosition;
                _isSwiping = true;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0) && _isSwiping)
            {
                Vector2 endTouchPosition = UnityEngine.Input.mousePosition;
                DetectSwipe(_startTouchPosition, endTouchPosition);
                _isSwiping = false;
            }
        }

        private void DetectSwipe(Vector2 startPosition, Vector2 endPosition)
        {
            Vector2 swipeDelta = endPosition - startPosition;
            
            if (swipeDelta.magnitude < _minSwipeDistance)
                return;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                    _playerInput.SetSwipeRight();
                else
                    _playerInput.SetSwipeLeft();
            }
            else
            {
                if (swipeDelta.y > 0)
                    _playerInput.SetSwipeUp();
                else
                    _playerInput.SetSwipeDown();
            }
        }

        public void UnlockInput()
        {
            IsInputLocked = false;
        }

        public void LockInput()
        {
            IsInputLocked = true;
        }
    }
}