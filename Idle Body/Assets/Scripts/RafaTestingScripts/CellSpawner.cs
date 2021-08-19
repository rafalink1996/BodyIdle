using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    public int organId;
    public OrganManager myOrganManager;
    [SerializeField]
    List<GameObject> cellsPrefabs;
    [SerializeField]
    List<GameObject> smallRedBloodCells;
    [SerializeField]
    List<GameObject> medRedBloodCells;
    [SerializeField]
    List<GameObject> bigRedBloodCells;
    [SerializeField]
    CellMerger myCellMerger;
    private void Start()
    {
        myCellMerger = GetComponent<CellMerger>();
    }
    void Update()
    {
        UpdateCells();
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuySmallRedBloodCell();
        }
    }
    public void InstantiateCells(Vector3 targetPosition = default(Vector3))
    {
        myOrganManager = GameManager.gameManager.organManager;
        if (myOrganManager.organs[organId].smallRedCells.Count != 0)
        {
            for (int i = smallRedBloodCells.Count; i < myOrganManager.organs[organId].smallRedCells.Count; i++)
            {
                Vector3 spawnPosition;

                if (targetPosition == default(Vector3))
                {
                    //spawnPosition = Random.insideUnitCircle * 3f;
                    Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
                    spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
                }
                else
                {
                    spawnPosition = targetPosition;
                }

                GameObject cell = Instantiate(cellsPrefabs[0], spawnPosition, Quaternion.identity);
                cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].smallRedCells[i].health;
                smallRedBloodCells.Add(cell);

            }
        }
        if (myOrganManager.organs[organId].medRedCells.Count != 0)
        {
            for (int i = medRedBloodCells.Count; i < myOrganManager.organs[organId].medRedCells.Count; i++)
            {
                Vector3 spawnPosition;

                if (targetPosition == default(Vector3))
                {
                    //spawnPosition = Random.insideUnitCircle * 3f;
                    Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
                    spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
                }
                else
                {
                    spawnPosition = targetPosition;
                }


                GameObject cell = Instantiate(cellsPrefabs[1], spawnPosition, Quaternion.identity);
                cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].medRedCells[i].health;
                medRedBloodCells.Add(cell);
            }
        }
        if (myOrganManager.organs[organId].bigRedCells.Count != 0)
        {
            for (int i = bigRedBloodCells.Count; i < myOrganManager.organs[organId].bigRedCells.Count; i++)
            {
                Vector3 spawnPosition;

                if (targetPosition == default(Vector3))
                {
                    //spawnPosition = Random.insideUnitCircle * 3f;
                    Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
                    spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
                }
                else
                {
                    spawnPosition = targetPosition;
                }

                GameObject cell = Instantiate(cellsPrefabs[2], spawnPosition, Quaternion.identity);
                cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].bigRedCells[i].health;
                bigRedBloodCells.Add(cell);
            }
        }
    }
    public void BuySmallRedBloodCell()
    {
        OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
        cellInfo.health = 5;
        cellInfo.maxHealth = 5;
        cellInfo.timer = 0;
        cellInfo.alive = true;
        myOrganManager.organs[organId].smallRedCells.Add(cellInfo);
        InstantiateCells();
        MergeCells();
    }
    public void SpawnMedRedBloodCell(Vector3 targetPosition = default(Vector3))
    {
        Vector3 spawnPosition;

        if (targetPosition == default(Vector3))
        {
            //spawnPosition = Random.insideUnitCircle * 3f;
            Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
            spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        }
        else
        {
            spawnPosition = targetPosition;
        }


        GameObject cell = Instantiate(cellsPrefabs[1], spawnPosition, Quaternion.identity);
        //cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].medRedCells[9].health;
        medRedBloodCells.Add(cell);
    }
    void MergeCells()
    {
        if (myOrganManager.organs[organId].smallRedCells.Count >= 10)
        {
            OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
            cellInfo.health = 5;
            cellInfo.maxHealth = 5;
            cellInfo.timer = 0;
            cellInfo.alive = true;
            myOrganManager.organs[organId].medRedCells.Add(cellInfo);
            myOrganManager.organs[organId].smallRedCells.Clear();
            myCellMerger.Merge(smallRedBloodCells);
            //smallRedBloodCells.Clear();

        }
        if (myOrganManager.organs[organId].medRedCells.Count >= 10)
        {
            OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
            cellInfo.health = 5;
            cellInfo.maxHealth = 5;
            cellInfo.timer = 0;
            cellInfo.alive = true;
            myOrganManager.organs[organId].bigRedCells.Add(cellInfo);
            myOrganManager.organs[organId].medRedCells.Clear();
            //myCellMerger.Merge(medRedBloodCells);
            //medRedBloodCells.Clear();

        }

    }
    public void CheckMedCellsMerge()
    {
        if (medRedBloodCells.Count >= 10)
        {
            myCellMerger.Merge(medRedBloodCells, true);
        }
    }
    public void UpdateCells()
    {
        if (myOrganManager.organs[organId].smallRedCells.Count != 0)
        {
            for (int i = 0; i < smallRedBloodCells.Count; i++)
            {
                myOrganManager.organs[organId].smallRedCells[i].health = smallRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
        if (myOrganManager.organs[organId].medRedCells.Count != 0)
        {
            for (int i = 0; i < medRedBloodCells.Count; i++)
            {
                myOrganManager.organs[organId].medRedCells[i].health = medRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
        if (myOrganManager.organs[organId].bigRedCells.Count != 0)
        {
            for (int i = 0; i < bigRedBloodCells.Count; i++)
            {
                myOrganManager.organs[organId].bigRedCells[i].health = bigRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
    }
    public void DestroyCells()
    {
        foreach (GameObject cell in smallRedBloodCells)
        {
            Destroy(cell);
        }
        foreach (GameObject cell in medRedBloodCells)
        {
            Destroy(cell);
        }
        foreach (GameObject cell in bigRedBloodCells)
        {
            Destroy(cell);
        }
        smallRedBloodCells.Clear();
        medRedBloodCells.Clear();
        bigRedBloodCells.Clear();
    }
}