using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    [HideInInspector]
    public CellSpawner cellSpawner;
    public PathogenSpawner pathogenSpawner;
    Coroutine changeOrganCoroutine;

    [System.Serializable]
    public class OrganType
    {
        public string Name;
        public double[] PointCost;
        public float[] ComplexityCost;
        public double pointMultiplierCost;
        public float pointsMultiplier;
        public bool unlocked;
        public Sprite border;
        public AnimatorOverrideController borderAnimation;
        public Color backgroundColor;

        [System.Serializable]
        public class OrganInfo
        {
            public string name;
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
        public List<OrganInfo> organs = new List<OrganInfo>();
    }
    public OrganType[] organTypes;
    public List<int> OrganOrder;

    public int activeOrganType = 0;
    public int activeOrganID = 0;



    // Start is called before the first frame update
    public void CustomStart()
    {
        cellSpawner = FindObjectOfType<CellSpawner>();
        pathogenSpawner = FindObjectOfType<PathogenSpawner>();
        for (int o = 0; o < organTypes[activeOrganType].organs.Count; o++)
        {
            for(int t = 0; t < organTypes[activeOrganType].organs[o].CellTypes.Length; t++)
            {
                organTypes[activeOrganType].organs[o].CellTypes[t].currentCellCost = CalculateCosts(o, t);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ChangeOrganRoutine(1));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ChangeOrganRoutine(0));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddNewOrgan(0);
        }
    }
    public void AsignNewOrganID(int id, out bool OrganUnlocked)
    {
        OrganUnlocked = false;
        if (organTypes[activeOrganType].organs[id].unlocked)
        {
            activeOrganID = id;
            OrganUnlocked = true;
        }
    }
    IEnumerator ChangeOrganRoutine(int id)
    {
        if (activeOrganID != id && organTypes[activeOrganType].organs[id].unlocked)
        {
            cellSpawner.DestroyCells();
            activeOrganID = id;
            yield return new WaitForSeconds(1f);
            cellSpawner.InstantiateCells();
        }

    }
    public float CalculateCosts(int organId, int cellType)
    {
        float totalCells = GetCellTypeTotal(organId, cellType);
        float cost = organTypes[activeOrganType].organs[organId].CellTypes[cellType].initialCellCost + ((totalCells * (totalCells + 1)) / 2);
        return cost;
    }
    float GetCellTypeTotal(int organId, int celltype = 0)
    {
        float totalCells = 0;
        for (int a = 0; a < organTypes[activeOrganType].organs[organId].CellTypes[celltype].cellSizes.Count; a++)
        {
            for (int b = 0; b < organTypes[activeOrganType].organs[organId].CellTypes[celltype].cellSizes[a].CellsInfos.Count; b++)
            {
                totalCells += Mathf.Pow(10, a);
            }
        }
        return totalCells;
    }


    public void AddNewOrgan(int organType)
    {
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
                       }
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
}
