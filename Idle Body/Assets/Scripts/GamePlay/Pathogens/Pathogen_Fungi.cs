using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Fungi : Pathogen_Base
{
    bool reproduce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        //Instantiate another fungus
    }
    IEnumerator Reproduce()
    {
        yield return new WaitForSeconds(10f);
        OnPathogenEffect();
        reproduce = false;
    }
}
