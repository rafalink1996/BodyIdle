using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Virus : Pathogen_Base
{
    private void Update()
    {
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
    protected override void OnPathogenEffect()
    {
        pathogenSpawner.AddPathogen(2, myOrganManager.activeOrganType, myOrganManager.activeOrganID);
        pathogenSpawner.SpawnPathogens(false, transform);
    }
}
