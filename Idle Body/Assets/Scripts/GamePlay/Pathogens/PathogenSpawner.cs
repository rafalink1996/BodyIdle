using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathogenSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] pathogenObjects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnPathogen(int pathogenId, bool random = true, Transform myTransform = null)
    {
        Vector3 spawnPosition;
        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        if (random)
        {
            spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        }
        else
        {
            Vector3 offset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            spawnPosition = myTransform.position + offset;
        }
        GameObject cell = Instantiate(pathogenObjects[pathogenId], spawnPosition, Quaternion.identity);
    }
}
