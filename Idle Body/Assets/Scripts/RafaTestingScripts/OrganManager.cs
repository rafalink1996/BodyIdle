using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    [HideInInspector]
    public CellSpawner cellSpawner;
   [System.Serializable]
    public class OrganInfo {
        public string name;
        public int id;
        public Sprite border;
        public AnimatorOverrideController borderAnimation;
        public Color backgroundColor;
        public float pointsMultiplier;
        [System.Serializable]
        public class CellInfo
        {
            public int health = 1;
            public int maxHealth = 1;
            public float timer = 0;
            public bool alive = true;
        }
        [Space (10)]
        [Header ("Red Blood Cells")] 
        public List<CellInfo> smallRedCells;
        public List<CellInfo> medRedCells;
        public List<CellInfo> bigRedCells;
    }
    public List<OrganInfo> organs;
    
    // Start is called before the first frame update
    void Start()
    {
        cellSpawner = FindObjectOfType<CellSpawner>();
        cellSpawner.InstantiateCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(ChangeOrgan(1));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ChangeOrgan(0));
        }
    }

    IEnumerator ChangeOrgan(int id)
    {
        if (cellSpawner.organId != id)
        {
            cellSpawner.DestroyCells();
            cellSpawner.organId = id;
            yield return new WaitForSeconds(1f);
            cellSpawner.InstantiateCells();
        }
    }
}
