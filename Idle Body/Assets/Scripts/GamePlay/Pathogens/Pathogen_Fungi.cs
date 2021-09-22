using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Fungi : Pathogen_Base
{
    bool reproduce;

    private void Update()
    {
        Move();
        if (!reproduce)
        {
            reproduce = true;
            StartCoroutine(Reproduce());
        }
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
        pathogenSpawner.SpawnPathogen(2, false, transform);
    }
    IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(2f);
        OnPathogenEffect();
        reproduce = false;
    }
}
