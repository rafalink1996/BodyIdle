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
        public float initialRedCellCost;
        public float currentRedCellCost;
        [System.Serializable]
        public class CellInfo
        {
            public int health = 1;
            public int maxHealth = 1;
            public float timer = 0;
            public bool alive = true;
        }
        [System.Serializable]
        public class cellsList
        {
           
            public string name;
            public int id;
            public List<CellInfo> Cells;
        }
        [Space(10)]
        [Header("Cells List")]
        public cellsList[] lists;
        
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
            organs[o].currentRedCellCost = CalculateCosts(o);
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
    public float CalculateCosts(int organId)
    {
        float totalCells = GetTotalCells(organId);
        float cost = organs[activeOrganID].initialRedCellCost + ((totalCells * (totalCells + 1)) / 2);
        return cost;
    }
    float GetTotalCells(int organId)
    {
        float totalCells = 0;

        for (int c = 0; c < 3; c++)
        {

            for (int r = 0; r < organs[organId].lists[c].Cells.Count; r++)
            {
                if (c == 0)
                {
                    totalCells++;
                }
                else if (c == 1)
                {
                    totalCells += 10;
                }
                else if (c == 2)
                {
                    totalCells += 100;
                }
            }
        }

        return totalCells;
    }


    void AddNewOrgan()
    {
        OrganInfo newOrgan = new OrganInfo
        {
            pointsMultiplier = 1,
            lists = new OrganInfo.cellsList[]
              {
                   new OrganInfo.cellsList
                   {
                       name = "Small Red Blood Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Medium Red Blood Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Big Red Blood Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Small white Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Medium white Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Big white Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Small Helper Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Medium Helper Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },
                   new OrganInfo.cellsList
                   {
                       name = "Big Helper Cells",
                       Cells = new List<OrganInfo.CellInfo>(),
                   },

              },
        };
        organs.Add(newOrgan);

    }
}
