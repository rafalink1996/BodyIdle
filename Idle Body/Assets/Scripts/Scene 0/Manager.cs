using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    [SerializeField] GameObject LoadingScreen;

    [Header("Script Referneces")]
    public PlayFabLogin playFab;
    public OfflineManager offlineManager;
    public InternetManager internetManager;
    public GameLoader gameloader;

    private void Awake()
    {
        if (instance == null)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            instance = this;
            //Rest of awake code
            GetReferences();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GetReferences();
        SetFrameRate();
        internetManager.CheckConection();

    }

    public void playfabstart()
    {
        playFab.GetReferences();
        playFab.Initialize();
        //offlineManager.GetReferences();
    }

    public void CheckData()
    {
        offlineManager.StartTimeData();
    }

    void SetFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
    void GetReferences()
    {
        offlineManager = FindObjectOfType<OfflineManager>();
        playFab = FindObjectOfType<PlayFabLogin>();
        internetManager = FindObjectOfType<InternetManager>();
        gameloader = FindObjectOfType<GameLoader>();
    }
}
