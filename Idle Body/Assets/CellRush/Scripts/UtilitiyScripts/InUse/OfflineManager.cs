using UnityEngine;
using System;
using PlayFab;

public class OfflineManager : MonoBehaviour
{
    [SerializeField] string TestString;
    [SerializeField] string Compare;

    Manager manager;

    public void GetReferences()
    {
        manager = FindObjectOfType<Manager>();
    }

    void GetOfflineSeconds(DateTime lastTime, out float seconds, out string timeString)
    {
        DateTime currentTime = DateTime.UtcNow;
        TimeSpan diference = currentTime.Subtract(lastTime);
        var rawTime = (float)diference.TotalSeconds;
        seconds = rawTime;
        TimeSpan timer = TimeSpan.FromSeconds(rawTime);
        timeString = $"{timer:dd\\:hh\\:mm\\:ss} ";
    }

    bool OfflineReward(float seconds ,out double offlinePoints)
    {
        offlinePoints = 0;
        if(seconds < 120)
        {
            return false;
        }
        else
        {
            offlinePoints = seconds / 10 * GameData.data.SavedPointsPerSecond;
            return true;
        }
    }

    public void StartTimeData()
    {
        if (GameData.data.PlayfabLogin)
        {
         
        }
        else
        {
            GetOfflineSeconds(GameData.data.lastSesionTime, out float seconds, out string timeString);
            OfflineReward(seconds, out double offlinePoints);
        } 
    }
}
