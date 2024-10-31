using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    public event Action JumpEvent;
    public event Action AttackEvent;
    public event Action RopeEvent;

    public Vector2 Movement { get; private set; }
    
    private Controls _controls;

    private void OnEnable() {
        if(_controls == null) {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }

        _controls.Player.Enable();
    }

    private void OnDisable() {
        _controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) {
        if(context.performed) JumpEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if(context.performed) AttackEvent?.Invoke();
    }

    public void OnRope(InputAction.CallbackContext context) {
        if(context.performed) RopeEvent?.Invoke();
    }
}
