using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    #endregion

    #region Movement
    [Header("Movement")]

    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed;

    private Vector2 movement;
    #endregion

    #region Dashing
    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashLength = 1f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing;
    private bool canDash;
    private Vector3 dashDirection;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        canDash = true;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x !=0 || movement.y!=0)
        {
            dashDirection = movement;
        }
;  

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = dashDirection.normalized * dashSpeed;      
        yield return new WaitForSeconds(dashLength);    
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
