using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    InputSystem_Actions controls;
    public Vector2 MoveVector { get; private set; }

    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }
    float maxTimeToCombo = 1.2f;
    float currentTimerToCombo;

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;

    void Start()
    {
        currentTimerToCombo = maxTimeToCombo;
        controls = new InputSystem_Actions();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    void OnDestroy()
    {
        controls.Player.Disable();
    }

    

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log(currentTimerToCombo);
        //currentTimerToCombo -= Time.deltaTime;
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }

        //currentTimerToCombo = maxTimeToCombo;
        //if()
    }

    /*
        public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }
     */

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    /*
      public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        DodgeEvent?.Invoke();
    }
     */

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();

        Debug.Log(MoveVector);
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }

  
}
