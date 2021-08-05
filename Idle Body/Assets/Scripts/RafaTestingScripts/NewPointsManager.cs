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

    // Update is called once per frame
    void Update()
    {
        totalPoints = PointsPerSecond();
    }
    IEnumerator GetPointsPerSecond()
    {
        yield return new WaitForSeconds(1f);
        totalPoints += PointsPerSecond();
        StartCoroutine(GetPointsPerSecond());

    }
    float PointsPerSecond()
    {
        float pointsPerSecond = 0;
        for (int o = 0; o < OM.organs.Count; o++)
        {
            for (int s = 0; s < OM.organs[o].smallRedCells.Count; s++)
            {
                if (OM.organs[o].smallRedCells[s].alive)
                {
                    pointsPerSecond += 1 * OM.organs[o].pointsMultiplier;
                }
            }
            for (int m = 0; m < OM.organs[o].medRedCells.Count; m++)
            {
                if (OM.organs[o].medRedCells[m].alive)
                {
                    pointsPerSecond += 10 * OM.organs[o].pointsMultiplier;
                }
            }
            for (int b = 0; b < OM.organs[o].bigRedCells.Count; b++)
            {
                if (OM.organs[o].bigRedCells[b].alive)
                {
                    pointsPerSecond += 100 * OM.organs[o].pointsMultiplier;
                }
            }
            //pointsPerSecond += (OM.organs[o].smallRedCells.Count + (OM.organs[o].medRedCells.Count * 10) + (OM.organs[o].bigRedCells.Count * 100)) * OM.organs[o].pointsMultiplier;
        }
        
        return pointsPerSecond;
    }
}
