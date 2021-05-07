using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OrganCellSpawner : MonoBehaviour
{
    public int organID;
    [SerializeField]
    List<GameObject> cells;
    public Vector3 numOfRedCells;
    public float organPoints;
    public Text total;
    public Text points;
    public string sceneToLoad;
    public bool combine_S;

    // Start is called before the first frame update
    void Start()
    {
        numOfRedCells = PointsManager.pointsManager.organCells[organID];
        StartCoroutine(SpawnCells());

    }

    // Update is called once per frame
    void Update()
    {
        //organPoints = GameObject.FindGameObjectsWithTag("RedCells1").Length + (GameObject.FindGameObjectsWithTag("RedCells10").Length * 10) + (GameObject.FindGameObjectsWithTag("RedCells100").Length * 100);

        PointsManager.pointsManager.organCells[organID] = new Vector3(GameObject.FindGameObjectsWithTag("RedCells1").Length, GameObject.FindGameObjectsWithTag("RedCells10").Length, GameObject.FindGameObjectsWithTag("RedCells100").Length);
        PointsManager.pointsManager.organPoints[organID] = PointsManager.pointsManager.organCells[organID].x + PointsManager.pointsManager.organCells[organID].y * 10 + PointsManager.pointsManager.organCells[organID].z * 100;
        if (PointsManager.pointsManager.organCells[organID].x == 10 && !combine_S)
        {
           
                StartCoroutine(SpawnMedCell());
          
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(sceneToLoad);
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
    IEnumerator SpawnMedCell()
    {
        combine_S = true;
        yield return new WaitForSeconds(0.6f);
        Vector3 randomPosition = Random.insideUnitCircle * 3f;
        GameObject medCell = Instantiate(cells[1], randomPosition, Quaternion.identity);
        medCell.GetComponent<RedCell_M>().spawn = false;
        yield return new WaitForSeconds(0.6f);
        foreach (GameObject cell_S in GameObject.FindGameObjectsWithTag("RedCells1"))
        {
            cell_S.GetComponent<RedCell_S>().combine = true;
            cell_S.GetComponent<RedCell_S>().combineTarget = medCell.transform.position;
        }
        
    }
    public void BuySmallCell(int costOfCell)
    {
        if (PointsManager.pointsManager.totalPoints >= costOfCell && PointsManager.pointsManager.organCells[organID].x < 10)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[0], randomPosition, Quaternion.identity);
            PointsManager.pointsManager.GetPoints(-costOfCell);
            combine_S = false;
        }
    }

}
