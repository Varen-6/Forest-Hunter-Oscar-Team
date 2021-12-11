using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;
using System;
using System.Linq;

public class AI_Deer : SteeringBase
{
    
    public Transform target;
    private Vector3 randomPos;
    private double randomPhi = 0;
    private Vector3 targetPos;
    
    private string state = "Wander";
    public float detectRange;
    public float calmRange;
    
    private float timeBetweenDoingSomething = 0.15f;
    private float timeWhenWeNextDoSomething;

    private List<GameObject> flock;
    private Transform closestDanger;
    private float closestSqrDangerDistance;

    void Start()
    {
        base.Start();
        PickRandomWanderSpot();
        WanderTimerReset();
    }

    
    private void WanderTimerReset()
    {
        timeWhenWeNextDoSomething = Time.time + timeBetweenDoingSomething;
    }

    private void PickRandomWanderSpot()
    {
        Random rn = new Random();
        randomPhi += rn.NextDouble() * 1.6 - 0.8;
        randomPos = new Vector2((float)Math.Cos(randomPhi), (float) Math.Sin(randomPhi));
        targetPos = transform.GetChild(0).position + randomPos;
    }
    
    Transform GetClosestEnemy()
    {
        List<Transform> enemies = GameObject.FindGameObjectsWithTag("Danger").Select(go => go.transform.parent.gameObject.transform).ToList();
        enemies.Remove(gameObject.transform);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestSqrDangerDistance = dSqrToTarget;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }

    void ApplyBehaviours()
    {
        
    }

    
    
    void FixedUpdate()
    {
        closestDanger = GetClosestEnemy();
        Vector3 avoidwalls = AvoidWalls();
        flock = GameObject.FindGameObjectsWithTag("Deer").Select(go => go.transform.parent.gameObject).ToList();
        Vector3 separate = Separate(flock);
        Vector3 align = Align(flock);
        Vector3 cohesion = Cohesion(flock);
        Vector3 steer = Vector3.zero;
        if (closestSqrDangerDistance < (detectRange * detectRange))
        {
            state = "Chase";
            if (closestDanger != null)
            {
                target = closestDanger; 
            }
            else
            {
                state = "Wander";
            }
            
        } else if (closestSqrDangerDistance > calmRange * calmRange)
        {
            state = "Wander";
        }
        else
        {
            if (closestDanger == null)
            {
                state = "Wander";
            }
        }
        
        switch (state)
        {
            case "Wander":
                steer = Steer(targetPos);
                if (timeWhenWeNextDoSomething <= Time.time)
                {
                    PickRandomWanderSpot();
                    timeWhenWeNextDoSomething = Time.time + timeBetweenDoingSomething;
                }
                break;
            case "Chase":
                steer = Flee(target.position);
                break;
            
        }
        
        ApplyForce(steer * 1.5f);
        ApplyForce(avoidwalls * 3f);
        ApplyForce(separate * 0.5f);
        ApplyForce(align);
        ApplyForce(cohesion * 0.5f);
        ApplySteeringToMotion();
        
    }

    public Vector3 Align(List<GameObject> flock)
    {
        float distForNeighbor = 10;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < distForNeighbor))
            {
                sum += deer.GetComponent<AI_Deer>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 steer = Vector3.ClampMagnitude(sum - velocity, maxForce);
            return steer;
        }

        return Vector3.zero;

    }
    
    public Vector3 Cohesion(List<GameObject> flock)
    {
        float distForNeighbor = 10;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < distForNeighbor))
            {
                sum += deer.GetComponent<AI_Deer>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            return Steer(sum);
        }

        return Vector3.zero;

    }
    
    public Vector3 Separate(List<GameObject> flock)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        float desiredSep = 1.5f;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < desiredSep))
            {
                Vector3 diff = transform.position - deer.transform.position;
                diff.Normalize();
                sum += diff;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 steer = Vector3.ClampMagnitude(sum - velocity, maxForce);
            return steer;
        }
        
        return Vector3.zero;
    }
}
