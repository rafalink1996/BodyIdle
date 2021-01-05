using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMeRandom : MonoBehaviour
{
    public GameObject body;
    Vector3 RandomPosition;
    public float speed;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 9);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != RandomPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, RandomPosition, speed);
        }
        else
        {
            randomizePos();
        }
    }

    void randomizePos()
    {
        Vector3 offset = Random.insideUnitCircle * radius;
        RandomPosition = body.transform.position + offset;

    }
}
