using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Idle;
using BreakInfinity;
public class CR_Data : MonoBehaviour
{
    [System.Serializable]
    public class OrganType
    {
        [Space(20)]
        [Header("ID")]
        public string Name;
        public Sprite border;
        public Sprite OrganSprite;
        public AnimatorOverrideController borderAnimation;
        public Color backgroundColor;

        public bool unlocked;

        [Space(20)]
        [Header("COSTS")]

        public double[] PointCost;
        public int[] ComplexityCost;
        public double pointMultiplierCost;
        public float pointsMultiplier;

        [Space(20)]
        [Header("PLATLETES")]
        public int platletNumber;
        public BigDouble plateletInitialCost;
        public BigDouble plateletCost;



        [System.Serializable]
        public class OrganInfo
        {
            public int id;
            public bool unlocked;
            public float infectionChance;
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
                public int id;
                public List<CellSizes> cellSizes;
                [Space(10)]
                [Header("Cell Type Cost")]
                public float initialCellCost;
                public float currentCellCost;

            }
            [System.Serializable]
            public class Pathogens
            {
                public int id;
                public float health = 1;
            }
            [Space(10)]
            [Header("Cells List")]
            public cellsType[] CellTypes;
            public List<Pathogens> pathogensList;
        }
        [Space(10)]
        public List<OrganInfo> organs = new List<OrganInfo>();

    }

    [Header("INSTANCE")]
    public static CR_Data data;


    public BigDouble _energy { get; private set; }
    public BigDouble _energyPerSecond { get; private set; }
    public int _complexity { get; private set; }
    public int _maxComplexity { get; private set; } = 100;
    public double _premium { get; private set; }


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
    #endregion SET METHODS

    public void AddNewOrgan(int organType)
    {
        organTypes[organType].unlocked = true;
        OrganType.OrganInfo newOrgan = new OrganType.OrganInfo
        {
            unlocked = true,
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
            pathogensList = new List<OrganType.OrganInfo.Pathogens>(),
        };
        organTypes[organType].organs.Add(newOrgan);
    }
}
