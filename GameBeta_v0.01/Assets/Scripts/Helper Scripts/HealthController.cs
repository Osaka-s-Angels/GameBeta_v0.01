using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [HideInInspector] public int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}

