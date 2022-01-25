using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [Header("References")]
    public NewPointsManager pointsManager;
    public OrganManager organManager;
    public CellView_UI_Manager CellViewUI;
    public OrganView_Manager OrganViewUI;
    public TopUI_Manager topUIManager;
    public PlayerInput playerInput;



    public enum gameState { store, cellsScreen, organScreen, organism };
    [Header("States")]
    public gameState currentState;

    [Header("Game Views")]
    [SerializeField] GameObject cell_View_Holder;
    [SerializeField] GameObject organ_View_Holder;
    [SerializeField] GameObject Organism_View_Holder;



    [Header("Testing")]
    [SerializeField] GameObject test;
    [SerializeField] GameObject testcanvas;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameManager == null)
        {
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
        topUIManager.customStart();
        organManager.CustomStart();
        pointsManager.CustomStart();
        CellViewUI.CustomStart();
        OrganViewUI.CustomStart();
        playerInput.CustomStart();
        topUIManager.TransitionOut();
        Camera.main.Render();

    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        topUIManager.TransitionOut();
    }


    public void changeView(int view)
    {
        organManager.cellSpawner.DestroyCells();
        //organManager.pathogenSpawner.DestroyPathogens();
        switch (view)
        {
            case 0: // Cell_view
                if (cell_View_Holder != null)
                    cell_View_Holder.SetActive(true);
                if (organ_View_Holder != null)
                    organ_View_Holder.SetActive(false);
                if (Organism_View_Holder != null)
                    Organism_View_Holder.SetActive(false);
                organManager.cellSpawner.InstantiateCells();
                organManager.pathogenSpawner.SpawnPathogens();
                CellViewUI.StartChangeCell(0, true);
                break;
            case 1: // organ_view
                if (cell_View_Holder != null)
                    cell_View_Holder.SetActive(false);
                if (organ_View_Holder != null)
                    organ_View_Holder.SetActive(true);
                if (Organism_View_Holder != null)
                    Organism_View_Holder.SetActive(false);
                OrganViewUI.ToggleOrgansButtons(true);
                break;
            case 2: // organism_view
                if (cell_View_Holder != null)
                    cell_View_Holder.SetActive(false);
                if (organ_View_Holder != null)
                    organ_View_Holder.SetActive(false);
                if (Organism_View_Holder != null)
                    Organism_View_Holder.SetActive(true);
                break;
        }

    }

    void GetReferences()
    {
        if (pointsManager == null)
            pointsManager = FindObjectOfType<NewPointsManager>();

        if (organManager == null)
            organManager = FindObjectOfType<OrganManager>();

        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();

        if (CellViewUI == null)
            CellViewUI = FindObjectOfType<CellView_UI_Manager>();

        if (topUIManager == null)
            topUIManager = FindObjectOfType<TopUI_Manager>();

        if (OrganViewUI == null)
            OrganViewUI = FindObjectOfType<OrganView_Manager>();
    }
}
