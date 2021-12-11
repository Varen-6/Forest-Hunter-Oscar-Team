using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject impactEffect;
    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        switch (hitInfo.gameObject.tag)
        {
            case "Wall":
                Impact();
                break;
            case "Enemy":
            case "Neutral":    
                Enemy enemy = hitInfo.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Die();
                }
                Impact();
                break;
        }
    }

    public void Impact()
    {
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
