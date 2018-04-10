using UnityEngine;
using System.Collections;
// This script was originally responsible only for moving but I expanded it to also track health
public enum NPC
{                   // This can be used to pre-assign all NPCs on the map, Hostile enemies will attack, Friendly NPCs will heal
    NOTASSSIGNED,
    FRIENDLY,
    HOSTILE
}

public class Movement : MonoBehaviour
{
    Transform player;                       // Reference to the player's position
    Transform wall;                         // Reference to the position of a wall NPCs can hide behind                
    Transform platform;                     // Reference to the position of a platfrom NPCs can go up on

    public bool followingPlayer = false;    // Toggles the function which tells NPCs to follow the player

    public int health = 100;                // The health you start with
    public int currentHealth;               // This tracks the current health

    int attackDamage = 10;                  // The base amount of damage this character deals
    float timeBetweenAttacks = 0.5f;        // The base frequency of attacks
    public int dmg_min = 10;                // Min and Max damage this character can deal
    public int dmg_max = 20;

    public NPC friendly = NPC.NOTASSSIGNED; // Set this to Friendly in the editor for Friendly AIs or Hostile for Enemies
    public GameObject enemy;                // Manual selection of an enemy for easier testing and to showcase its' use

    public bool healing = false;            // Toggles healing on and off
    public bool enemyrInRange;              // Checks if enemy is in attack range
    float timer = 0.0f;                     // Simple timer to track if the character can attack again

    UnityEngine.AI.NavMeshAgent nav;

    void Awake()                            // Set up references
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currentHealth = health;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit goTo;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out goTo, 100))
            {
                nav.destination = goTo.point;   // Checks for the position of the mouse cursor on the screen and tells the selected characters to move there
            }

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {   // This is a bit cheaty... and I currently don't even use it, but the idea was to have all walls untagged, and then tag them only if I clicked on them
                // the reason for this was to ensure there was only maxium of one object tagged as Wall on the map at all times
                if (hitInfo.transform.gameObject.tag != "Wall" && hitInfo.transform.gameObject.tag != "Player" && hitInfo.transform.gameObject.tag != "EditorOnly"
                    && hitInfo.transform.gameObject.tag != "Stair" && hitInfo.transform.gameObject.tag != "Platform" && hitInfo.transform.gameObject.tag != "AI") 
                {    // Code below tells the selected characters to stop following the player (if they were) and stack up by the wall player clicked on
                    hitInfo.transform.gameObject.tag = "Wall";
                    wall = GameObject.FindGameObjectWithTag("Wall").transform;
                    nav.SetDestination(wall.position);
                    hitInfo.transform.gameObject.tag = "Untagged";
                    followingPlayer = false;
                }
                else if (hitInfo.transform.gameObject.tag == "Player")
                {   // or if Player clicked on himself, this tells all selected characters to follow the Player instead
                    nav.SetDestination(player.position);
                    followingPlayer = true;
                }
                else if (hitInfo.transform.gameObject.tag == "Platform" || hitInfo.transform.gameObject.tag == "Stair")
                {   // this will tell the selected characters to climb up the stairs and stack up on a platform
                    platform = GameObject.FindGameObjectWithTag("Platform").transform;
                    nav.SetDestination(platform.position);
                    followingPlayer = false;
                }
                else if (hitInfo.transform.gameObject.tag == "EditorOnly")
                {
                    // do nothing
                }
            }
        }

        if (followingPlayer == true) // Updates the position of the target
        {
            if (this.friendly == NPC.FRIENDLY)
            {
                nav.SetDestination(player.position);
            }
        }

        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && enemyrInRange && currentHealth > 0)
        {
            Attack();              // If the conditions above are met, call the Attack function
        }

        if (healing == true)        // Calls the Heal function and ensures character cannot be over-healed
        {
            Heal();
            if (currentHealth > 100)
            {
                currentHealth = 100;
            }
        }
    }

// This function is currently a bit arbitrary as it does not link with the target, but this is not the objective of this project so I will leave it like this for now
    void Attack()
    {
        timer = 0f;                                           // Reset the timer

        if (currentHealth > 0)
        {
            attackDamage = Random.Range(dmg_min, dmg_max);    // Randomise the damage dealt slightly 
            TakeDamage(attackDamage);
        }
    }

    void TakeDamage(int attackDamage)
    {
        currentHealth -= attackDamage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);                            // Kill the character when health reaches 0
        }
    }

    void Heal()
    {
        if (currentHealth <= 100)
        {
            currentHealth++;                                // Restore characters health each tick
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Wall")
        {
            healing = true;                                 // Enable healing if character is by the wall
        }

        if (other.gameObject.tag == "Platform")
        {
            dmg_min = 20;                                   // Increase the minimum damage if character has high-ground
        }

        if (other.gameObject.name == "Cover")
        {
            dmg_max = 30;                                   // Increase the maximum damage if character is in cover
        }

        if (other.gameObject == enemy)
        {
            enemyrInRange = true;                           // Send signal that the enemy is in range
        }
    }

    void OnTriggerExit(Collider other)                      // Restore defaults
    {
        if (other.gameObject.name == "Wall")
        {
            healing = false;
        }

        if (other.gameObject.tag == "Platform")
        {
            attackDamage = 10;
        }

        if (other.gameObject.name == "Cover")
        {
            dmg_max = 20;
        }

        if (other.gameObject == enemy)
        {
            enemyrInRange = false;
        }
    }
}
