using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{

    public OrganManager myOrganManager;
    [SerializeField]
    List<GameObject> cellsPrefabs;
    /*[SerializeField]
    List<GameObject> smallRedBloodCells;
    [SerializeField]
    List<GameObject> medRedBloodCells;
    [SerializeField]
    List<GameObject> bigRedBloodCells;*/
    [SerializeField]
    CellMerger myCellMerger;

    public bool canBuyRedcell = true;
    [System.Serializable]
    public class cellsList
    {
        public string name;
        public int id;
        public List<GameObject> Cells;
    }
    public cellsList[] CellTypes = new cellsList[]
    {
        new cellsList
        {
            name = "Small red cell",
            id = 0,
        },
        new cellsList
        {
            name = "Medium red cell",
            id = 1,
        },
        new cellsList
        {
            name = "Big red cell",
            id = 2,
        },
        new cellsList
        {
            name = "small White cell",
            id = 3,
        },
        new cellsList
        {
            name = "Medium White cell",
            id = 4,
        },
        new cellsList
        {
            name = "Big White cell",
            id = 5,
        },
        new cellsList
        {
            name = "small Helper cell",
            id = 6,
        },
         new cellsList
        {
            name = "Medium Helper cell",
            id = 7,
        },
          new cellsList
        {
            name = "Big Helper cell",
            id = 8,
        },
    };



    private void Start()
    {
        myCellMerger = GetComponent<CellMerger>();
    }
    void Update()
    {
        //UpdateCells();
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuySmallRedBloodCell();
        }
    }
    public void InstantiateCells(Vector3 targetPosition = default(Vector3), int cellId = 0)
    {
        myOrganManager = GameManager.gameManager.organManager;
        for (int l = 0; l < CellTypes.Length; l++)
        {
            if (myOrganManager.organs.Count != 0)
            {
                if (myOrganManager.organs[myOrganManager.activeOrganID].lists[l].Cells.Count != 0)
                {
                    for (int i = CellTypes[l].Cells.Count; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[l].Cells.Count; i++)
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

                        GameObject cell = Instantiate(cellsPrefabs[l], spawnPosition, Quaternion.identity);
                        cell.GetComponent<HitPoints>().health = myOrganManager.organs[myOrganManager.activeOrganID].lists[l].Cells[i].health;
                        CellTypes[l].Cells.Add(cell);
                    }
                }
            }
        }
    }
    public void BuySmallRedBloodCell()
    {
        if (canBuyRedcell)
        {
            if (GameManager.gameManager.pointsManager.totalPoints >= myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost)
            {
                OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
                cellInfo.maxHealth = 5;
                cellInfo.health = 5;
                cellInfo.timer = 0;
                cellInfo.alive = true;
                myOrganManager.organs[myOrganManager.activeOrganID].lists[0].Cells.Add(cellInfo);
                InstantiateCells();
                MergeCells();
                GameManager.gameManager.pointsManager.GetPoints(-myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost);
                myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost = myOrganManager.CalculateCosts(myOrganManager.activeOrganID);
            }
            else
            {
                //Not enough points todo
            }
        }
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
        CellTypes[1].Cells.Add(cell);
    }
    void MergeCells()
    {
        if (myOrganManager.organs[myOrganManager.activeOrganID].lists[0].Cells.Count >= 10)
        {
            OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
            cellInfo.health = 5;
            cellInfo.maxHealth = 5;
            cellInfo.timer = 0;
            cellInfo.alive = true;
            myOrganManager.organs[myOrganManager.activeOrganID].lists[1].Cells.Add(cellInfo);
            myOrganManager.organs[myOrganManager.activeOrganID].lists[0].Cells.Clear();
            myCellMerger.Merge(CellTypes[0].Cells);
            canBuyRedcell = false;
            //smallRedBloodCells.Clear();

        }
        if (myOrganManager.organs[myOrganManager.activeOrganID].lists[1].Cells.Count >= 10)
        {
            OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
            cellInfo.health = 5;
            cellInfo.maxHealth = 5;
            cellInfo.timer = 0;
            cellInfo.alive = true;
            myOrganManager.organs[myOrganManager.activeOrganID].lists[2].Cells.Add(cellInfo);
            myOrganManager.organs[myOrganManager.activeOrganID].lists[1].Cells.Clear();
            //myCellMerger.Merge(medRedBloodCells);
            //medRedBloodCells.Clear();

        }

    }
    public void CheckMedCellsMerge()
    {
        if (CellTypes[1].Cells.Count >= 10)
        {
            myCellMerger.Merge(CellTypes[1].Cells, true);
        }
        else
        {
            canBuyRedcell = true;
        }
    }
    /*public void UpdateCells()
    {
        if (myOrganManager.organs[myOrganManager.activeOrganID].smallRedCells.Count != 0)
        {
            for (int i = 0; i < smallRedBloodCells.Count; i++)
            {
                myOrganManager.organs[myOrganManager.activeOrganID].smallRedCells[i].health = smallRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
        if (myOrganManager.organs[myOrganManager.activeOrganID].medRedCells.Count != 0)
        {
            for (int i = 0; i < medRedBloodCells.Count; i++)
            {
                myOrganManager.organs[myOrganManager.activeOrganID].medRedCells[i].health = medRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
        if (myOrganManager.organs[myOrganManager.activeOrganID].bigRedCells.Count != 0)
        {
            for (int i = 0; i < bigRedBloodCells.Count; i++)
            {
                myOrganManager.organs[myOrganManager.activeOrganID].bigRedCells[i].health = bigRedBloodCells[i].GetComponent<HitPoints>().health;
            }
        }
    }*/
    public void DestroyCells(bool destroyAll = true)
    {
        if (destroyAll)
        {
            for (int l = 0; l < CellTypes.Length; l++)
            {
                for (int c = 0; c < CellTypes[l].Cells.Count; c++)
                {
                    Destroy(CellTypes[l].Cells[c]);

                }
                CellTypes[l].Cells.Clear();
            }
        }
        /*foreach (GameObject cell in smallRedBloodCells)
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
        bigRedBloodCells.Clear();*/
    }
}