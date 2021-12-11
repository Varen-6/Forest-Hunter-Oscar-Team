using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;


public class AI_Wolf : SteeringBase
{
    public Transform target;
    private Vector3 targetPos;
    private Vector3 randomPos;
    private double randomPhi = 0;
    private string state = "Wander";
    public float detectRange;
    public float calmRange;
    
    private float timeBetweenDoingSomething = 0.15f;
    private float timeWhenWeNextDoSomething;
    
    private Transform closestPrey;
    private float closestSqrPreyDistance;
    
    private void Start()
    {
        base.Start();
        PickRandomWanderSpot();
        WanderTimerReset();
    }
    
    private void FixedUpdate()
    {

        closestPrey = GetClosestEnemy();
        if (closestSqrPreyDistance < (detectRange * detectRange))
        {
            state = "Chase";
            if (closestPrey != null)
            {
                target = closestPrey; 
            }
            else
            {
                state = "Wander";
            }
            
        } else if (closestSqrPreyDistance > calmRange * calmRange)
        {
            state = "Wander";
        }
        else
        {
            if (closestPrey == null)
            {
                state = "Wander";
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
                steer = Steer(target.position);
                break;
            
        }
        ApplyForce(steer);
        ApplyForce(avoidWalls*1.5f);
        ApplySteeringToMotion();
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
        // tag_1 = GameObject.FindGameObjectsWithTag("Neutral").Select(go => go.transform).ToArray();
        // tag_2 = GameObject.FindGameObjectsWithTag("Player").Select(go => go.transform).ToArray();
        Transform[] enemies = GameObject.FindGameObjectsWithTag("Prey").Select(go => go.transform.parent.gameObject.transform).ToArray();
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestSqrPreyDistance = dSqrToTarget;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
    
    private void OnTriggerEnter2D(Collider2D biteInfo)
    {
        switch (biteInfo.gameObject.tag)
        {
            case "Neutral":
                Enemy prey = biteInfo.GetComponent<Enemy>();
                if (prey != null)
                {
                    prey.Die();
                    state = "Wander";
                }
                break;
            case "Player":
                PlayerController player = biteInfo.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Die();
                }
                break;
            
    
        }
    }
}
