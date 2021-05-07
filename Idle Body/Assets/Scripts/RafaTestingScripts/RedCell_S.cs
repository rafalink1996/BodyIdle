using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell_S : MonoBehaviour
{
    public Vector2 target;
    public Vector2 currentPos;
    public Vector2 combineTarget;
    public bool combine;
    // Start is called before the first frame update
    void Start()
    {
        target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime);
        if (currentPos == target)
        {
            target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        }
        if (combine)
        {
            StartCoroutine(Combine());
        }
    }
    IEnumerator Combine()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<Animator>().SetTrigger("Light");
        yield return new WaitForSeconds(0.3f);
        target = combineTarget;
    }
}
