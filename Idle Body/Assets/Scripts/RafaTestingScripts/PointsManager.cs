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
        StartCoroutine(GetPointsPerSecond()); //Empieza la Coroutine que cada segundo agrega los puntos
    }

    // Update is called once per frame
    void Update()
    {
        //El valor de los puntos por segundo se consigue con el número de células que haya en la escena. Se podría
        //guardar un valor aparte por órgano en GameStats y que se sumen los valores de cada órgano para el total
        pointsPerSecond = GameObject.FindGameObjectsWithTag("RedCells1").Length + (GameObject.FindGameObjectsWithTag("RedCells10").Length * 10) + (GameObject.FindGameObjectsWithTag("RedCells100").Length * 100);
        
        total.text = "Total points: " + totalPoints.ToString();
        points.text = "Points per second: " + pointsPerSecond.ToString();

        
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
        // Esta función es para agregar puntos manualmente. Por ejemplo haciendo tap en la pantalla
    }
}
