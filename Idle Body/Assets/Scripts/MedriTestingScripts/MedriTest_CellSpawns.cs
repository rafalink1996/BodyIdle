using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedriTest_CellSpawns : MonoBehaviour
{
    //public void InstantiateCells(Vector3 targetPosition = default(Vector3), int cellId = 0)
    //{
    //    myOrganManager = GameManager.gameManager.organManager;
    //    for (int l = 0; l < CellTypes.Length; l++)
    //    {
    //        if (myOrganManager.organs.Count != 0)
    //        {
    //            if (myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells.Count != 0)
    //            {
    //                for (int i = CellTypes[l].CellSizes.Count; i < myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells.Count; i++)
    //                {
    //                    Vector3 spawnPosition;

    //                    if (targetPosition == default(Vector3))
    //                    {
    //                        //spawnPosition = Random.insideUnitCircle * 3f;
    //                        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
    //                        spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
    //                    }
    //                    else
    //                    {
    //                        spawnPosition = targetPosition;
    //                    }

    //                    GameObject cell = Instantiate(cellsPrefabs[l], spawnPosition, Quaternion.identity);
    //                    cell.GetComponent<HitPoints>().health = myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells[i].health;
    //                    cell.TryGetComponent(out Cell_Base cell_Base);
    //                    switch (l)
    //                    {
    //                        case 0:
    //                        case 3:
    //                        case 6:
    //                            if (cell_Base != null)
    //                                cell_Base.CellStart(i, Cell_Base.CellSize.Small, myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells[i]);
    //                            break;
    //                        case 1:
    //                        case 4:
    //                        case 7:
    //                            if (cell_Base != null)
    //                                cell_Base.CellStart(i, Cell_Base.CellSize.Medium, myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells[i]);
    //                            break;
    //                        case 2:
    //                        case 5:
    //                        case 8:
    //                            if (cell_Base != null)
    //                                cell_Base.CellStart(i, Cell_Base.CellSize.Big, myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[l].Cells[i]);
    //                            break;
    //                        default:
    //                            Debug.Log("error initializing");
    //                            break;
    //                    }

    //                    CellTypes[l].CellSizes.Add(cell);
    //                }
    //            }
    //        }
    //    }
    //}

    //public void BuyCell(int CellID, out bool Bought)
    //{
    //    Bought = false;
    //    if (canBuyRedcell)
    //    {
    //        if (GameManager.gameManager.pointsManager.totalPoints >= myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost)
    //        {
    //            OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
    //            cellInfo.maxHealth = 1;
    //            cellInfo.health = 1;
    //            cellInfo.timer = 0;
    //            cellInfo.alive = true;


    //            myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[0].Cells.Add(cellInfo);
    //            InstantiateCells();
    //            MergeCells();
    //            GameManager.gameManager.pointsManager.GetPoints(-myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost);
    //            myOrganManager.organs[myOrganManager.activeOrganID].currentRedCellCost = myOrganManager.CalculateCosts(myOrganManager.activeOrganID);
    //            Bought = true;
    //        }
    //    }
    //}
    //public void SpawnMedRedBloodCell(Vector3 targetPosition = default(Vector3))
    //{
    //    Vector3 spawnPosition;

    //    if (targetPosition == default(Vector3))
    //    {
    //        //spawnPosition = Random.insideUnitCircle * 3f;
    //        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
    //        spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
    //    }
    //    else
    //    {
    //        spawnPosition = targetPosition;
    //    }


    //    GameObject cell = Instantiate(cellsPrefabs[1], spawnPosition, Quaternion.identity);
    //    //cell.GetComponent<Cell_Base>().CellStart(CellTypes[1].Cells.Count, Cell_Base.CellSize.Medium);
    //    //cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].medRedCells[9].health;
    //    CellTypes[1].CellSizes.Add(cell);
    //}
    //void MergeCells()
    //{
    //    if (myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[0].Cells.Count >= 10)
    //    {
    //        OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
    //        cellInfo.health = 1;
    //        cellInfo.maxHealth = 1;
    //        cellInfo.timer = 0;
    //        cellInfo.alive = true;


    //        myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[1].Cells.Add(cellInfo);
    //        myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[0].Cells.Clear();
    //        myCellMerger.Merge(CellTypes[0].CellSizes);
    //        canBuyRedcell = false;
    //        //smallRedBloodCells.Clear();
    //    }
    //    if (myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[1].Cells.Count >= 10)
    //    {
    //        OrganManager.OrganInfo.CellInfo cellInfo = new OrganManager.OrganInfo.CellInfo();
    //        cellInfo.health = 5;
    //        cellInfo.maxHealth = 5;
    //        cellInfo.timer = 0;
    //        cellInfo.alive = true;
    //        myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[2].Cells.Add(cellInfo);
    //        myOrganManager.organs[myOrganManager.activeOrganID].CellTypes[1].Cells.Clear();
    //    }


    //}
    //public void CheckMedCellsMerge()
    //{
    //    if (CellTypes[1].CellSizes.Count >= 10)
    //    {
    //        myCellMerger.Merge(CellTypes[1].CellSizes, true);
    //    }
    //    else
    //    {
    //        canBuyRedcell = true;
    //    }
    //}

    //public void DestroyCells(bool destroyAll = true)
    //{
    //    if (destroyAll)
    //    {
    //        for (int l = 0; l < CellTypes.Length; l++)
    //        {
    //            for (int c = 0; c < CellTypes[l].CellSizes.Count; c++)
    //            {
    //                Destroy(CellTypes[l].CellSizes[c]);

    //            }
    //            CellTypes[l].CellSizes.Clear();
    //        }
    //    }
    //}
}
