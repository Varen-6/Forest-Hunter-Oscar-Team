using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SteeringBase : MonoBehaviour
{
    public float maxSpeed, maxForce;
    public Vector3 velocity, location, acceleration, startPosition;
    
    
    protected void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        location = transform.position;
        startPosition = transform.position;
    }

    public Vector3 AvoidWalls()
    {
        if (location.x < -18f)
        {
            return SteerForWallAvoid(new Vector2(maxSpeed, velocity.y));
        }
        if (location.x > 18f)
        {
            return SteerForWallAvoid(new Vector2(-maxSpeed, velocity.y));
        }
        if (location.y < -8f)
        {
            return SteerForWallAvoid(new Vector2(velocity.x, maxSpeed));
        }
        if (location.y > 8f)
        {
            return SteerForWallAvoid(new Vector2(velocity.x, -maxSpeed));
        }

        return Vector3.zero;
    }
    
    public void ApplySteeringToMotion()
    {
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
        location += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        RotateTowardTarget();
        transform.position = location;
    }


    private void RotateTowardTarget()
    {
        Vector3 dirToDesiredLoc = location - transform.position;
        dirToDesiredLoc.Normalize();
        float rotZ = Mathf.Atan2(dirToDesiredLoc.y, dirToDesiredLoc.x) * Mathf.Rad2Deg;
        rotZ -= 90;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }
    

    public Vector3 SteerForWallAvoid(Vector3 desired)
    {
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
    
    public Vector3 Steer(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
    
    public Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - location;
        desired.Normalize();
        desired *= -maxSpeed;
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
}
