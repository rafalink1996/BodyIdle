using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idle;
using BreakInfinity;
public class CR_Data : MonoBehaviour
{


    [Header("INSTANCE")]
    public static CR_Data data;



    // GAME SETTINGS //
    public enum Languages {English, Spanish }
    public Languages _language;

    public float _musicVolume { get; private set; }
    public float _SFXVolume { get; private set; }

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
                      }
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
                      }
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
        public BigDouble pointMultiplierCost;
        public float pointsMultiplier;

        [System.Serializable]
        public struct Platletes
        {
            public int platletNumber;
            public BigDouble plateletInitialCost;
            public BigDouble plateletCost;
        }
        public Platletes plateletInfo;
       
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
                public float initialCellCost;
                public float currentCellCost;
                public float growthRate;

            }
            [Header("LISTS")]
            public cellsType[] CellTypes;
        }

        [Space(10)]
        public List<OrganInfo> organs = new List<OrganInfo>();

    }
    #endregion Serialized Classes
}
