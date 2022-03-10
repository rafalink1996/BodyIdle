using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idle;
using BreakInfinity;
using System;

public class CR_Data : MonoBehaviour
{
    [Header("INSTANCE")]
    public static CR_Data data;



    // GAME SETTINGS //
    public enum Languages { English, Spanish, NumOfLanguages }
    public Languages _language;
    public bool _notifications { get; private set; }

    public float _musicVolume { get; private set; }
    public float _SFXVolume { get; private set; }

    public BigDouble saveCount;
    public System.DateTime _lastSesionTime;

    // GAME SETTINGS //


    // GAME VARIABLES //
    public BigDouble _energy { get; private set; }
    public BigDouble _energyPerSecond { get; private set; }
    public int _complexity { get; private set; }
    public int _maxComplexity { get; private set; } = 100;
    public double _premium { get; private set; }
    // GAME VARIABLES //


    [Header("ORGAN DATA")]
    public OrganType[] organTypes;

    // OTHER VARIABLES //
   

    public static event Action onLanguageChange;

    private void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < organTypes.Length; i++)
        {
           CalculateMultiplier(i);
        }
    }

    public void AddNewOrgan(int organType)
    {
        organTypes[organType].unlocked = true;
        OrganType.OrganInfo newOrgan = new OrganType.OrganInfo
        {
            CellTypes = new OrganType.OrganInfo.cellsType[]
              {
                  new OrganType.OrganInfo.cellsType
                  {
                      name = "Red Cells",
                      cellSizes = new List<OrganType.OrganInfo.cellsType.CellSizes>
                      {
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Small red blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Medium red blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Big red blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          }
                      },
                      initialCellCost = Mathf.Pow(7.5f, organType),
                      currentCellCost = Mathf.Pow(7.5f, organType),
                      growthRate = 1.05f

                  },
                  new OrganType.OrganInfo.cellsType
                  {
                      name = "White Cells",
                      cellSizes = new List<OrganType.OrganInfo.cellsType.CellSizes>
                      {
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Small White blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Medium White blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Big White blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          }
                      },
                      initialCellCost = Mathf.Pow(7.5f, organType),
                      currentCellCost = Mathf.Pow(7.5f, organType),
                      growthRate = 1.01f


                  },
                  new OrganType.OrganInfo.cellsType
                  {
                      name = "Helper Cells",
                      cellSizes = new List<OrganType.OrganInfo.cellsType.CellSizes>
                      {
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Small Helper blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Medium Helper blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          },
                          new OrganType.OrganInfo.cellsType.CellSizes
                          {
                              name = "Big Helper blood cell",
                              CellsInfos = new List<OrganType.OrganInfo.cellsType.CellSizes.CellInfo>(),
                          }
                      },
                      initialCellCost = Mathf.Pow(30, organType),
                      currentCellCost = Mathf.Pow(30, organType),
                      growthRate = 1.05f
                  },
              },
        };
        organTypes[organType].organs.Add(newOrgan);
    }

    

    #region SET METHODS
    public void SetEnergy(BigDouble energy)
    {
        _energy = energy;
        if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance._overlayUI.UpdateEnergy();
    }
    public void SetEnergyPerSecond(BigDouble energyPerSecond)
    {
        _energyPerSecond = energyPerSecond;
        if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance._overlayUI.UpdateEnergy();
    }
    public void SetComplexity(int complexity)
    {
        _complexity = complexity;
        if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance._overlayUI.UpdateComplexity();
    }
    public void SetMaxComplexity(int maxComplexity)
    {
        _maxComplexity = maxComplexity;
        if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance._overlayUI.UpdateComplexity();

    }
    public void SetPremium(double premium)
    {
        _premium = premium;
        if (CR_Idle_Manager.instance != null) CR_Idle_Manager.instance._overlayUI.UpdatePremium();

    }

    public void SetLanguage(Languages language)
    {
        _language = language;
        onLanguageChange?.Invoke();
    }

    public void SetNotifications(bool on)
    {
        _notifications = on;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        _SFXVolume = volume;
    }
    #endregion SET METHODS
    #region GetMethods
    public int GetTotalCells(int OrganType, int OrganNumber, int cellType)
    {
        if ((organTypes.Length - 1) < OrganType) return 0;
        if ((organTypes[OrganType].organs.Count - 1) < OrganNumber) return 0;
        int totalCells = 0;
        for (int a = 0; a < organTypes[OrganType].organs[OrganNumber].CellTypes[cellType].cellSizes.Count; a++)
        {
            for (int b = 0; b < organTypes[OrganType].organs[OrganNumber].CellTypes[cellType].cellSizes[a].CellsInfos.Count; b++)
            {
                switch (cellType)
                {
                    case 0:
                    case 1:
                        totalCells += (int)Mathf.Pow(10, a);
                        break;
                    case 2:
                        totalCells += (int)Mathf.Pow(3, a);
                        break;
                }

            }
        }
        return totalCells;
    }

    public int GetTotalOrganTypeCells(int organType, int cellType)
    {
        if ((organTypes.Length - 1) < organType) return 0;
        int totalCells = 0;
        for (int a = 0; a < organTypes[organType].organs.Count; a++)
        {
            for (int b = 0; b < organTypes[organType].organs[a].CellTypes[cellType].cellSizes.Count; b++)
            {
                for (int c = 0; c < organTypes[organType].organs[a].CellTypes[cellType].cellSizes[b].CellsInfos.Count; c++)
                {
                    switch (cellType)
                    {
                        case 0:
                        case 1:
                            totalCells += (int)Mathf.Pow(10, b);
                            break;
                        case 2:
                            totalCells += (int)Mathf.Pow(3, b);
                            break;
                    }
                }
            }
        }
        return totalCells;
    }

    public BigDouble GetEnergyPerSecond()
    {
        BigDouble energyPerSecond = 0;
        for (int i = 0; i < organTypes.Length; i++)
        {
            for (int o = 0; o < organTypes[i].organs.Count; o++)
            {
                int redCells = GetTotalCells(i, o, 0);
                energyPerSecond += redCells * organTypes[i].pointsMultiplier;
            }
        }
        SetEnergyPerSecond(energyPerSecond);
        return energyPerSecond;
    }
    public BigDouble GetEnergyPerSecond(int organType, int organNumber)
    {
        BigDouble energyPerSecond = 0;
        int redCells = GetTotalCells(organType, organNumber, 0);
        energyPerSecond += redCells * organTypes[organType].pointsMultiplier;
        return energyPerSecond;
    }

    public int GetOrganAmount(int organType)
    {
        return organTypes[organType].organs.Count;
    }
    #endregion GetMethods
    #region CalculateMethods
    public BigDouble CalculateMultiplier(int organType)
    {
        BigDouble returnValue = organTypes[organType].basePointsMultiplier;
        for (int i = 0; i < organTypes[organType].upgrades.Length; i++)
        {
            if (i >= CR_Idle_Manager.instance.organTypeAsstes[organType].upgrades.Count) break;
            if (organTypes[organType].upgrades[i])
            {
                switch (CR_Idle_Manager.instance.organTypeAsstes[organType].upgrades[i].type)
                {
                    case CR_Idle_Manager.OrganUpgrade.UpgradeType.Multiply:
                        returnValue *= CR_Idle_Manager.instance.organTypeAsstes[organType].upgrades[i].amount;
                        break;
                    case CR_Idle_Manager.OrganUpgrade.UpgradeType.MultiplyAndOrganPower:
                        returnValue *= Mathf.Pow(CR_Idle_Manager.instance.organTypeAsstes[organType].upgrades[i].amount, CR_Data.data.organTypes[organType].organs.Count) ;
                        break;
                    case CR_Idle_Manager.OrganUpgrade.UpgradeType.Power:
                        returnValue = BigDouble.Pow(returnValue, CR_Idle_Manager.instance.organTypeAsstes[organType].upgrades[i].amount);
                        break;
                    default:
                        break;
                }
            }
        }
        organTypes[organType].pointsMultiplier = returnValue;
        _energyPerSecond = GetEnergyPerSecond();

        return returnValue;

    }
    #endregion CalculateMethods

    #region Serialized Classes
    [System.Serializable]
    public class OrganType
    {
        [Header("ID")]
        public string Name;
        public bool unlocked;
        [Header("COSTS")]
        public BigDouble[] PointCost;
        public int[] ComplexityCost;

        [Header("MULTIPLIER")]
        //public BigDouble pointMultiplierCost;
        //public BigDouble pointsMultiplierInitialCost;
        public int multiplierLevel;
        public BigDouble basePointsMultiplier;
        public BigDouble pointsMultiplier;


        [System.Serializable]
        public struct Platletes
        {
            public int platletNumber;
            public BigDouble plateletInitialCost;
            public BigDouble plateletCost;
        }
        [Header("PLATELETS")]
        public Platletes plateletInfo;

        [Header("UPGRADES")]
        public bool[] upgrades;

        [System.Serializable]
        public class OrganInfo
        {
            [System.Serializable]
            public class cellsType
            {
                [System.Serializable]
                public class CellSizes
                {
                    [System.Serializable]
                    public class CellInfo
                    {
                        public float health = 1;
                        public float maxHealth = 1;
                        public float timer = 0;
                        public bool alive = true;
                    }
                    public string name;
                    public List<CellInfo> CellsInfos;
                }
                public string name;
                public List<CellSizes> cellSizes;
                [Header("CELL COSTS")]
                public BigDouble initialCellCost;
                public BigDouble currentCellCost;
                public float growthRate;

            }
            [Header("CELL LISTS")]
            public cellsType[] CellTypes;
            [Header("INFECTION")]
            public bool infected;
            public CR_PathogenSystem.Infection infection;
            public int InfectionAmount;
           
        }

        [Space(10)]
        public List<OrganInfo> organs = new List<OrganInfo>();

    }
    #endregion Serialized Classes
}
