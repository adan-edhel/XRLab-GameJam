using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem[] EndParticles;

    float waitDuration = 1f;
    float timer;

    bool startCountdown;

    private void Update()
    {
        if (startCountdown == false) return;

        timer += Time.deltaTime;

        if (timer > waitDuration)
        {
            foreach (SpriteRenderer spriteRend in GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRend.enabled = true;
            }
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EndParticles.Length > 0)
            {
                for (int i = 0; i < EndParticles.Length; i++)
                {
                    EndParticles[i].Play();
                }
            }
            else
            {
                Debug.Log($"No particles assigned to the {name}");
            }

            if (anim != null)
            {
                anim.SetBool("Victory", true);
            }

            InputController.instance.enableInputInvert = true;
            startCountdown = true;
        }
    }
}
