using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Organ1CellSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> cells;
    public Vector3 numOfRedCells;
    public float organPoints;
    public Text total;
    public Text points;

    // Start is called before the first frame update
    void Start()
    {
        numOfRedCells = PointsManager.pointsManager.organ1Cells;
        StartCoroutine(SpawnCells());
        
    }

    // Update is called once per frame
    void Update()
    {
        //organPoints = GameObject.FindGameObjectsWithTag("RedCells1").Length + (GameObject.FindGameObjectsWithTag("RedCells10").Length * 10) + (GameObject.FindGameObjectsWithTag("RedCells100").Length * 100);
        
        PointsManager.pointsManager.organ1Cells = new Vector3(GameObject.FindGameObjectsWithTag("RedCells1").Length, GameObject.FindGameObjectsWithTag("RedCells10").Length, GameObject.FindGameObjectsWithTag("RedCells100").Length);
        PointsManager.pointsManager.organ1Points = PointsManager.pointsManager.organ1Cells.x + PointsManager.pointsManager.organ1Cells.y * 10 + PointsManager.pointsManager.organ1Cells.z * 100;
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("RafaTestsOrgan2");
        }
        total.text = "Total points: " + PointsManager.pointsManager.totalPoints.ToString();
        points.text = "Points per second: " + PointsManager.pointsManager.pointsPerSecond.ToString();
    }
    IEnumerator SpawnCells()
    {
        
        for (int i = 0; i < numOfRedCells.x; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[0], randomPosition, Quaternion.identity);

        }
        for (int i = 0; i < numOfRedCells.y; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[1], randomPosition, Quaternion.identity);

        }
        for (int i = 0; i < numOfRedCells.z; i++)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[2], randomPosition, Quaternion.identity);

        }
        yield return new WaitForSeconds(0f);
    }
    public void BuySmallCell(int costOfCell)
    {
        if (PointsManager.pointsManager.totalPoints >= costOfCell)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[0], randomPosition, Quaternion.identity);
            PointsManager.pointsManager.GetPoints(-costOfCell);
        }
    }

}
