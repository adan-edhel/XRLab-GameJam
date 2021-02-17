using UnityEngine;



public class Abilities : MonoBehaviour, IGravity
{
    Rigidbody2D rb;
    SpriteRenderer spriteRend;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void InvertGravity(bool inverted)
    {
        if (inverted)
        {
            rb.gravityScale = -1;
            spriteRend.flipY = true;

            CameraHandler.Instance.FlipCamera(true, spriteRend.flipX);
        }
        else
        {
            rb.gravityScale = 1;
            spriteRend.flipY = false;
            CameraHandler.Instance.FlipCamera(false);
        }
    }
}
