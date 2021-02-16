using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsRoleSystem : MonoBehaviour
{
    public GameObject Cell;
    

    public List<GameObject> IdleCells = new List<GameObject>();
    public List<GameObject> RedBloodCells = new List<GameObject>();
    public List<GameObject> WhiteBloodCells = new List<GameObject>();

    public Cell[] CellsScriptableObjects;


    //---- Systems ----//

    public RedBloodCellSystem redBloodCellSystem;
    public WhiteBloodCellSystem whiteBloodCellSystem;

    //------- Body Parts------//
    public int CurrentBodyPart;

    public List<GameObject> BodyParts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void CellRoleAsign(int cellRole)
    {
        switch (cellRole)
        {
            case 0: // add IdleCell
                GameObject AddedCell = Instantiate(Cell);
                CellBehaviour AddedCellBeh = AddedCell.GetComponent<CellBehaviour>();
                AddedCellBeh.body = BodyParts[CurrentBodyPart];
;                AddedCellBeh.CellScriptableObject = CellsScriptableObjects[0];
                AddedCell.name = "Cell" + "(" + Stats.stats.cells + ")";
                IdleCells.Add(AddedCell);
                break;

            // ------------- RED BLOOD CELL ------------//
            case 1: // add RedBloodCell
                if (IdleCells.Count > 0)
                {
                    GameObject CellChange;
                    CellChange = IdleCells[IdleCells.Count - 1];
                    IdleCells.RemoveAt(IdleCells.Count - 1);
                    CellBehaviour cellChangeBeh = CellChange.GetComponent<CellBehaviour>();
                    cellChangeBeh.CellScriptableObject = CellsScriptableObjects[1];
                    RedBloodCells.Add(CellChange);
                    redBloodCellSystem.AddRedBloodCell();
                    Debug.Log("addedRedBloodCell");

                    break;

                }
                else
                {
                    Debug.Log("OutOfIdleCells");
                    break;
                }

            case -1: // remove RedBloodCell
                if (RedBloodCells.Count > 0)
                {
                    GameObject CellChange;
                    CellChange = RedBloodCells[RedBloodCells.Count - 1];
                    RedBloodCells.RemoveAt(RedBloodCells.Count - 1);
                    CellBehaviour cellChangeBeh = CellChange.GetComponent<CellBehaviour>();
                    cellChangeBeh.CellScriptableObject = CellsScriptableObjects[0];
                    IdleCells.Add(CellChange);
                    redBloodCellSystem.RemoveRedBloodCell();
                    Debug.Log("RemovedRedBloodCell");
                    break;
                }
                else
                {
                    Debug.Log("noRedBloodCells");
                    break;
                }

            // ------------- WHITE BLOOD CELL ------------//
            case 2:// add White Blood Cell
                if (IdleCells.Count > 0)
                {
                    GameObject CellChange;
                    CellChange = IdleCells[IdleCells.Count - 1];
                    IdleCells.RemoveAt(IdleCells.Count - 1);
                    CellBehaviour cellChangeBeh = CellChange.GetComponent<CellBehaviour>();
                    cellChangeBeh.CellScriptableObject = CellsScriptableObjects[2];
                    WhiteBloodCells.Add(CellChange);
                    whiteBloodCellSystem.AddWhiteBloodCell();
                    break;

                }
                else
                {
                    Debug.Log("OutOfIdleCells");
                    break;
                }
            case -2:// Remove White Blood Cell
                if (WhiteBloodCells.Count > 0)
                {
                    GameObject CellChange;
                    CellChange = WhiteBloodCells[WhiteBloodCells.Count - 1];
                    WhiteBloodCells.RemoveAt(WhiteBloodCells.Count - 1);
                    CellBehaviour cellChangeBeh = CellChange.GetComponent<CellBehaviour>();
                    cellChangeBeh.CellScriptableObject = CellsScriptableObjects[0];
                    IdleCells.Add(CellChange);
                    whiteBloodCellSystem.RemoveWhiteBloodCell();
                    break;
                }
                else
                {
                    Debug.Log("no WhiteBloodCells");
                    break;
                }

        }

        
    }
}
