using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager gameManager;
    [Header("References")]
    public NewPointsManager pointsManager;
    public OrganManager organManager;
    public BottomUiManager bottomUiManager;
    public TopUIManager topUIManager;

    
    public enum gameState { store, cellsScreen, organScreen, organism};
    [Header("States")]
    public gameState currentState;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager == null)
        {
            //DontDestroyOnLoad(gameObject);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            gameManager = this;
            //Rest of awake code
            GetReferences();

        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        GameData.data.CustomStart();
        pointsManager.StartPointsManager();

    }

    void GetReferences()
    {
        if(pointsManager == null)
        {
            pointsManager = FindObjectOfType<NewPointsManager>();
        }
        if (organManager == null)
        {
            organManager = FindObjectOfType<OrganManager>();
        }
     
    }
}
