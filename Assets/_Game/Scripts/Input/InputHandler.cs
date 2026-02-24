using UnityEngine;
using _Game.Core;

namespace _Game.Input
{
    public class InputHandler : Singleton<InputHandler>
    {
        [Header("Input Settings")] [SerializeField, Tooltip("Scriptable object for managing player input.")]
        private PlayerInputSO _playerInput;

        private bool _isInputLocked = false;

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
            if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                HandleDown();
            }
            else if (UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetKey(KeyCode.Space))
            {
                HandleHeld();
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0) || UnityEngine.Input.GetKeyUp(KeyCode.Space))
            {
                HandleUp();
            }
        }

        private void HandleDown()
        {
            _playerInput.SetDown();
        }

        private void HandleHeld()
        {
            _playerInput.SetHeld();
        }

        private void HandleUp()
        {
            _playerInput.SetUp();
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