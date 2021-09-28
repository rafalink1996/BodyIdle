using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatletManager : MonoBehaviour
{
    #region Pooling
    [SerializeField] Transform PoolHolder;
    [System.Serializable]
    public class PlateletPool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<PlateletPool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    string[] platletTags = new string[]{
       "SmallPlatelet_",
       "MediumPlatelet_",
       "BigPlatelet_"
        };

    void InsantiatePools()
    {
        foreach (PlateletPool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(PoolHolder.transform);
                ObjectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, ObjectPool);
            //Debug.Log("Added pool with key " + pool.tag);
        }
    }

    GameObject SpawnFroomPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool With tag" + tag + " deosn't exist");
            return null;
        }
        GameObject ObjectToSpawn = PoolDictionary[tag].Dequeue();
        ObjectToSpawn.SetActive(true);
        PoolDictionary[tag].Enqueue(ObjectToSpawn);
        return ObjectToSpawn;
    }
    #endregion Pooling

    [Header("References")]
    GameManager GM;
    OrganManager myOrganManager;

    [System.Serializable]
    public class plataletObject
    {
        public string name;
        public int position;
        public GameObject PlatletHolder;
        public List<GameObject> SmallPlatelets;
        public List<GameObject> MediumPlatelets;
        public List<GameObject> BigPlatelets;
    }
    [Header("PlatleteObjects")]
    public plataletObject[] plateletScreens;

    [SerializeField] int currentScreen;

    [Header("Conditions")]
    public bool canBuy = true;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        InsantiatePools();
    }
    public void CustomStart()
    {
        GM = GameManager.gameManager;
        if (GM != null)
            myOrganManager = GM.organManager;

        InstantiatePlatalettes(0, myOrganManager.activeOrganType);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            BuyPlatelet();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InstantiatePlatalettes(0, myOrganManager.activeOrganType);
        }
    }

    public void BuyPlatelet()
    {
        if (canBuy)
        {
            if (GM.pointsManager.totalPoints >= myOrganManager.organTypes[myOrganManager.activeOrganType].plateletCost)
            {
                if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[2].Quantity < 5)
                {
                    GM.pointsManager.ManagePoints(-(myOrganManager.organTypes[myOrganManager.activeOrganType].plateletCost));
                    myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity++;
                    StartCoroutine(CheckPlateletInstances());
                }
            }
        }
    }
    void SpawnMerger()
    {

    }

    public void InstantiatePlatalettes(int screen /* 0 - 2 */, int organTypeId)
    {
        if (organTypeId == 12)
        {
            Debug.LogWarning("Choose Organ Screen doesn't have platelets");
            return;
        }

        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }

        Debug.Log("Updating Screen " + screenReference + "-Position "+ plateletScreens[screenReference].position + "- With Organ Type " + organTypeId);
        for (int s = plateletScreens[screenReference].SmallPlatelets.Count; s < myOrganManager.organTypes[organTypeId].platletSizes[0].Quantity; s++)
        {
            GameObject SmallPlatelet = SpawnFroomPool(platletTags[0] + screenReference);
            SmallPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
            plateletScreens[screenReference].SmallPlatelets.Add(SmallPlatelet);
            SmallPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
        }
        for (int m = plateletScreens[screenReference].MediumPlatelets.Count; m < myOrganManager.organTypes[organTypeId].platletSizes[1].Quantity; m++)
        {
            GameObject MedPlatelet = SpawnFroomPool(platletTags[1] + screenReference);
            MedPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
            plateletScreens[screenReference].MediumPlatelets.Add(MedPlatelet);
            MedPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
        }
        for (int b = plateletScreens[screenReference].BigPlatelets.Count; b < myOrganManager.organTypes[organTypeId].platletSizes[2].Quantity; b++)
        {
            GameObject BigPlatelet = SpawnFroomPool(platletTags[2] + screenReference);
            BigPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
            plateletScreens[screenReference].BigPlatelets.Add(BigPlatelet);
            BigPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
        }

    }

    IEnumerator CheckPlateletInstances()
    {
        canBuy = false;
        bool SmallCellMerge = false;
        if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity >= 5)
        {
            SmallCellMerge = true;
            SpawnMerger();
        }
        while (SmallCellMerge)
        {
            //Merge Small Cells
            yield return null;
        }
        bool MediumCellMerge = false;
        if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity >= 5)
        {
            MediumCellMerge = true;
            SpawnMerger();
        }
        while (MediumCellMerge)
        {
            //Merge Medium Cells
            yield return null;
        }
        canBuy = true;
    }

    public void UpdatePlateletHolderPosition(bool Left)
    {
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (Left)
            {
                plateletScreens[i].position--;
                if (plateletScreens[i].position < 0)
                {
                    plateletScreens[i].position = 2;
                }
            }
            else
            {
                plateletScreens[i].position++;
                if (plateletScreens[i].position > 2)
                {
                    plateletScreens[i].position = 0;
                }
            }
        }
    }


}
