using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera sceneCamera;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Weapon weapon;
    public GameObject deathEffect;
    
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    
    
    void Update()
    {
        if(!PauseMenu.isPaused)
        {
            ProcessInputs();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }
        
        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
    
    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
