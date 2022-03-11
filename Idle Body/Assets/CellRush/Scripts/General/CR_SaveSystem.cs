
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using System.IO;
using System;
using Idle;

public class CR_SaveSystem : MonoBehaviour
{
    public static CR_SaveSystem instance;

    string _pathName = "/save.txt";
    [SerializeField] CR_OfflineProgress _OfflineProgress;
    float AutoSaveCountdown = 6;
    [SerializeField] float AutosaveSeconds = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        if (_OfflineProgress == null) _OfflineProgress = FindObjectOfType<CR_OfflineProgress>();
    }

    private void Update()
    {
        Autosave();
    }

    void Autosave()
    {
        if (AutoSaveCountdown > 0)
        {
            AutoSaveCountdown -= Time.deltaTime;
            save();
        }
        else
        {
            AutoSaveCountdown = AutosaveSeconds;
        }
    }


    public void save()
    {
        string path;
        CR_Data.data.saveCount += 1;
        if (Application.isEditor)
        {
            path = Application.dataPath;
        }
        else
        {
            path = Application.persistentDataPath;
        }
        SaveObject newSaveObject = ConstructSaveObject();
        string json = JsonUtility.ToJson(newSaveObject);
        File.WriteAllText(path + _pathName, json);
        Debug.Log("Saved");
    }


    public void Load(out bool saveExists)
    {
        string path;
        saveExists = false;
        if (Application.isEditor)
        {
            path = Application.dataPath;
        }
        else
        {
            path = Application.persistentDataPath;
        }
        if (File.Exists(path + _pathName))
        {
            saveExists = true;
            string json = File.ReadAllText(path + _pathName);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            SetSaveData(saveObject);
            Debug.Log("Loaded");
        }
        else
        {
            if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance.GameStart();
            Debug.Log("No save data");
        }
    }


    void SetSaveData(SaveObject saveObject)
    {
        var data = CR_Data.data;
        DateTime timeFromJson = JsonUtility.FromJson<JsonDateTime>(saveObject._saveTime);
        timeFromJson.ToLocalTime();
        data._lastSesionTime = timeFromJson;
        data.SetEnergy(saveObject._energy);
        data.SetLanguage(saveObject._language);
        data.SetComplexity(saveObject._complexity);
        data.SetMaxComplexity(saveObject._maxComplexity);
        data.SetPremium(saveObject._premium);
        data.SetMusicVolume(saveObject._musicVolume);
        data.SetSFXVolume(saveObject._SFXVolume);
        data.SetNotifications(saveObject._notifications);

        data.organTypes = saveObject.organTypes;
        data.GetEnergyPerSecond();

    }

    SaveObject ConstructSaveObject()
    {
        var data = CR_Data.data;
        var time = DateTime.Now; ;
        string path;
        if (Application.isEditor)
        {
            path = Application.dataPath;
        }
        else
        {
            path = Application.persistentDataPath;
        }
        if (File.Exists(path + _pathName))
        {
            if (!data._offlineProgressCollected)
            {
                if (data._lastSesionTime != null)
                {
                    time = data._lastSesionTime;
                }

            }
        }
        var jsonDateTime = JsonUtility.ToJson((JsonDateTime)time);



        SaveObject saveObject = new SaveObject
        {
            _saveTime = jsonDateTime,
            _saveNumber = data.saveCount,
            _notifications = data._notifications,
            _language = data._language,
            _musicVolume = data._musicVolume,
            _SFXVolume = data._SFXVolume,
            _energy = data._energy,
            _complexity = data._complexity,
            _maxComplexity = data._maxComplexity,
            _premium = data._premium,
            organTypes = data.organTypes
        };
        return saveObject;
    }

    [Serializable]
    public struct JsonDateTime
    {
        public long value;
        public static implicit operator DateTime(JsonDateTime jdt)
        {
            // Debug.Log("Converted to time");
            return DateTime.FromFileTimeUtc(jdt.value);
        }
        public static implicit operator JsonDateTime(DateTime dt)
        {
            // Debug.Log("Converted to JDT");
            JsonDateTime jdt = new JsonDateTime();
            jdt.value = dt.ToFileTimeUtc();
            return jdt;
        }
    }



    [System.Serializable]
    public class SaveObject
    {
        public BigDouble _saveNumber;
        public string _saveTime;

        public CR_Data.Languages _language;
        public bool _notifications;

        public float _musicVolume;
        public float _SFXVolume;

        public BigDouble _energy;
        public int _complexity;
        public int _maxComplexity;
        public double _premium;

        public CR_Data.OrganType[] organTypes;

    }
}
