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
        transform.position = InputController.instance.lastCheckpoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-100, respawnThresholds[0], 0), new Vector3(100, respawnThresholds[0], 0));
        Gizmos.DrawLine(new Vector3(-100, respawnThresholds[1], 0), new Vector3(100, respawnThresholds[1], 0));
    }
}
