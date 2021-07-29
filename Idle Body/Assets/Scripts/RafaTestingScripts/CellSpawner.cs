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
    void Update()
    {
        UpdateCells();
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuySmallRedBloodCell();
        }
    }
    public void InstantiateCells()
    {
        myOrganManager = GameManager.gameManager.organManager;
        if (myOrganManager.organs[organId].smallRedCells.Count != 0)
        {
            for (int i = smallRedBloodCells.Count; i < myOrganManager.organs[organId].smallRedCells.Count; i++)
            {
                Vector3 randomPosition = Random.insideUnitCircle * 3f;
                GameObject cell = Instantiate(cellsPrefabs[0], randomPosition, Quaternion.identity);
                cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].smallRedCells[i].health;
                smallRedBloodCells.Add(cell);
            }
        }
        if (myOrganManager.organs[organId].medRedCells.Count != 0)
        {
            for (int i = medRedBloodCells.Count; i < myOrganManager.organs[organId].medRedCells.Count; i++)
            {
                Vector3 randomPosition = Random.insideUnitCircle * 3f;
                GameObject cell = Instantiate(cellsPrefabs[1], randomPosition, Quaternion.identity);
                cell.GetComponent<HitPoints>().health = myOrganManager.organs[organId].medRedCells[i].health;
                medRedBloodCells.Add(cell);
            }
        }
        if (myOrganManager.organs[organId].bigRedCells.Count != 0)
        {
            for (int i = bigRedBloodCells.Count; i < myOrganManager.organs[organId].bigRedCells.Count; i++)
            {
                Vector3 randomPosition = Random.insideUnitCircle * 3f;
                GameObject cell = Instantiate(cellsPrefabs[2], randomPosition, Quaternion.identity);
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
