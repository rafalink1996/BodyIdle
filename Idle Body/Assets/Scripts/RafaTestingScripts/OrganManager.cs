using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    [HideInInspector]
    public CellSpawner cellSpawner;
    [System.Serializable]
    public class OrganInfo
    {
        public string name;
        public int id;
        public bool unlocked;
        public Sprite border;
        public AnimatorOverrideController borderAnimation;
        public Color backgroundColor;
        public float pointsMultiplier;

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
        [Space(10)]
        [Header("Cells List")]
        public cellsType[] CellTypes;
    }

    public List<OrganInfo> organs = new List<OrganInfo>();
    public int activeOrganID = 0;



    // Start is called before the first frame update
    void Start()
    {
        cellSpawner = FindObjectOfType<CellSpawner>();
        cellSpawner.InstantiateCells();
        for (int o = 0; o < organs.Count; o++)
        {
            for(int t = 0; t < organs[o].CellTypes.Length; t++)
            {
                organs[o].CellTypes[t].currentCellCost = CalculateCosts(o, t);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ChangeOrgan(1));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ChangeOrgan(0));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddNewOrgan();
        }
    }

    IEnumerator ChangeOrgan(int id)
    {
        if (activeOrganID != id && organs[id].unlocked)
        {
            cellSpawner.DestroyCells();
            activeOrganID = id;
            //cellSpawner.organId = id;
            yield return new WaitForSeconds(1f);
            cellSpawner.InstantiateCells();
            //organs[activeOrganID].currentRedCellCost = CalculateCosts();
        }
    }
    public float CalculateCosts(int organId, int cellType)
    {
        float totalCells = GetCellTypeTotal(organId, cellType);
        float cost = organs[organId].CellTypes[cellType].initialCellCost + ((totalCells * (totalCells + 1)) / 2);
        return cost;
    }
    float GetCellTypeTotal(int organId, int celltype = 0)
    {
        float totalCells = 0;
        for (int a = 0; a < organs[organId].CellTypes[celltype].cellSizes.Count; a++)
        {
            for (int b = 0; b < organs[organId].CellTypes[celltype].cellSizes[a].CellsInfos.Count; b++)
            {
                totalCells += Mathf.Pow(10, a);
            }
        }
        return totalCells;
    }


    void AddNewOrgan()
    {
        OrganInfo newOrgan = new OrganInfo
        {
            pointsMultiplier = 1,
            unlocked = true,
            CellTypes = new OrganInfo.cellsType[]
              {
                   new OrganInfo.cellsType
                   {
                       name = "Red Cells",
                       cellSizes = new List<OrganInfo.cellsType.CellSizes>
                       {
                           new OrganInfo.cellsType.CellSizes
                           {
                               name = "Small red blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                             new OrganInfo.cellsType.CellSizes
                           {
                               name = "Medium red blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                               new OrganInfo.cellsType.CellSizes
                           {
                               name = "Big red blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           }
                       }
                   },
                   new OrganInfo.cellsType
                   {
                       name = "White Cells",
                       cellSizes = new List<OrganInfo.cellsType.CellSizes>
                       {
                           new OrganInfo.cellsType.CellSizes
                           {
                               name = "Small White blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                             new OrganInfo.cellsType.CellSizes
                           {
                               name = "Medium White blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                               new OrganInfo.cellsType.CellSizes
                           {
                               name = "Big White blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           }
                       }
                   },
                   new OrganInfo.cellsType
                   {
                       name = "Helper Cells",
                       cellSizes = new List<OrganInfo.cellsType.CellSizes>
                       {
                           new OrganInfo.cellsType.CellSizes
                           {
                               name = "Small Helper blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                             new OrganInfo.cellsType.CellSizes
                           {
                               name = "Medium Helper blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           },
                               new OrganInfo.cellsType.CellSizes
                           {
                               name = "Big Helper blood cell",
                               CellsInfos = new List<OrganInfo.cellsType.CellSizes.CellInfo>(),
                           }
                       }
                   },
              },
        };
        organs.Add(newOrgan);

    }
}
