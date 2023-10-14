using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy"   || collision.gameObject.tag == "EnemyProjectile"|| collision.gameObject.tag == "Obstacle")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<HealthController>().TakeDamage(1);;
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
      
    }
}
