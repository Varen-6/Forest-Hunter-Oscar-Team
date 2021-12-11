using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;

    public float ammo;
    public float fireForce;

    public void Fire()
    {
        if (ammo > 0)
        {
            GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            ammo -= 1;
        }
        
    }
}
