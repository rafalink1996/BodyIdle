using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    Transform Target;
    public float[] speed;
    float  MoveSpeed;
    const float Epsilon = 0.1f;
    Vector2 mlookDirection;
 
    void Start()
    {
        Target = GameObject.Find("redBloodCellTarget").GetComponent<Transform>();
        MoveSpeed = Random.Range(speed[0], speed[1]);
    }

    
    void Update()
    {
       
        mlookDirection = (Target.position - transform.position).normalized;

        if ((transform.position - Target.position).magnitude > Epsilon)
        {   
            transform.Translate(mlookDirection * MoveSpeed * Time.deltaTime);
            //transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        }
       
    }
}
