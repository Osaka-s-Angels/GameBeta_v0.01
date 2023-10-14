using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "PlayerProjectile")
        {
             if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<HealthController>().TakeDamage(1);
                Destroy(gameObject);
            }
            else {
                Destroy(gameObject);
            }
            
        }
    }
}