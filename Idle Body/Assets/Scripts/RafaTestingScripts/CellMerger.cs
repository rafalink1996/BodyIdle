using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMerger : MonoBehaviour
{
    [SerializeField] GameObject merger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Merge(List<GameObject> cellsToMerge, bool spawnBigCell = false)
    {
        StartCoroutine(MergeCells(cellsToMerge, spawnBigCell));
    }

    IEnumerator MergeCells(List<GameObject> cellsToMerge, bool spawnBigCell)
    {
        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        GameObject cellMerger = Instantiate(merger, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < cellsToMerge.Count; i++)
        {
            RedCells red = cellsToMerge[i].GetComponent<RedCells>();
            red.combineTarget = cellMerger.transform.position;
            red.StartMerge();
            red.myCellMerger = this;
        }
        while (true)
        {
            bool cellsAreReady()
            {
                for (int i = 0; i < cellsToMerge.Count; i++)
                {
                    if (cellsToMerge[i].transform.position != cellMerger.transform.position)
                    {
                        return false;
                    }
                }
                return true;
            }
            if (cellsAreReady())
            {
                break;
            }
            else
            {
                Debug.Log("Cells are not ready");
            }
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < cellsToMerge.Count; i++)
        {
            Destroy(cellsToMerge[i]);
            
        }
        cellsToMerge.Clear();
        CellSpawner myCellSpawner = GameManager.gameManager.organManager.cellSpawner;
        if (!spawnBigCell)
        {
            myCellSpawner.SpawnMedRedBloodCell();
        }
        else
        {
            myCellSpawner.InstantiateCells(cellMerger.transform.position);
        }
        Destroy(cellMerger);
        myCellSpawner.CheckMedCellsMerge();
    }
}
