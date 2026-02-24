using UnityEngine;
using UnityEngine.Events;

namespace RKNTemplate.Input
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Input/PlayerInput")]
    public class PlayerInputSO : ScriptableObject
    {
        [SerializeField] private UnityEvent _onDown = new UnityEvent();
        [SerializeField] private UnityEvent _onHeld = new UnityEvent();
        [SerializeField] private UnityEvent _onUp = new UnityEvent();
        
        public UnityEvent OnDown => _onDown;
        public UnityEvent OnHeld => _onHeld;
        public UnityEvent OnUp => _onUp;
        
        public void SetDown()
        {
            _onDown?.Invoke();
        }

        public void SetHeld()
        {
            _onHeld?.Invoke();
        }

        public void SetUp()
        {
            _onUp?.Invoke();
        }
    }
}