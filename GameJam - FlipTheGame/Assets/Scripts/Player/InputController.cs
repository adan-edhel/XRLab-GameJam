using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

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
    public bool Interacting;
    public bool Teleporting;

    public bool AlternateIdle;

    IMovement[] i_Movement;
    IGravity i_Gravity;

    [HideInInspector]
    public Vector3 lastCheckpoint;
    public Transform teleportDestination;

    public List<Portal> PortalList = new List<Portal>();

    private void Awake()
    {
        instance = this;
        lastCheckpoint = transform.position;
        i_Movement = GetComponents<IMovement>();
        i_Gravity = GetComponent<IGravity>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        for (int i = 0; i < i_Movement.Length; i++)
        {
            if (gravityInverted && CameraHandler.Instance.enableCameraFlipping)
            {
                i_Movement[i].Movement(-context.action.ReadValue<Vector2>());
            }
            else
            {
                i_Movement[i].Movement(context.action.ReadValue<Vector2>());
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded)
            {
                for (int i = 0; i < i_Movement.Length; i++)
                {
                    i_Movement[i].Jump(gravityInverted);
                }
                IsJumping = true;
                AlternateIdle = !AlternateIdle;
            }
        }

        if (context.canceled)
        {
            for (int i = 0; i < i_Movement.Length; i++)
            {
                i_Movement[i].CutJump();
            }
            IsJumping = false;
        }
    }

    public void OnInvertGravity(InputAction.CallbackContext context)
    {
        return;

        if (context.performed)
        {
            if (IsGrounded)
            {
                gravityInverted = !gravityInverted;
                i_Gravity.InvertGravity(gravityInverted);
                IsJumping = true;
                AlternateIdle = !AlternateIdle;
            }
        }
        if (context.canceled)
        {
            IsJumping = false;
        }
    }

    public void OnReturnToMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneHandler.TransitionScene(0);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Interacting = true;
        }
    }

    public void Teleport(Vector3 destination)
    {
        Interacting = false;
        transform.position = new Vector3(destination.x, destination.y, 0);
    }

    public void PortalRegistry(Portal portal)
    {
        if (PortalList.Contains(portal))
        {
            PortalList.Remove(portal);
        }
        else
        {
            PortalList.Add(portal);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            lastCheckpoint = collision.transform.position;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("GravityPoint"))
        {
            gravityInverted = !gravityInverted;
            i_Gravity.InvertGravity(gravityInverted);
        }
    }
}