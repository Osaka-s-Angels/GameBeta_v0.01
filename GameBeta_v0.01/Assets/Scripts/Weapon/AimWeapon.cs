using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{

    #region Object References
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    #endregion

    #region Bullet Physics
    [SerializeField] private float bulletForce = 0.1f;
    private float nextShot = 0.15f;
    [SerializeField] private float fireRate = 0.5f;
    #endregion

    private void Update()
    {
        HandleAiming();
        HandleShooting();

    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        weaponTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle < 0)
        {
            angle += 360f;
        }
        if (angle >= 90 && angle <= 270)
        {
            aimLocalScale.y = -1f;
           
        }
        else
        {
           
            aimLocalScale.y = +1f;

        }
        weaponTransform.localScale = aimLocalScale;
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextShot)
        {

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

            nextShot = Time.time + fireRate;
        }

    }


    #region Helper Methods
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion
}
