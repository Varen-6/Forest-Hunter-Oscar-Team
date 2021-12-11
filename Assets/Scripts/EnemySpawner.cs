using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bunny;
    public GameObject wolf;
    public GameObject deer;
    
    private Vector2 GenerateRandomSpawnPoint()
    {
        float randX = UnityRandom.Range(-19f, 19f);
        float randY = UnityRandom.Range(-7f, 9f);
        return new Vector2(randX, randY);
    }
    void Start()
    {
        for (int i = 0; i < GameSettings.BunnyCount; i++)
        {
            Instantiate(bunny, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
        for (int i = 0; i < GameSettings.WolfCount; i++)
        {
            Instantiate(wolf, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
        for (int i = 0; i < GameSettings.DeerCount; i++)
        {
            Instantiate(deer, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
    }
}
