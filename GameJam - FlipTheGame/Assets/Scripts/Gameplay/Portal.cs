using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Scriptable Object
    [SerializeField] PortalData portalType;

    // Closest Cinemachine Confiner
    PolygonCollider2D closestConfiner;

    // List of connected portals
    List<Portal> connectedPortals = new List<Portal>();

    // Layers to check collision with
    [SerializeField] LayerMask aliveEntities;

    public Portal destinationPortal;

    public bool isActivatable;
    public bool isRepeatable;

    private void Awake()
    {
        InputController.instance.PortalRegistry(this);
    }

    private void Start()
    {

        // Assign portals of the same type to Connected Portals list
        AssignConnectedPortals();

        // Assign closest Cinemachine Virtualcam Confiner
        closestConfiner = AssignClosestConfiner();

        // Stop all particles
        foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Stop();
        }
    }

    PolygonCollider2D AssignClosestConfiner()
    {
        // Save all objects with "Confiner" tag in an array
        PolygonCollider2D[] allConfiners = FindObjectsOfType<PolygonCollider2D>();

        if (allConfiners.Length == 0) return null;

        float shortestDistance = Vector3.Distance(allConfiners[0].transform.position, transform.position);
        PolygonCollider2D nearestConfiner = allConfiners[0];

        // Loop through all confiner objects and assign the closest one to a variable
        foreach (var confiner in allConfiners)
        {
            var pos = confiner.transform.position;
            var dist = Vector3.Distance(pos, transform.position);

            if (dist <= shortestDistance)
            {
                nearestConfiner = confiner;
            }
        }

        // Return the collider of the nearest confiner
        return nearestConfiner;
    }

    void AssignConnectedPortals()
    {
        // Save Portals that have the same assigned color into Connected Portals list
        for (int i = 0; i < InputController.instance.PortalList.Count; i++)
        {
            if (InputController.instance.PortalList[i].Equals(this))
            {
                continue;
            }

            if (InputController.instance.PortalList[i].portalType.color == portalType.color)
            {
                connectedPortals.Add(InputController.instance.PortalList[i]);
            }
        }

        // Warn if no portals are connected to current portal
        if (destinationPortal == null)
        {
            isActivatable = false;
            Debug.Log($"No portals connected to {gameObject.name} in {transform.root.name}.");
        }

        // Warn if multiple portals are connected to current portal
        if (connectedPortals.Count > 1)
        {
            //Debug.Log($"Multiple portals connected to {gameObject.name} in {transform.root.name}. This will randomize its destination.");
        }
    }

    /// <summary>
    /// Selects a connected portal to send the player to
    /// </summary>
    /// <returns></returns>
    Transform SelectPortalToTeleportTo()
    {
        //return connectedPortals[Random.Range(0, connectedPortals.Count)].transform;

        return destinationPortal?.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collision is not an alive entity, return.
        if (!aliveEntities.Includes(collision.transform.gameObject.layer)) return; // Mort - custom extension method!

        if (isActivatable || isRepeatable)
        {
            // Play all particles
            foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
            {
                particleSystem.Play();
            }
        }

        // If collision has player tag
        if (collision.transform.CompareTag("Player"))
        {   // Update Cinemachine Confiner
            CameraHandler.Instance.UpdateConfiner(closestConfiner);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the portal is not activatable nor repeatable, return.
        if (!isActivatable && !isRepeatable) return;

        if (!InputController.instance.Interacting) return;

        if (destinationPortal == null) return;

        // If colliding object has player tag
        if (collision.transform.CompareTag("Player"))
        {
            // Set player teleportation state
            InputController.instance.Teleporting = true;

            // Set player teleportation destination
            InputController.instance.Teleport(SelectPortalToTeleportTo().position);

            // Play teleportation audio
            //AudioManager.PlaySound(AudioManager.Sound.TeleportDissolve, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If there are any alive entities inside, return.
        if (Physics2D.OverlapCircle(transform.position, GetComponent<CircleCollider2D>().radius, aliveEntities))
        {
            return;
        }

        // Stop all particles
        foreach (var particleSystem in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Stop();
        }
    }

    private void OnDestroy()
    {
        // Remove this from the list of portals once destroyed
        InputController.instance.PortalRegistry(this);
    }
}