using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Bacteria : Pathogen_Base
{
    private void Update()
    {
        Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform == target && move)
        {
            if (target.GetComponent<HitPoints>().canDie)
            {
                target.GetComponent<HitPoints>().health -= 1;
                //GetComponent<HitPoints>().hitPoints -= 1;
                StartCoroutine(MoveAgain());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected override void OnPathogenEffect()
    {
     
        Destroy(gameObject);
    }
}
