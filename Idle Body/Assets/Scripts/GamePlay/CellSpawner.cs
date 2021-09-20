using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{

    public OrganManager myOrganManager;
    public CellMerger myCellMerger;
    [SerializeField] GameObject CellHolder;

    public bool CanBuyCell = true;
    [System.Serializable]
    public class CellType
    {
        [System.Serializable]
        public class CellSize
        {
            public string name;
            public List<GameObject> Cells;
            public GameObject CellPrefab;
        }
        public string name;
        public CellSize[] CellSizes;
    }
    public CellType[] CellTypes = new CellType[]
    {
        new CellType
        {
            name = "Red Cells",
            CellSizes = new CellType.CellSize[]
            {
                new CellType.CellSize
                {
                    name = "Small Red",
                    Cells = new List<GameObject>()
                },
                  new CellType.CellSize
                {
                    name = "Medium Red",
                    Cells = new List<GameObject>()
                },
                    new CellType.CellSize
                {
                    name = "Big Red",
                    Cells = new List<GameObject>()
                }
            }
        },
        new CellType
        {
            name = "White Cells",
                  CellSizes = new CellType.CellSize[]
            {
                new CellType.CellSize
                {
                    name = "Small White",
                    Cells = new List<GameObject>()
                },
                  new CellType.CellSize
                {
                    name = "Medium White",
                    Cells = new List<GameObject>()
                },
                    new CellType.CellSize
                {
                    name = "Big White",
                    Cells = new List<GameObject>()
                }
            }
        },
        new CellType
        {
            name = "Helper Cells",
                  CellSizes = new CellType.CellSize[]
            {
                new CellType.CellSize
                {
                    name = "Small Helper",
                    Cells = new List<GameObject>()
                },
                  new CellType.CellSize
                {
                    name = "Medium Helper",
                    Cells = new List<GameObject>()
                },
                    new CellType.CellSize
                {
                    name = "Big Helper",
                    Cells = new List<GameObject>()
                }
            }
        },
    };



    public void CustomStart()
    {
        myCellMerger = GetComponent<CellMerger>();
        myOrganManager = GameManager.gameManager.organManager;
    }

    public void InstantiateCells(Vector3 targetPosition = default(Vector3), bool SpawnAll = true, int cellType = 0, int cellSize = 0, bool IgnoreCellInfo = false)
    {
        if (SpawnAll)
        {
            if (myOrganManager != null)
            {
                if (myOrganManager.organTypes[myOrganManager.activeOranType].organs.Count != 0) // check if there is an organ
                {
                    for (int a = 0; a < myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes.Length; a++) // go through all cell types
                    {
                        for (int b = 0; b < myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[a].cellSizes.Count; b++) // go through all cell sizes
                        {
                            if (myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[a].cellSizes[b].CellsInfos.Count != 0) // check if there are cells for current size
                            {
                                for (int c = CellTypes[a].CellSizes[b].Cells.Count; c < myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[a].cellSizes[b].CellsInfos.Count; c++) // go through differenece of cells of current size vs current instaniated cells
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

                                    GameObject cell = Instantiate(CellTypes[a].CellSizes[b].CellPrefab, spawnPosition, Quaternion.identity);
                                    cell.transform.SetParent(CellHolder.transform);
                                    cell.GetComponent<HitPoints>().health = myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[a].cellSizes[b].CellsInfos[c].health;
                                    cell.TryGetComponent(out Cell_Base cell_Base);
                                    cell_Base.CellStart(c, Cell_Base.CellSize.Small, myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[a].cellSizes[b].CellsInfos[c]);
                                    cell_Base.CellStartAnim();


                                    CellTypes[a].CellSizes[b].Cells.Add(cell);
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                Debug.LogWarning("Error: Organ Manager is null (at CellSpawner.cs - instantiateCells()");
            }
        }
        else
        {
            Vector3 spawnPosition;
            if (targetPosition == default(Vector3))
            {
                Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
                spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
            }
            else
            {
                spawnPosition = targetPosition;
            }
            GameObject cell = Instantiate(CellTypes[cellType].CellSizes[cellSize].CellPrefab, spawnPosition, Quaternion.identity);
            cell.TryGetComponent(out Cell_Base cell_Base);
            cell_Base.CellStartAnim();
            if (!IgnoreCellInfo)
            {
                int cellPosition = myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].cellSizes[cellSize].CellsInfos.Count;
                cell.GetComponent<HitPoints>().health = myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].cellSizes[cellSize].CellsInfos[cellPosition - 1].health;
                if (cell_Base != null)
                {
                    cell_Base.CellStart(cellPosition, Cell_Base.CellSize.Small, myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].cellSizes[cellSize].CellsInfos[cellPosition - 1]);
                }
                
            }
            CellTypes[cellType].CellSizes[cellSize].Cells.Add(cell);
        }
    }


    public void BuyCell(out bool Bought, int cellType = 0)
    {
        Bought = false;
        if (CanBuyCell)
        {
            if (GameManager.gameManager.pointsManager.totalPoints >= myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].currentCellCost)
            {

                /*----DATA----*/
                /*----Create new Small Cell Data----*/
                OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo cellInfo = new OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo();
                cellInfo.maxHealth = 1;
                cellInfo.health = 1;
                cellInfo.timer = 0;
                cellInfo.alive = true;
                myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].cellSizes[0].CellsInfos.Add(cellInfo);


                /*----Check for merge in data----*/
                DataMerge(cellType, out bool merge, out bool big);

                /*----Manage points a cost data----*/
                GameManager.gameManager.pointsManager.ManagePoints(-myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].currentCellCost);
                myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[cellType].currentCellCost = myOrganManager.CalculateCosts(myOrganManager.activeOrganID, cellType);
                Bought = true;

                /*----END OF DATA----*/
                /*-------------------*/
                /*-------VISUAL------*/


                /*----Instantiate New Cell----*/
                if (merge)
                {
                    if (big)
                    {
                        myCellMerger.Merge(CellTypes[cellType].CellSizes[0].Cells, cellType, true);
                    }
                    else
                    {
                        myCellMerger.Merge(CellTypes[cellType].CellSizes[0].Cells, cellType);
                    }
                    CanBuyCell = false;
                }
                else
                {
                    InstantiateCells();
                }
            }
            else
            {
                Debug.Log("not enough points");
            }
        }
    }
    void DataMerge(int CellType, out bool MergeTime, out bool Big)
    {
        MergeTime = false;
        Big = false;
        for (int i = 0; i < myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes.Count; i++) // Check all cell Sizes of current type
        {
            if (myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[i].CellsInfos.Count >= 10) // check if current cell size cell infos are greater than 10
            {
                OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo cellInfo = new OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo();
                cellInfo.health = Mathf.Pow(10, i);
                cellInfo.maxHealth = Mathf.Pow(10, i);
                cellInfo.timer = 0;
                cellInfo.alive = true;
                myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[i + 1].CellsInfos.Add(cellInfo);
                myOrganManager.organTypes[myOrganManager.activeOranType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[i].CellsInfos.Clear();
                MergeTime = true;
                if (i == 1)
                {
                    Big = true;
                }
            }
        }
    }
     public void CheckMedCellsMerge(int cellType)
    {
        if (CellTypes[cellType].CellSizes[1].Cells.Count >= 10)
        {
            Debug.Log("Whoah it's a big cell");
            myCellMerger.Merge(CellTypes[cellType].CellSizes[1].Cells, cellType, false);
        }
        else
        {
            CanBuyCell = true;
        }
    }

    public void DestroyCells(bool destroyAll = true)
    {
        if (destroyAll)
        {
            for (int a = 0; a < CellTypes.Length; a++)
            {
                for (int b = 0; b < CellTypes[a].CellSizes.Length; b++)
                {
                    for (int c = 0; c < CellTypes[a].CellSizes[b].Cells.Count; c++)
                    {
                        Destroy(CellTypes[a].CellSizes[b].Cells[c]);
                    }
                    CellTypes[a].CellSizes[b].Cells.Clear();
                }
            }
        }
    }
}