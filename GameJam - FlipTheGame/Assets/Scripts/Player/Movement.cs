using UnityEngine;



public class Movement : MonoBehaviour, IMovement
{
    [Header("Values")]
    public float MoveSpeed = 5f;
    public float JumpForce = 8f;

    [Header("References")]
    public LayerMask surfaceLayer;
    SpriteRenderer spriteRend;
    Collider2D coll;
    Rigidbody2D rb;

    // Movement Adjusters
    float fCutJumpHeight = .5f;
    float slopeRayHeight;

    // Ground Collision
    bool[] groundCollision = new bool[3];
    private float groundColliderSize = .05f;
    private float groundCollidersOffset;
    private float groundHeightOffset;
    float surfaceCheckDelayValue = .2f;
    float surfaceCheckDelay;
    float cameraShakeVelocity = 11;

    // Movement Input Value
    [HideInInspector] public Vector2 moveInputValue;

    float originalFrictionValue = 50;
    Vector2 velocity;
    Vector2 oldVelocity;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        CalculateColliderValues();

        rb.sharedMaterial.friction = originalFrictionValue;
        rb.sharedMaterial = rb.sharedMaterial;
    }

    private void Update()
    {
        CheckForGroundCollision();

        velocity.x = moveInputValue.x * MoveSpeed;

        if (Time.frameCount % 5 == 0)
        {
            oldVelocity = rb.velocity;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    #region Movement
    /// <summary>
    /// Handles player movement and jumping using input interfaces
    /// </summary>
    public void HandleMovement()
    {
        if (moveInputValue.x * System.Math.Sign(moveInputValue.x) > 0.01f)
        {
            rb.velocity = new Vector2(moveInputValue.x * MoveSpeed, rb.velocity.y);
        }
    }
    #endregion

    #region Jump
    void IMovement.Jump(bool inverted)
    {
        if (inverted)
        {
            rb.velocity = new Vector2(rb.velocity.x, -JumpForce * 2);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * 2);
        }
    }

    /// <summary>
    /// Cuts jumps in half if input is released
    /// </summary>
    void IMovement.CutJump()
    {
        if (rb.velocity.y != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
        }
    }
    #endregion

    #region collisions
    /// <summary>
    /// Checks for ground colliders at the base of player
    /// </summary>
    void CheckForGroundCollision()
    {
        if (InputController.instance.gravityInverted)
        {
            groundCollision[0] = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCollidersOffset, transform.position.y + groundHeightOffset), groundColliderSize, surfaceLayer);
            groundCollision[1] = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + groundHeightOffset), groundColliderSize, surfaceLayer);
            groundCollision[2] = Physics2D.OverlapCircle(new Vector2(transform.position.x - groundCollidersOffset, transform.position.y + groundHeightOffset), groundColliderSize, surfaceLayer);
        }
        else
        {
            groundCollision[0] = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCollidersOffset, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);
            groundCollision[1] = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);
            groundCollision[2] = Physics2D.OverlapCircle(new Vector2(transform.position.x - groundCollidersOffset, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);
        }

        // surface check countdown
        surfaceCheckDelay -= Time.deltaTime;

        // Adjust friction according to collisions
        if (groundCollision[1] == true)
        {
            rb.sharedMaterial.friction = originalFrictionValue;
        }
        else
        {
            rb.sharedMaterial.friction = 0;
        }
        rb.sharedMaterial = rb.sharedMaterial;

        // Set base grounded state using each ground collider
        for (int i = 0; i < groundCollision.Length; i++)
        {
            if (groundCollision[i])
            {
                surfaceCheckDelay = surfaceCheckDelayValue;
                InputController.instance.IsGrounded = true;

                return;
            }

            if (surfaceCheckDelay < 0 && !groundCollision[i])
            {
                InputController.instance.IsGrounded = false;
            }
        }

        // Reset delay if player jumps
        if (InputController.instance.IsJumping)
        {
            surfaceCheckDelay = -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ground Impact Camera Shake
        if (InputController.instance.gravityInverted)
        {
            if (oldVelocity.y > cameraShakeVelocity)
            {
                if (oldVelocity.y > cameraShakeVelocity * 2)
                {
                    CameraHandler.Instance.ShakeCamera(4, 6);
                }
                else
                {
                    CameraHandler.Instance.ShakeCamera(2, 3);
                }
            }
        }
        else
        {
            if (oldVelocity.y < -cameraShakeVelocity)
            {
                if (oldVelocity.y < -cameraShakeVelocity * 2)
                {
                    CameraHandler.Instance.ShakeCamera(4, 6);
                }
                else
                {
                    CameraHandler.Instance.ShakeCamera(2, 3);
                }
            }
        }

    }
    #endregion

    #region GetValues
    /// <summary>
    /// Calculates the essential values for the ground check colliders
    /// </summary>
    private void CalculateColliderValues()
    {
        if (coll)
        {
            slopeRayHeight = coll.bounds.extents.y;
            groundHeightOffset = coll.bounds.extents.y;
            groundCollidersOffset = coll.bounds.size.x / 3;
        }
    }
    void IMovement.Movement(Vector2 value)
    {
        moveInputValue = value;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (spriteRend && InputController.instance != null)
        {
            if (InputController.instance.gravityInverted)
            {
                Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCollidersOffset, transform.position.y + groundHeightOffset, -2), groundColliderSize);
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + groundHeightOffset, -2), groundColliderSize);
                Gizmos.DrawWireSphere(new Vector3(transform.position.x - groundCollidersOffset, transform.position.y + groundHeightOffset, -2), groundColliderSize);
            }
            else
            {
                Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCollidersOffset, transform.position.y - groundHeightOffset, -2), groundColliderSize);
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundHeightOffset, -2), groundColliderSize);
                Gizmos.DrawWireSphere(new Vector3(transform.position.x - groundCollidersOffset, transform.position.y - groundHeightOffset, -2), groundColliderSize);
            }

        }

        if (moveInputValue.x != 0)
        {
            Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - slopeRayHeight), Vector2.right * Mathf.Sign(velocity.x));
        }
    }
    #endregion

    private void OnDisable()
    {
        rb.sharedMaterial.friction = originalFrictionValue;
    }
}
