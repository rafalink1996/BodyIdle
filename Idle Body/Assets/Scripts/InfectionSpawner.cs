using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionSpawner : MonoBehaviour
{
    public float minRadius = 3f;
    public float maxRadius = 7f;
    public int pointAmount = 100;
    public GameObject prefab;
    public int InfectionChance = 20;
    

    bool spawning; 

    void Start()
    {
        spawning = true;
        StartCoroutine(SpawnerTimer());

       

    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {

        var randomDirection = (Random.insideUnitCircle * origin).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);

        var point = origin + randomDirection * randomDistance;

        return point;
    }

    IEnumerator SpawnerTimer()
    {
        while (true)
        {
            if (spawning == true)
            {
                yield return new WaitForSeconds(5);
                float infectionRoll = Random.Range(1, 100);
                if (infectionRoll <= InfectionChance)
                {   
                    Spawninfection();
                }
                
            }
        }
        
    }
    void Spawninfection()
    {
        var origin = transform.position;
        for (int i = 0; i < pointAmount; i++)
        {
            var pointToSpawnAt = RandomPointInAnnulus(origin, minRadius, maxRadius);
            GameObject SpawnedInfection = Instantiate(prefab, pointToSpawnAt, prefab.transform.rotation);
            MoveMeRandom SpawnedInfectionMovement = SpawnedInfection.GetComponent<MoveMeRandom>();
            SpawnedInfectionMovement.body = this.gameObject;

        }
    }
}