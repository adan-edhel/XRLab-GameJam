using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem[] EndParticles;

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
        }
    }
}
