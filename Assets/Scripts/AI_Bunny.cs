using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;
public class AI_Bunny : SteeringBase
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
    

    private Transform closestStranger;
    private float closestSqrStrangerDistance;

    public float speedWander;
    public float speedChase;
    
    private void Start()
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
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        closestStranger = GetClosestEnemy();
        if (closestSqrStrangerDistance < (detectRange * detectRange))
        {
            state = "Chase";
            if (closestStranger != null)
            {
                target = closestStranger;
                maxSpeed = speedChase;
            }
            else
            {
                state = "Wander";
                maxSpeed = speedWander;
            }
        } else if (closestSqrStrangerDistance > calmRange * calmRange)
        {
            state = "Wander";
            maxSpeed = speedWander;
        }
        else
        {
            if (closestStranger == null)
            {
                state = "Wander";
                maxSpeed = speedWander;
            }
        }
        Vector3 avoidWalls = AvoidWalls();
        Vector3 steer = Vector3.zero;
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
        ApplyForce(steer);
        ApplyForce(avoidWalls * 1.5f);
        ApplySteeringToMotion();
    }
    
    Transform GetClosestEnemy()
    {
        List<Transform> enemies = GameObject.FindGameObjectsWithTag("Stranger").Select(go => go.transform.parent.gameObject.transform).ToList();
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
                closestSqrStrangerDistance = dSqrToTarget;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
}
