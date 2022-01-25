using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPointsManager : MonoBehaviour
{
    public double totalPoints;
    public double pointsPerSecond;
    public int ComplexityPoints;
    public int ComplexityMaxPoints = 100;
    public int GoldenMolecules;

    private GameManager GM;
    private OrganManager OM;

    void Start()
    {
        GM = GameManager.gameManager;
        if (GM != null)
            OM = GM.organManager;
    }
    public void CustomStart()
    {
        StartCoroutine(GetPointsPerSecond());
    }

    IEnumerator GetPointsPerSecond()
    {
        float PointDilation = 1f;
        while (true)
        {
            yield return new WaitForSeconds(1f * PointDilation);
            ManagePoints(PointsPerSecond() * PointDilation);
            pointsPerSecond = PointsPerSecond();
            updatePointButtons();

        }
    }
    public double PointsPerSecond()
    {
        double pointsPerSecond = 0;

        if (OM.organTypes.Length != 0) // check if we have organ types
        {
            for (int t = 0; t < OM.organTypes.Length; t++) // go through all organ types
            {
                if (OM.organTypes[t].organs.Count != 0) // check if we have an organ
                {
                    for (int o = 0; o < OM.organTypes[t].organs.Count; o++) // Check all organs
                    {
                        if (OM.organTypes[t].organs[0].CellTypes.Length == 3) // Check if cell Types are the correct size
                        {
                            if (OM.organTypes[t].organs[o].CellTypes[0] != null) // Check if red blood cells are correct
                            {
                                if (OM.organTypes[t].organs[o].CellTypes[0].cellSizes.Count == 3) // check if there are 3 cell sizes
                                {
                                    for (int b = 0; b < OM.organTypes[t].organs[o].CellTypes[0].cellSizes.Count; b++) // go through all sizes
                                    {
                                        if (OM.organTypes[t].organs[o].CellTypes[0].cellSizes[b].CellsInfos.Count != 0) // check if there are red cells of current size
                                        {
                                            for (int c = 0; c < OM.organTypes[t].organs[o].CellTypes[0].cellSizes[b].CellsInfos.Count; c++) // check all cells of current size
                                            {
                                                if (OM.organTypes[t].organs[o].CellTypes[0].cellSizes[b].CellsInfos[c].alive)
                                                {
                                                    int multiplier = Mathf.FloorToInt(Mathf.Pow(10, b));
                                                    pointsPerSecond += multiplier * OM.organTypes[t].pointsMultiplier;

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        return pointsPerSecond;
    }
    public void ManagePoints(double pointValue)
    {
        totalPoints += pointValue;
        // Esta función es para agregar puntos manualmente. Por ejemplo haciendo tap en la pantalla
    }
    public void OnClickGetPoints()
    {
        ManagePoints(1 * OM.organTypes[OM.activeOrganType].pointsMultiplier);
    }

    void updatePointButtons()
    {
        GM.OrganViewUI.UpdateButtons();
    }
}





#region Back Up Code
//if (OM.organs.Count != 0) // check if we have organ
//{
//    for (int o = 0; o < OM.organs.Count; o++) // Check all organs
//    {
//        if (OM.organs[0].lists.Length != 0) // Check if there are cells lists
//        {
//            if (OM.organs[o].lists[0].Cells.Count != 0) // Check if there are small red blood cells
//            {
//                for (int s = 0; s < OM.organs[o].lists[0].Cells.Count; s++) // Check all small red blood cells
//                {
//                    if (OM.organs[o].lists[0].Cells[s].alive)
//                    {
//                        pointsPerSecond += 1 * OM.organs[o].pointsMultiplier;

//                    }
//                }
//            }

//            if (OM.organs[o].lists[1].Cells.Count != 0)// Check if there are Medium red blood cells
//            {
//                for (int m = 0; m < OM.organs[o].lists[1].Cells.Count; m++)// Check all Medium red blood cells
//                {
//                    if (OM.organs[o].lists[1].Cells[m].alive)
//                    {
//                        pointsPerSecond += 10 * OM.organs[o].pointsMultiplier;
//                    }
//                }
//            }
//            if (OM.organs[o].lists[1].Cells.Count != 0)// Check if there are Medium red blood cells
//            {
//                for (int b = 0; b < OM.organs[o].lists[2].Cells.Count; b++)// Check all Big red blood cells
//                {
//                    if (OM.organs[o].lists[2].Cells[b].alive)
//                    {
//                        pointsPerSecond += 100 * OM.organs[o].pointsMultiplier;
//                    }
//                }
//            }

//        }
//        else
//        {
//            Debug.Log("Organs list of cells is equal to 0!");
//        }
//    }
//}
#endregion Back Up Code
