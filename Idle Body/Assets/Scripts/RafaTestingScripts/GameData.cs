using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameData : MonoBehaviour
{
    public float energyPoints;
    public static GameData data;
    OrganManager myOrganManager;
    SaveObject LoadedObject;
  

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
                myOrganManager.organTypes = LoadedObject.Organs;
            }
            energyPoints = LoadedObject.energyPoints;
        }
    }

    private class SaveObject
    {
        public float energyPoints;
        public OrganManager.OrganType[] Organs;
        
    }


    /// DELETE THIS METHOD////TODO

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            SetStats();
        }
     
    }




}
