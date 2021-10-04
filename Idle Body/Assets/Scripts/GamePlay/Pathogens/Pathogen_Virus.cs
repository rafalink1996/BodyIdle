using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Virus : Pathogen_Base
{
    private void Update()
    {
        Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform == target && move)
        {
            print("collided");
            if (target.GetComponent<HitPoints>().canDie)
            {
                target.GetComponent<HitPoints>().health -= 1;
                if (target.GetComponent<HitPoints>().health <= 0)
                {
                    OnPathogenEffect();
                }
                //GetComponent<HitPoints>().hitPoints -= 1;
                StartCoroutine(MoveAgain());
            }
        }
    }

    protected override void OnPathogenEffect()
    {
        pathogenSpawner.AddPathogen(1, myOrganManager.activeOrganType, myOrganManager.activeOrganID);
        pathogenSpawner.SpawnPathogens(false, transform);
    }
}
