using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacles : MonoBehaviour
{
    private Animator animator;
    private int currentHealth;
    [SerializeField] private int maxHealth = 3;
    

    void Start()
    {
        currentHealth = maxHealth;
        animator=GetComponent<Animator>();
        
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        string phase = "Phase" + (maxHealth - currentHealth);
        if (currentHealth > 0)
        {
            animator.SetBool(phase, true);
        }
        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile" || collision.gameObject.tag == "EnemyProjectile")
        { TakeDamage(1); }
    
    }
}
