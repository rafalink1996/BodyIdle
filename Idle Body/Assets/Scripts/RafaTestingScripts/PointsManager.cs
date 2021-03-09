using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PointsManager : MonoBehaviour
{
    public float pointsPerSecond = 1f;
    public float totalPoints = 1f;
    public Text total;
    public Text points;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPointsPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        total.text = "Total points: " + totalPoints.ToString();
        points.text = "Points per second: " + pointsPerSecond.ToString();
        if (Input.GetKeyDown(KeyCode.P))
        {
            pointsPerSecond *= 2;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetPoints(4);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetPoints(10);
        }
    }
    IEnumerator GetPointsPerSecond()
    {
        yield return new WaitForSeconds(1f);
        totalPoints += pointsPerSecond;
        StartCoroutine(GetPointsPerSecond());

    }
    public void GetPoints(float pointValue)
    {
        totalPoints += pointValue;
    }
}
