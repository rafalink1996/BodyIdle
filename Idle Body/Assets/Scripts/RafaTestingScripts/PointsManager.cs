using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PointsManager : MonoBehaviour
{
    public static PointsManager pointsManager;
    public float pointsPerSecond = 1f;
    public float totalPoints = 1f;
    //public Text total;
    //public Text points;
    public Vector3 organ1Cells;
    public float organ1Points;
    public Vector3 organ2Cells;
    public float organ2Points;
    // Start is called before the first frame update
    private void Awake()
    {
        if (pointsManager == null)
        {
            DontDestroyOnLoad(gameObject);
            pointsManager = this;
        }
        else if (pointsManager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        organ1Points = organ1Cells.x + organ1Cells.y * 10 + organ1Cells.z * 100;
        organ2Points = organ2Cells.x + organ2Cells.y * 10 + organ2Cells.z * 100;
        StartCoroutine(GetPointsPerSecond()); //Empieza la Coroutine que cada segundo agrega los puntos
    }

    // Update is called once per frame
    void Update()
    {
        //El valor de los puntos por segundo se consigue con el número de células que haya en la escena. Se podría
        //guardar un valor aparte por órgano en GameStats y que se sumen los valores de cada órgano para el total
        pointsPerSecond = organ1Points + organ2Points;
        
        //total.text = "Total points: " + totalPoints.ToString();
        //points.text = "Points per second: " + pointsPerSecond.ToString();

        
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetPoints(1);
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
