using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{

    public int startingHealth = 100;            // Set the amount of health of this character when game starts
    public int currentHealth;                   // This tracks the current health (and it's made public for easier debugging, could be displayed via HUD later on)
    public int attackDamage = 10;               // The base amount of damage this character deals
    float timeBetweenAttacks = 0.5f;            // The base frequency of attacks
    int dmg_min = 10;                           // Min and Max damage this character can deal
    int dmg_max = 20;

    public GameObject enemy;                    // For testing and showcase purposes, this can be used to manually assign an enemy
    bool enemyrInRange;                         // Checks if enemy is in attack range
    float timer = 0.0f;                         // Simple timer to track if the character can attack again

    void Start()
    {
        currentHealth = startingHealth;         // Set the health
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy)
        {
            enemyrInRange = true;               // Check if enemy is in range and notify if that's the case
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == enemy)
        {
            enemyrInRange = false;              // Notify when character leaves the range of the enemy (to stop attacking)
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && enemyrInRange && currentHealth > 0)
        {
            Attack();                           // Attack if coditions above are met
        }

        if (currentHealth <= 0)
        {

        }
    }

    void Attack()
    {
        timer = 0f;

        if (currentHealth > 0)
        {
            attackDamage = Random.Range(dmg_min, dmg_max);  // Randomise the damage dealt slightly 
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
}
