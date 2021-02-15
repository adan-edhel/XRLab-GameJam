using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public interface IMovement
{
    void Movement(Vector2 value);
}

public class InputController : MonoBehaviour
{
    IMovement i_Movement;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) // GetKeyDown
        {
            i_Movement.Movement(context.action.ReadValue<Vector2>());
        }

        if (context.canceled) // GetKeyUp
        {

        }
    }
}
