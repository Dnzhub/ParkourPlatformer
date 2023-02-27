using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controllers.IPlayerActions
{
    private Controllers controllers;

    public Vector2 MovementValue { get; private set; }
    public event Action JumpEvent;
    public event Action CrouchEvent;
    private void Start()
    {
        controllers = new Controllers();
        
        //Bind actions to this class
        controllers.Player.SetCallbacks(this);

        controllers.Player.Enable();
    }
    private void OnDestroy()
    {
        controllers.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        JumpEvent?.Invoke(); 
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        CrouchEvent?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
       MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLooking(InputAction.CallbackContext context)
    {
       
    }
}
