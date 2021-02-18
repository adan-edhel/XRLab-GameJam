using UnityEngine;

public class AnimationHandler : MonoBehaviour, IMovement
{
    float moveInput;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spriteRend;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        UpdateAnimatorValues();
        UpdateSprite();
    }

    void UpdateAnimatorValues()
    {
        anim.SetFloat("XInput", Mathf.Abs(moveInput));
        anim.SetFloat("YVelocity", Mathf.Abs(rb.velocity.y));
        anim.SetBool("Jumping", InputController.instance.IsJumping);
        anim.SetBool("Grounded", InputController.instance.IsGrounded);
        anim.SetBool("Alternate Idle", InputController.instance.AlternateIdle);
        anim.SetBool("Hurt", InputController.instance.Hurt);
    }

    void UpdateSprite()
    {
        if (rb.velocity.x < -0.01)
        {   // turn left
            spriteRend.flipX = true;
        }
        else if (rb.velocity.x > 0.01)
        {   // turn right
            spriteRend.flipX = false;
        }
    }

    public void Movement(Vector2 value)
    {
        moveInput = value.x;
    }

    public void Jump(bool inverted)
    {
        
    }

    public void CutJump()
    {
        
    }
}
