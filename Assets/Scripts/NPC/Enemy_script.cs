using UnityEngine;
using System.Collections;
using UnityEditor;

public class Enemy_script : MonoBehaviour
{
    Transform player;                           // Sets player as the target and stores his position
    UnityEngine.AI.NavMeshAgent nav;            // Needed for the AI to move around the nav mesh
    RaycastHit hitInfo = new RaycastHit();      // Stores info on the gameobject raycast hit (via mouse click)
    public float fov = 60f;                     // Field-of-View of the character
    public float sightDist = 10.0f;             // Distance at which the character can see
    public float heightMultiplier = 0.5f;       // This simply moves the "eyes" of the character above the ground-level
    bool CanSee = false;                        // This, combined with the three float variables above, tell if the character can see the target
    Transform point_A;                          // Basic way to store 2 points on the floor...
    Transform point_B;                          // ... which are used to set up a patrol route
    bool pointA = false;                        // This is used to ensure the character comes back to it's patrol route

    void Awake()                                // Set up references
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        point_A = GameObject.FindGameObjectWithTag("Point A").transform;
        point_B = GameObject.FindGameObjectWithTag("Point B").transform;
    }


    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);      // Used to see if player enters the FoV of the enemy

        var rotationRight = Quaternion.AngleAxis(fov, transform.up) * transform.forward;    // Right border of the cone of vision
        var rotationLeft = Quaternion.AngleAxis(-fov, transform.up) * transform.forward;    // Left border of the cone of vision

        //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, rotationRight * sightDist, Color.cyan); // right
        //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, rotationLeft * sightDist, Color.cyan); // left

        // Code below checks if the character can see its enemy (the Player)
        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, direction.normalized, out hitInfo, sightDist))
        {
            if (hitInfo.collider.gameObject.tag == "Player")
            {
                if (angle < fov)
                {
                    CanSee = true;
                }
            }
            else if (hitInfo.collider.gameObject.tag != "Player")
            {
                CanSee = false;
            }
        }

        if (CanSee == true)                         // if it can, start chasing it
        {
            nav.SetDestination(player.position);
        }
        else if (CanSee == false)
        {
            if (pointA == false)                    // else, keep patrolling
            {
                nav.SetDestination(point_B.position);
            }
            else
            {
                nav.SetDestination(point_A.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)           // this ensures the character goes from point A to B
    {
        if (other.gameObject.tag == ("Point A"))
        {
            pointA = false;
        }
        else if (other.gameObject.tag == ("Point B"))
        {
            pointA = true;
        }
    }
}
