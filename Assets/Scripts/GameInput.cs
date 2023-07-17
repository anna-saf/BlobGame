using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance { get; private set; }

    public FixedJoystick _fixedJoystick;

    public PlayerInputActions _inputActions;

    public event EventHandler OnJump; 
    public event EventHandler<MovementVectorEventArgs> OnMove;
    public class MovementVectorEventArgs : EventArgs
    {
        public Vector2 movementVector;
    }

    private void Awake()
    {
        Instance = this;
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Jump.performed += Jump_performed;
        
    }

    private void Update()
    {
        Vector2 movementVector = GetMovementVectorNormalized();
        if(movementVector != Vector2.zero || true)
        {
            OnMove?.Invoke(this, new MovementVectorEventArgs { movementVector = movementVector });
        }
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    /*public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = new Vector2(0,0);

        if (_fixedJoystick.Horizontal > 0)
        {
            inputVector.x = 1;
        }
        if (_fixedJoystick.Horizontal < 0)
        {
            inputVector.x = -1;
        }
        if (_fixedJoystick.Horizontal == 0)
        {
            inputVector.x = 0;
        }

       
        inputVector = inputVector.normalized;

        return inputVector;
    }*/





   
}
