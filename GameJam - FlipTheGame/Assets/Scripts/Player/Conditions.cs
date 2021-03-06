using UnityEngine;

public class Conditions : MonoBehaviour
{
    float[] respawnThresholds = new float[] {-200f, 200f};

    private void Update()
    {
        if (transform.position.y <= respawnThresholds[0] || transform.position.y >= respawnThresholds[1])
        {
            Respawn();
            Debug.Log($"Character out of world bounds, respawning...");
        }
    }

    private void Respawn()
    {
        InputController.instance.Hurt = true;
        transform.position = InputController.instance.lastCheckpoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        InputController.instance.deathCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-100, respawnThresholds[0], 0), new Vector3(100, respawnThresholds[0], 0));
        Gizmos.DrawLine(new Vector3(-100, respawnThresholds[1], 0), new Vector3(100, respawnThresholds[1], 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Respawn();
        }
    }
}
