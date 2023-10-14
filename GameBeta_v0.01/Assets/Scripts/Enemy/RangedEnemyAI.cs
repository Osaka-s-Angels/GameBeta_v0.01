using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
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
    #endregion

    #region ENEMY VARIABLES
    [Header("Enemy Variables")]
    [SerializeField] private Transform bow;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed;

    [Header("Attack Variables")]
    [SerializeField] private float bulletForce;
    [SerializeField] private float fireRate;
    [SerializeField] private CircleCollider2D attackRange;
    #endregion

    #region PRIVATE VARIABLES
    [Header("DEBUG PURPOSE ONLY")]
    private float nextWaypointDistance = 3f;
    private float nextShot = 0.15f;
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
            if (Time.time > nextShot)
            {
                Shoot();
                nextShot = Time.time + fireRate;
            }
        }
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
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        bow.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        
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
}
