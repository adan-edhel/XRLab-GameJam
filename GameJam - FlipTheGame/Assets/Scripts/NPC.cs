using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    SpriteRenderer spriteRend;
    float distanceToPlayer;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, InputController.instance.gameObject.transform.position);
        if (distanceToPlayer < 5f)
        {
            if (transform.position.x < InputController.instance.gameObject.transform.position.x)
            {
                spriteRend.flipX = false;
            }
            else
            {
                spriteRend.flipX = true;
            }
        }
    }
}
