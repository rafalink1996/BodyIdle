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
    public Text buttonText;
    public string sceneToLoad;
    public bool combine_S;
    public bool combine_M;
    public bool canBuyRedCell = true;
    int costOfCell;

    // Start is called before the first frame update
    void Start()
    {
        numOfRedCells = PointsManager.pointsManager.organCells[organID];
        StartCoroutine(SpawnCells());
        
        buttonText.text = "Get Cell \n Cost: " + PointsManager.pointsManager.organCostOfCell[organID].ToString();

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
        if (PointsManager.pointsManager.organCells[organID].y == 10 && !combine_M)
        {
            StartCoroutine(SpawnLargeCell());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        total.text = "Total Points: " + PointsManager.pointsManager.totalPoints.ToString();
        points.text = "Points per second: " + PointsManager.pointsManager.pointsPerSecond.ToString();
        if (Input.GetKeyDown(KeyCode.B)) //spawnear bacterias for testing todo
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[3], randomPosition, Quaternion.identity);
        }

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
        foreach (GameObject cell_S in GameObject.FindGameObjectsWithTag("RedCells1"))
        {
            cell_S.GetComponent<HitPoints>().canDie = false;
        }
        canBuyRedCell = false;
        combine_S = true;
        combine_M = true;
        yield return new WaitForSeconds(0.6f);
        Vector3 randomPosition = Random.insideUnitCircle * 3f;
        GameObject medCell = Instantiate(cells[1], randomPosition, Quaternion.identity);
        medCell.GetComponent<RedCell_M>().spawn = false;
        medCell.GetComponent<RedCell_M>().move = false;
        yield return new WaitForSeconds(0.6f);
        medCell.tag = "RedCells10";
        foreach (GameObject cell_S in GameObject.FindGameObjectsWithTag("RedCells1"))
        {
            cell_S.tag = "Untagged"; 
            cell_S.GetComponent<RedCell_S>().combine = true;
            cell_S.GetComponent<RedCell_S>().combineTarget = medCell.transform.position;
        }
        
    }
    IEnumerator SpawnLargeCell()
    {
        foreach (GameObject cell_M in GameObject.FindGameObjectsWithTag("RedCells10"))
        {
            cell_M.GetComponent<HitPoints>().canDie = false;
        }
        combine_M = true;
        yield return new WaitForSeconds(0.6f);
        Vector3 randomPosition = Random.insideUnitCircle * 3f;
        GameObject largeCell = Instantiate(cells[2], randomPosition, Quaternion.identity);
        largeCell.GetComponent<RedCell_L>().spawn = false;
        largeCell.GetComponent<RedCell_L>().move = false;
        yield return new WaitForSeconds(0.6f);
        largeCell.tag = "RedCells100";
        foreach (GameObject cell_M in GameObject.FindGameObjectsWithTag("RedCells10"))
        {
            cell_M.tag = "Untagged";
            cell_M.GetComponent<RedCell_M>().combine = true;
            cell_M.GetComponent<RedCell_M>().combineTarget = largeCell.transform.position;
        }

    }
    public void BuySmallCell()
    {
        if (PointsManager.pointsManager.totalPoints >= PointsManager.pointsManager.organCostOfCell[organID] && PointsManager.pointsManager.organCells[organID].x < 10 && canBuyRedCell)
        {
            Vector3 randomPosition = Random.insideUnitCircle * 3f;
            Instantiate(cells[0], randomPosition, Quaternion.identity);
            PointsManager.pointsManager.GetPoints(-PointsManager.pointsManager.organCostOfCell[organID]);
            PointsManager.pointsManager.organCostOfCell[organID] += 1;
            buttonText.text = "Get Cell \n Cost: " + PointsManager.pointsManager.organCostOfCell[organID].ToString();
            combine_S = false;
        }
    }

}
