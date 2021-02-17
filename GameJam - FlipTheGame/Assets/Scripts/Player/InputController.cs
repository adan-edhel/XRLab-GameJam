using UnityEngine.InputSystem;
using UnityEngine;

public interface IMovement
{
    void Movement(Vector2 value);
    void Jump(bool inverted);
    void CutJump();
}

public interface IGravity
{
    void InvertGravity(bool inverted);
}

public class InputController : MonoBehaviour
{
    public static InputController instance { get; private set; }

    [Header("Conditions")]
    public bool gravityInverted;
    public bool IsGrounded;
    public bool IsJumping;

    IMovement i_Movement;
    IGravity i_Gravity;

    [HideInInspector]
    public Vector3 lastCheckpoint;

    private void Awake()
    {
        instance = this;
        lastCheckpoint = transform.position;
        i_Movement = GetComponent<IMovement>();
        i_Gravity = GetComponent<IGravity>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        i_Movement.Movement(context.action.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded)
            {
                i_Movement.Jump(gravityInverted);
                IsJumping = true;
            }
        }

        if (context.canceled)
        {
            i_Movement.CutJump();
            IsJumping = false;
        }
    }

    public void OnInvertGravity(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded)
            {
                gravityInverted = !gravityInverted;
                i_Gravity.InvertGravity(gravityInverted);
            }
        }
    }

    public void OnReturnToMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneHandler.TransitionScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            lastCheckpoint = collision.transform.position;
            Destroy(collision.gameObject);
        }
    }
}