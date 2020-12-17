using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsRoleSystem : MonoBehaviour
{
    public GameObject Cell;
    

    public List<GameObject> IdleCells = new List<GameObject>();
    public List<GameObject> RedBloodCells = new List<GameObject>();
    public List<GameObject> WhiteBloodCells = new List<GameObject>();

    int cellQuantity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            IdleCells.Add(Cell);
            
            
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject CellChange;
            CellChange = IdleCells[IdleCells.Count - 1];
            IdleCells.RemoveAt(IdleCells.Count -1);

            RedBloodCells.Add(CellChange);
            
            
        }
    }
    public void AddIdleCell()
    {
        GameObject AddedCell = Instantiate(Cell);
        AddedCell.name = "Cell" + "(" + cellQuantity + ")";
        cellQuantity++;
        IdleCells.Add(AddedCell);
        
    }
    public void AddRedBloodCell()
    {
        if (IdleCells.Count > 0)
        {
            GameObject CellChange;
            CellChange = IdleCells[IdleCells.Count - 1];
            IdleCells.RemoveAt(IdleCells.Count - 1);

            RedBloodCells.Add(CellChange);

        }
        else
        {
            Debug.Log("OutOfIdleCells");
        }

    }

    public void RemoveRedBloodCell()
    {
        if (RedBloodCells.Count > 0)
        {
            GameObject CellChange;
            CellChange = RedBloodCells[RedBloodCells.Count - 1];
            RedBloodCells.RemoveAt(RedBloodCells.Count - 1);

            IdleCells.Add(CellChange);
        }
        else
        {
            Debug.Log("noRedBloodCells");
        }
        
    }
}
