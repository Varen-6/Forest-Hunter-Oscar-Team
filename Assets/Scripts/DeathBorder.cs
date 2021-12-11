using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D tresspassInfo)
    {
        switch (tresspassInfo.gameObject.tag)
        {
            case "Player" :
                PlayerController player = tresspassInfo.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Die();
                }
                break;
            case "Neutral":
            case "Enemy" :    
                Enemy tresspasser = tresspassInfo.GetComponent<Enemy>();
                if (tresspasser != null)
                {
                    tresspasser.Die();
                }
                break;
            

        }
    }
}
