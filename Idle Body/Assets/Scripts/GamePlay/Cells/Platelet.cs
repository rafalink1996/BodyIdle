using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platelet : MonoBehaviour
{

    Rigidbody2D RB;
    [SerializeField] float speed = 2;
    [SerializeField] bool Move;
    [SerializeField] bool Stop;
    Coroutine WaitCoroutine;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    public void CustomStart()
    {
        if(RB == null)
        {
            Debug.Log("Rb is null");
            RB = GetComponent<Rigidbody2D>();
        }
        PlateletMove();
    }

    private void Update()
    {
        if (Move)
        {
            PlateletMove();
            Move = false;
        }
        if (Stop)
        {
            StopCoroutine(WaitCoroutine);
            Stop = false;
        }
    }
    private void PlateletMove()
    {
        Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if(RB == null)
        {
            
        }
        RB.AddForce(force.normalized * speed * RB.mass);
        WaitCoroutine = StartCoroutine(WaitTime(Random.Range(8f,10f)));
    }
    IEnumerator WaitTime(float Time)
    {
        yield return new WaitForSeconds(Time);
        PlateletMove();
    }

    public void Despawn()
    {
        StopCoroutine(WaitCoroutine);
        gameObject.SetActive(false);
    }

    // Moverse.
    // Detectar si hay una herida.

}
