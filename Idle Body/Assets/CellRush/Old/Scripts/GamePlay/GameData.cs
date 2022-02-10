
using UnityEngine;
using System;
public class GameData : MonoBehaviour
{
    [Header ("Data")]
    public double energyPoints;
    public double SavedPointsPerSecond;
    public DateTime lastSesionTime;
    public int saveNumber;

    [Header("Instance")]
    public static GameData data;

    [Header("References")]
    OrganManager myOrganManager;
    SaveObject LoadedObject;
    public SaveObject LoadedPlayfabObject1;
    public SaveObject LoadedPlayfabObject2;

    public bool PlayfabLogin;
    public bool FacebookLogin;

    private void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
            //Rest of Awake code
            SaveSystem.Init();
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }
    public void CustomStart()
    {  
        myOrganManager = GameManager.gameManager.organManager;
    }


    private void save()
    {
        SaveObject saveObject = new SaveObject
        {
            energyPoints = energyPoints,
            Organs = myOrganManager.organTypes,
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
        Debug.Log("Saved Data");

        // guardar en la nube
    }

    private void Load()
    {
        string saveString = SaveSystem.Load();
        if (!string.IsNullOrEmpty(saveString))
        {
            LoadedObject = JsonUtility.FromJson<SaveObject>(saveString);
            SetStats();
            Debug.Log("Load Data");
        }
        else
        {
            Debug.LogWarning("No save");
        }
    }

    void SetStats()
    {
        if(LoadedObject != null)
        {
            if(LoadedObject.Organs != null)
            {
                if(myOrganManager != null)
                {
                    myOrganManager.organTypes = LoadedObject.Organs;
                }  
            }
            energyPoints = LoadedObject.energyPoints;
        }
    }

    public class SaveObject
    {
        public double energyPoints;
        public OrganManager.OrganType[] Organs;
        
    }


    /// DELETE THIS METHOD////TODO

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        save();
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        Load();
    //    }
    //    if(Input.GetKeyDown(KeyCode.M))
    //    {
    //        SetStats();
    //    }
     
    //}




}
