using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class GameInputDelegator : MonoBehaviour, InputActions.IGameplayActions
    {
        public static event Action<bool> InputLockChanged;
        public static event Action<Vector2> OnMovementChanged;
        public static event Action<bool> OnJumpPressed;
    
        public static event Action<bool> OnLeftClick;
        public static event Action<bool> OnRightClick;

        public static event Action<Vector2> OnMouseMove;

        public static bool LockInputs { get; private set; }

        private Vector2 _currentInput;
    
        //============================================================================================================//\

        private void OnEnable()
        {
            Inputs.Input.Gameplay.Enable();
            Inputs.Input.Gameplay.AddCallbacks(this);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }
    
        private void OnDisable()
        {
            Inputs.Input.Gameplay.Disable();
            Inputs.Input.Gameplay.RemoveCallbacks(null);
        }

        //Lock Input
        //============================================================================================================//
        
        public static void SetInputLock(bool lockState)
        {
            LockInputs = lockState;
            
            InputLockChanged?.Invoke(lockState);
        }

        //============================================================================================================//

        public void OnHorizontalMovement(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                _currentInput = Vector2.zero;
                OnMovementChanged?.Invoke(_currentInput);
                return;
            }
            
            var x = context.ReadValue<float>();

            _currentInput.x = x;
            OnMovementChanged?.Invoke(_currentInput);

        }

        public void OnVerticalMovement(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                _currentInput = Vector2.zero;
                OnMovementChanged?.Invoke(_currentInput);
                return;
            }
            
            var y = context.ReadValue<float>();

            _currentInput.y = y;
            OnMovementChanged?.Invoke(_currentInput);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (LockInputs) return;
            
            if(!context.performed) return;
            
            var pressed = context.ReadValueAsButton();
            OnJumpPressed?.Invoke(pressed);
        }

        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            if (LockInputs)
            {
                OnLeftClick?.Invoke(false);
                return;
            }
            
            var pressed = context.ReadValueAsButton();
            OnLeftClick?.Invoke(pressed);
        }

        public void OnMouseRightClick(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            if (LockInputs)
            {
                OnRightClick?.Invoke(false);
                return;
            }
            
            var pressed = context.ReadValueAsButton();
            OnRightClick?.Invoke(pressed);
        }

        public void OnMouseLook(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            var delta = context.ReadValue<Vector2>();
            OnMouseMove?.Invoke(delta);
        }

        //============================================================================================================//
    }
}
