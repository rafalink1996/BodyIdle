using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> cells;
    public int numOfSmallCells;
    public int numOfMedCells;
    public int numOfBigCells;
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSmallCells());
        StartCoroutine(SpawnMedCells());
        StartCoroutine(SpawnBigCells());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnSmallCells()
    {
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < numOfSmallCells; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[0], randomPosition, Quaternion.identity);

        }
    }
    IEnumerator SpawnMedCells()
    {
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < numOfMedCells; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[1], randomPosition, Quaternion.identity);

        }
    }
    IEnumerator SpawnBigCells()
    {
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < numOfBigCells; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[2], randomPosition, Quaternion.identity);

        }
    }



}
