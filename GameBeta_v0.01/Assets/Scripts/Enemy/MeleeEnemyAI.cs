using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    #region COMPONENTS
    private Animator anim;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    #endregion

    #region PLAYER DETECTION
    [Header("Player Detection")]
    private GameObject Player;
    [SerializeField] private bool isPlayerInRange;
    [SerializeField] private LayerMask enemyLayers;
    #endregion

    #region ATTACK VARIABLES
    [Header("Attack Variables")]
    [SerializeField] private Transform sword;
    [SerializeField] private Transform firePoint;
    [SerializeField] private CircleCollider2D attackRangeRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackRate;
    [SerializeField] private float speed;
    #endregion

    #region PRIVATE VARIABLES
    [Header("DEBUG PURPOSE ONLY")]
    private float nextWaypointDistance = 3f;
    private float nextAttack = 0.15f;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    #endregion

    
    void Start()
    {
        //Getters for components
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //Invokes "UpdatePath" every 0.3 seconds 
        InvokeRepeating("UpdatePath", 0f, .3f);
        
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            //If cooldown is over, the enemy will attack
            if (Time.time > nextAttack)
            {
                Attack();
                nextAttack = Time.time + attackRate;
            }
        }
    }

    void FixedUpdate()
    {
        //If the path is null, it will return
        if (path == null)
        {
            return;
        }

        //If the current waypoint is bigger than the path, it will set the reachedEndOfPath to true, and return
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        //If the enemy has reached the end of the path, it will set the reachedEndOfPath to true, and return
        else
        {
            reachedEndOfPath = false;
        }

        //Sets the direction to the next waypoint, and normalizes it
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        //Sets the force to the direction, and multiplies it with the speed and Time.deltaTime
        Vector2 force = direction * speed * Time.deltaTime;

        //Moves the rigidbody if the player is not in range
        if (!isPlayerInRange)
        {
            rb.AddForce(force);
        }

        //Calculates the distance between the enemy and the next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //You can see in the debug log how many waypoints there are, and which one the enemy is currently on
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //This 3 lines of code is essentially for visually tracking to enemy by looking at it 
        Vector2 lookDir = Player.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        sword.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the enemy collides with the player, it will set the isPlayerInRange to true
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the enemy exits the player, it will set the isPlayerInRange to false
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }

    void Attack()
    {
        //Sets the animation trigger to "Attack"
        anim.SetTrigger("Attack");

        //Creates a circle around the enemy, and if the player is in the circle, it will print "We hit (player name)"
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRadius, enemyLayers);

        //Loops through all the enemies in the circle
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<HealthController>().TakeDamage(1);
            Debug.Log("We hit " + enemy.name);
        }
    }

    void UpdatePath()
    {
        //If the enemy is not in range of the player, it will start a path to the player
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, Player.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        //If the path is not an error, it will set the path to the path it found, and set the current waypoint to 0
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        //For debugging purposes, it will draw a circle around the enemy, and the radius of the circle is the attackRadius
        Gizmos.DrawWireSphere(firePoint.position, attackRadius);
    }
}
