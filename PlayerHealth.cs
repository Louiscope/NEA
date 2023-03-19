using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Setting health and damage values -- Calling FinishLevel.cs
    public float health;
    public float EnemyDamage = 2f;
    public float SpikeDamage = 1f;
    FinishLevel FL;

    void Start()
    {
        FL = GameObject.FindGameObjectWithTag("Fin").GetComponent<FinishLevel>();
    }

    // Checker for when the player enters a trigger -- Finds the tag and determines damage done and damage type -- When health reaches 0 due to damage type, death counter is increase from FinishLevel.cs
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            health -= EnemyDamage;
            if (health <= 0)
            {
                FL.AddDeath(true);
                health = 5;
            }
        }
        else if (other.tag == "Spike")
        {
            health -= SpikeDamage;
            if (health <= 0)
            {
                FL.AddDeath(false);
                health = 5;
            }
        }
    }
}
