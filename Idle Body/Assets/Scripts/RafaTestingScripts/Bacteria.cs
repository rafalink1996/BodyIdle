using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : MonoBehaviour
{
    [SerializeField] List<GameObject> possibleCells = null;
    public Transform target = null;
    public float speed;
    bool move;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveAgain());
        /*foreach(GameObject cell_S in GameObject.FindGameObjectsWithTag("RedCells1"))
        {
            possibleCells.Add(cell_S);
        }
        foreach (GameObject cell_M in GameObject.FindGameObjectsWithTag("RedCells10"))
        {
            possibleCells.Add(cell_M);
        }
        foreach (GameObject cell_L in GameObject.FindGameObjectsWithTag("RedCells100"))
        {
            possibleCells.Add(cell_L);
        }*/
        target = FindClosestCell();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = FindClosestCell();
        }
        if (target != null && move)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
            target.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform == target && move)
        {
            if (target.GetComponent<HitPoints>().canDie)
            {
                target.GetComponent<HitPoints>().hitPoints -= 1;
                GetComponent<HitPoints>().hitPoints -= 1;
                StartCoroutine(MoveAgain());
            }
        }
    }
    IEnumerator MoveAgain()
    {
        move = false;
        yield return new WaitForSeconds(Random.Range(1.2f, 1.6f));
        target = null;
        move = true;
    }
    Transform FindClosestCell()
    {
        possibleCells.Clear();
        float distanceToClosestCell = Mathf.Infinity;
        GameObject ClosestCell = null;
        //GameObject[] allCells = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject cell_S in GameObject.FindGameObjectsWithTag("RedCells1"))
        {
            possibleCells.Add(cell_S);
        }
        foreach (GameObject cell_M in GameObject.FindGameObjectsWithTag("RedCells10"))
        {
            possibleCells.Add(cell_M);
        }
        foreach (GameObject cell_L in GameObject.FindGameObjectsWithTag("RedCells100"))
        {
            possibleCells.Add(cell_L);
        }
        
        if (possibleCells.Count != 0)
        {
            //Debug.Log("locatingEnemy");
            foreach (GameObject currentCell in possibleCells)
            {
                if (currentCell != null)
                {
                    float distanceToCell = (currentCell.transform.position - this.transform.position).sqrMagnitude;
                    if (distanceToCell < distanceToClosestCell)
                    {
                        distanceToClosestCell = distanceToCell;
                        ClosestCell = currentCell;
                    }
                }
                else
                {
                    return null;
                }

            }

            //target = ClosestEnemy.transform;
            return ClosestCell.transform;
            //Debug.Log("enemy located" + target.name); ;

        }
        else
        {
            // Debug.Log("no enemies");
            return null;
        }
        
    }
}
