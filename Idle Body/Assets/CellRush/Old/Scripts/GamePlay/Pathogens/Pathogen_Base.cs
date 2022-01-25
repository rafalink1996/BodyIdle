using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathogen_Base : MonoBehaviour
{
    [SerializeField] List<GameObject> possibleCells = null;
    public Transform target = null;
    public float speed;
    protected bool move;
    protected PathogenSpawner pathogenSpawner;
    public OrganManager myOrganManager;
    // Start is called before the first frame update
    void Start()
    {
        PathogenStart();
    }
    public virtual void PathogenStart()
    {
        myOrganManager = GameManager.gameManager.organManager;
        pathogenSpawner = FindObjectOfType<PathogenSpawner>();
        StartCoroutine(MoveAgain());
        target = FindClosestCell();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    protected void Move()
    {
        if (target == null)
        {
            target = FindClosestCell();
        }
        if (target != null && move)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
            //target.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
    }
    protected IEnumerator MoveAgain()
    {
        move = false;
        yield return new WaitForSeconds(Random.Range(1.2f, 1.6f));
        target = null;
        move = true;
    }
    protected virtual void OnPathogenEffect()
    {

    }
    Transform FindClosestCell()
    {
        possibleCells.Clear();
        float distanceToClosestCell = Mathf.Infinity;
        GameObject ClosestCell = null;
        //GameObject[] allCells = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject redCell in GameObject.FindGameObjectsWithTag("RedCells"))
        {
            possibleCells.Add(redCell);
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
