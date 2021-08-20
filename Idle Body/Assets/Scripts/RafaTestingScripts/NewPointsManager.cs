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
        for (int o = 0; o < OM.organs.Count; o++)
        {
            for (int s = 0; s < OM.organs[o].lists[0].Cells.Count; s++)
            {
                if (OM.organs[o].lists[0].Cells[s].alive)
                {
                    pointsPerSecond += 1 * OM.organs[o].pointsMultiplier;
                }
            }
            for (int m = 0; m < OM.organs[o].lists[1].Cells.Count; m++)
            {
                if (OM.organs[o].lists[1].Cells[m].alive)
                {
                    pointsPerSecond += 10 * OM.organs[o].pointsMultiplier;
                }
            }
            for (int b = 0; b < OM.organs[o].lists[2].Cells.Count; b++)
            {
                if (OM.organs[o].lists[2].Cells[b].alive)
                {
                    pointsPerSecond += 100 * OM.organs[o].pointsMultiplier;
                }
            }
            //pointsPerSecond += (OM.organs[o].smallRedCells.Count + (OM.organs[o].medRedCells.Count * 10) + (OM.organs[o].bigRedCells.Count * 100)) * OM.organs[o].pointsMultiplier;
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
