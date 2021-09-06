using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPointsManager : MonoBehaviour
{
    public float totalPoints;
    private OrganManager OM;
    // Start is called before the first frame update
    void Start()
    {
        OM = GameManager.gameManager.organManager;
    }
    public void StartPointsManager()
    {
        StartCoroutine(GetPointsPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        //totalPoints = PointsPerSecond();
    }
    IEnumerator GetPointsPerSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GetPoints(PointsPerSecond());  
        }
    }
    float PointsPerSecond()
    {
        float pointsPerSecond = 0;

        if (OM.organs.Count != 0) // check if we have organ
        {
            for (int o = 0; o < OM.organs.Count; o++) // Check all organs
            {
                if (OM.organs[0].lists.Length == 9) // Check if cell lists are the correct size
                {
                    for (int c = 0; c < 3; c++) // Cehck red blood cells
                    {
                        if (OM.organs[o].lists[c] != null)
                        {
                            if (OM.organs[o].lists[c].Cells != null)
                            {
                                if (OM.organs[o].lists[c].Cells.Count != 0)
                                {
                                    for (int b = 0; b < OM.organs[o].lists[c].Cells.Count; b++)// Check cell list cells red blood cells
                                    {
                                        if (OM.organs[o].lists[c].Cells[b].alive)
                                        {
                                            int multiplier = Mathf.FloorToInt(Mathf.Pow(10, c));
                                            Debug.Log(multiplier);
                                            pointsPerSecond += multiplier * OM.organs[o].pointsMultiplier;
                                            
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.Log("No cells of type: " + c);
                                }

                            }
                            else
                            {
                                Debug.Log("Cells are null");
                            }
                        }
                        else
                        {
                            Debug.Log("Cells list are null");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cell list lenght is not 9");
                }
            }
        }

        return pointsPerSecond;
        
    }
    public void GetPoints(float pointValue)
    {
        totalPoints += pointValue;
        // Esta función es para agregar puntos manualmente. Por ejemplo haciendo tap en la pantalla
    }
    public void OnClickGetPoints()
    {
        GetPoints(1 * OM.organs[OM.activeOrganID].pointsMultiplier);
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
