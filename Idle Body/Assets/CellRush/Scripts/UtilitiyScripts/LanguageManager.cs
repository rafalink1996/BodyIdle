using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [System.Serializable]
    public struct OrganName
    {
        public string name;
        public string[] singular;
        public string[] prural;
    }
    public OrganName[] organNames;

    [System.Serializable]
    public struct OrganismViewTexts
    {
        public string[] Buy;
        public string[] SeeOrgans;
    }
    public OrganismViewTexts organismViewTexts;

    [System.Serializable]
    public struct OrganViewTexts
    {
        public string[] UpgradeMultiplier;
        public string[] BuyPlatles;
        public string[] Boosters;
        public string[] EnergyProduction;
        public string[] Sell;
        public string[] Owned;
    }
    public OrganViewTexts organViewTexts;

    [System.Serializable]
    public struct CellViewTexts
    {
        public string[] seeCells;
        public string[] cells;
        public string[] buyCells;
        public string[] redBloodCells;
        public string[] whiteBloodCells;
        public string[] helperBloodCells;
    }
    public CellViewTexts cellViewTexts;

    [System.Serializable]
    public struct OfflineProgressTexts
    {
        public string[] workedHard;
        public string[] hibernation;
        public string[] made;
    }
    public OfflineProgressTexts offlineProgressTexts;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public string translateOrgan(int OrganType, CR_Data.Languages language, bool prural = false)
    {
        if (prural)
        {
            return organNames[OrganType].prural[(int)language];
        }
        else
        {
            return organNames[OrganType].singular[(int)language];
        }

        
        
    }

}
