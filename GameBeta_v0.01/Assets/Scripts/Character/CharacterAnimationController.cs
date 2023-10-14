using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{

    public Animator animator;
    Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");     
        animator.SetFloat("Speed", movement.sqrMagnitude);

        AngleInput();
    }

    private void AngleInput()
       
    {
        Vector3 characterPosition = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector3 direction = mousePosition - characterPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360f;
        }

        float sinValue = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cosValue= Mathf.Cos(angle * Mathf.Deg2Rad);

        animator.SetFloat("Sin", sinValue);
        animator.SetFloat("Cos", cosValue);

        Vector3 localScale = Vector3.one;
        if (angle >= 90 && angle <= 270)
        {
            localScale.x = -1f;
        }
        else
        {
            localScale.x = +1f;
        }
        this.transform.localScale = localScale;

    }
 
}
