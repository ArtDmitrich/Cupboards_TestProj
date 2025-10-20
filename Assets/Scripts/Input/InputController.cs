using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputController: IInputController, IDisposable
    {
        public event Action<Vector2> OnLeftMouseButtonClicked;
    
        private Vector2 _mousePosition;
    
        private readonly Actions _inputAction;

        protected InputController()
        {
            _inputAction = new Actions();
            _inputAction.Enable();
            _inputAction.Mouse.LeftButton.performed += OnClickLeftMouseButton;
        }

        private void OnClickLeftMouseButton(InputAction.CallbackContext context)
            => OnLeftMouseButtonClicked?.Invoke(_inputAction.Mouse.MousePosition.ReadValue<Vector2>());

        public void Dispose()
        {
            _inputAction.Disable();
            _inputAction.Mouse.LeftButton.performed -= OnClickLeftMouseButton;
        }
    }
}
