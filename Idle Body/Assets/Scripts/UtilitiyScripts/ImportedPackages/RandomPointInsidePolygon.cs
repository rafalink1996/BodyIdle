using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointInsidePolygon : MonoBehaviour
{
    Vector2[] Vertices2D;
    EdgeCollider2D edgeCollider2D;
    GameObject body;
    private void Start()
    {
        body = GameObject.Find("Body");
        edgeCollider2D = body.GetComponent<EdgeCollider2D>();
        


    }

}
