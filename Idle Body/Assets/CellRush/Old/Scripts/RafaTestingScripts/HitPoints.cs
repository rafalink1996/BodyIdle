using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public float health;
    public bool canDie = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            DeactivateCell();
        }
    }

    void DeactivateCell()
    {
        //Destroy(gameObject);
    }
}
