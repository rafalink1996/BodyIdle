using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager gameManager;
    [Header("References")]
    public PointsManager pointsManager;
    public OrganManager organManager;
    public enum gameState { store, cellsScreen, organScreen, organism};
    public gameState currentState;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()

    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
