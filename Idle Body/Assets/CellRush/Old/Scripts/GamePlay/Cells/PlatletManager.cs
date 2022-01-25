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
    [SerializeField] GameObject BuyButton;
    [System.Serializable]
    public class plataletObject
    {
        public string name;
        public int position;
        public GameObject PlatletHolder;
        public string Organ;
        public List<GameObject> SmallPlatelets;
        public List<GameObject> MediumPlatelets;
        public List<GameObject> BigPlatelets;
    }
    [Header("PlatleteObjects")]
    public plataletObject[] plateletScreens;

    [SerializeField] int currentScreen;

    [Header("Conditions")]
    public bool canBuy = true;

    [Header("Coroutines")]
    Coroutine plataletInstancesRoutine;

    [Header("Merging")]
    public bool Merging = false;
    GameObject CurrentMerger;
    [SerializeField] GameObject MergeObject;




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
                    plataletInstancesRoutine = StartCoroutine(CheckPlateletInstances());
                    if(BuyButton != null)
                    {
                        PlateletButtonEffect();
                    }
 
                    AudioManager.Instance.Play("UI_Synth");
                }
                else
                {
                    Debug.LogWarning("Max platelets reached");
                }
            }
            else
            {
                Debug.LogWarning("Not enough points to buy platelet = " + myOrganManager.organTypes[myOrganManager.activeOrganType].plateletCost);
            }
        }
        else
        {
            Debug.LogWarning("Cant buy");
        }
    }
    void PlateletButtonEffect()
    {
        float TweenTime = 2f;
        LeanTween.cancel(BuyButton);
        //LTDescr l = LeanTween.scale(BuyButton, new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeInOutExpo);
        //l.setOnComplete(finishEffect);
        //void finishEffect()
        //{
        //    LeanTween.scale(BuyButton, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeInOutExpo);
        //}

        LeanTween.scale(BuyButton, new Vector3(1.2f, 1.2f, 1.2f), TweenTime / 4).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(BuyButton, new Vector3(1, 1, 1), TweenTime / 4).setEase(LeanTweenType.easeInExpo).setDelay(TweenTime / 4);
    }
    public void InstantiatePlatalettes(int screen /* 0 - 2 */, int organTypeId, bool All = true)
    {
        CancelMerge();
        if (organTypeId == 12)
        {
            Debug.LogWarning("Choose Organ Screen doesn't have platelets");
            plateletScreens[screen].Organ = "New Organ";
            return;
        }
        else
        {
            plateletScreens[screen].Organ = myOrganManager.organTypes[organTypeId].Name;
        }

        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }
        if (All)
        {
            Debug.Log("Updating Screen " + screenReference + "-Position " + plateletScreens[screenReference].position + "- With Organ Type " + organTypeId);
            for (int s = plateletScreens[screenReference].SmallPlatelets.Count; s < myOrganManager.organTypes[organTypeId].platletSizes[0].Quantity; s++)
            {
                GameObject SmallPlatelet = SpawnFroomPool(platletTags[0] + screenReference);
                SmallPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].SmallPlatelets.Add(SmallPlatelet);
                SmallPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
                SmallPlatelet.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    platelet.CustomStart(0, false, false);
                }
            }
            for (int m = plateletScreens[screenReference].MediumPlatelets.Count; m < myOrganManager.organTypes[organTypeId].platletSizes[1].Quantity; m++)
            {
                GameObject MedPlatelet = SpawnFroomPool(platletTags[1] + screenReference);
                MedPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].MediumPlatelets.Add(MedPlatelet);
                MedPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
                MedPlatelet.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    platelet.CustomStart(1, false, false);
                }
            }
            for (int b = plateletScreens[screenReference].BigPlatelets.Count; b < myOrganManager.organTypes[organTypeId].platletSizes[2].Quantity; b++)
            {
                GameObject BigPlatelet = SpawnFroomPool(platletTags[2] + screenReference);
                BigPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].BigPlatelets.Add(BigPlatelet);
                BigPlatelet.transform.position = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
                BigPlatelet.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    platelet.CustomStart(2, false, false);
                }
            }
        }
    }

    void SpawnSinglePlatelet(int size, int screen, bool CustomPos = false, Vector3 Position = default(Vector3))
    {
        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }

        Vector3 SpawnPos = plateletScreens[screenReference].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f)); ;
        if (CustomPos)
        {
            SpawnPos = Position;
        }

        switch (size)
        {
            case 0: // Small
                GameObject SmallPlatelet = SpawnFroomPool(platletTags[0] + screenReference);
                SmallPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].SmallPlatelets.Add(SmallPlatelet);
                SmallPlatelet.transform.position = SpawnPos;
                SmallPlatelet.TryGetComponent(out Platelet smallPlatelet);
                if (smallPlatelet != null)
                {
                    smallPlatelet.CustomStart(0, true, true);
                }
                break;
            case 1: // Medium
                GameObject MedPlatelet = SpawnFroomPool(platletTags[1] + screenReference);
                MedPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].MediumPlatelets.Add(MedPlatelet);
                MedPlatelet.transform.position = SpawnPos;
                MedPlatelet.TryGetComponent(out Platelet mediumPlatelet);
                if (mediumPlatelet != null)
                {
                    mediumPlatelet.CustomStart(1, true, true);
                }
                break;
            case 2: // Big
                GameObject BigPlatelet = SpawnFroomPool(platletTags[2] + screenReference);
                BigPlatelet.transform.SetParent(plateletScreens[screenReference].PlatletHolder.transform);
                plateletScreens[screenReference].BigPlatelets.Add(BigPlatelet);
                BigPlatelet.transform.position = SpawnPos;
                BigPlatelet.TryGetComponent(out Platelet bigPlatelet);
                if (bigPlatelet != null)
                {
                    bigPlatelet.CustomStart(2, true, true);
                }
                break;
        }
    }



    public void UpdatePlatelets(int screen, int organTypeID)
    {
        Debug.Log("Organ Type Id = " + organTypeID);
        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }
        ClearScreen(screen);
        InstantiatePlatalettes(screen, organTypeID);
    }

    public void ClearScreen(int screen)
    {
        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }
        for (int s = 0; s < plateletScreens[screenReference].SmallPlatelets.Count; s++)
        {
            plateletScreens[screenReference].SmallPlatelets[s].TryGetComponent(out Platelet platelet);
            if (platelet != null)
            {
                platelet.Despawn();
            }
        }
        plateletScreens[screenReference].SmallPlatelets.Clear();
        for (int m = 0; m < plateletScreens[screenReference].MediumPlatelets.Count; m++)
        {
            plateletScreens[screenReference].MediumPlatelets[m].TryGetComponent(out Platelet platelet);
            if (platelet != null)
            {
                platelet.Despawn();
            }
        }
        plateletScreens[screenReference].MediumPlatelets.Clear();
        for (int b = 0; b < plateletScreens[screenReference].BigPlatelets.Count; b++)
        {
            plateletScreens[screenReference].BigPlatelets[b].TryGetComponent(out Platelet platelet);
            if (platelet != null)
            {
                platelet.Despawn();
            }
        }
        plateletScreens[screenReference].BigPlatelets.Clear();

        Debug.Log("Cleared screen " + screenReference);
    }

    void ClearPlatetSize(int size, int screen)
    {
        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == screen)
            {
                screenReference = i;
            }
        }
        switch (size)
        {
            case 0: // small
                for (int s = 0; s < plateletScreens[screenReference].SmallPlatelets.Count; s++)
                {
                    plateletScreens[screenReference].SmallPlatelets[s].TryGetComponent(out Platelet platelet);
                    if (platelet != null)
                    {
                        platelet.Despawn();
                    }
                }
                plateletScreens[screenReference].SmallPlatelets.Clear();
                break;
            case 1: // Med
                for (int m = 0; m < plateletScreens[screenReference].MediumPlatelets.Count; m++)
                {
                    plateletScreens[screenReference].MediumPlatelets[m].TryGetComponent(out Platelet platelet);
                    if (platelet != null)
                    {
                        platelet.Despawn();
                    }
                }
                plateletScreens[screenReference].MediumPlatelets.Clear();
                break;
            case 2: // Big
                for (int b = 0; b < plateletScreens[screenReference].BigPlatelets.Count; b++)
                {
                    plateletScreens[screenReference].BigPlatelets[b].TryGetComponent(out Platelet platelet);
                    if (platelet != null)
                    {
                        platelet.Despawn();
                    }
                }
                plateletScreens[screenReference].BigPlatelets.Clear();
                break;
        }
    }

    GameObject SpawnMerger(int screen, Transform parent, Vector3 Pos = default(Vector3))
    {
        Vector3 spawnpoint = plateletScreens[screen].PlatletHolder.transform.position + new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-3f, 3f));
        if (Pos != default(Vector3))
        {
            spawnpoint = Pos;
        }

        GameObject merger = Instantiate(MergeObject, spawnpoint, Quaternion.identity);
        merger.transform.SetParent(parent);
        return merger;

    }

    public void CheckIfMergeisDone(bool small, GameObject Merger)
    {
        Debug.Log("Check");
        int screenReference = 1;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == 1)
            {
                screenReference = i;
            }
        }
        if (small)
        {
            for (int i = 0; i < plateletScreens[screenReference].SmallPlatelets.Count; i++)
            {
                GameObject plateletObject = plateletScreens[screenReference].SmallPlatelets[i];
                plateletObject.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    if (!platelet.MergeReady)
                    {
                        Merging = true;
                        return;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < plateletScreens[screenReference].MediumPlatelets.Count; i++)
            {
                GameObject plateletObject = plateletScreens[screenReference].MediumPlatelets[i];
                plateletObject.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    if (!platelet.MergeReady)
                    {
                        Merging = true;
                        return;
                    }
                }
            }
        }
        //Debug.Log("Destroy Merger");
        //Destroy(Merger);
        Merging = false;
    }



    IEnumerator CheckPlateletInstances()
    {
        canBuy = false;
        bool SmallCellMerge = false;
        bool SpawnMedPlatelet = false;
        int screenReference = 0;
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (plateletScreens[i].position == 1)
            {
                screenReference = i;
            }
        }

        GameObject MergerReference;
        if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity >= 5)
        {
            myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity = 0;
            myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity++;
            SmallCellMerge = true;
            GameObject merger = SpawnMerger(screenReference, plateletScreens[screenReference].PlatletHolder.transform);
            CurrentMerger = merger;
            for (int i = 0; i < plateletScreens[screenReference].SmallPlatelets.Count; i++)
            {
                GameObject plateletObject = plateletScreens[screenReference].SmallPlatelets[i];
                plateletObject.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    platelet.StartMerge(merger.transform, this, true);
                }
            }
            Merging = true;
            while (SmallCellMerge)
            {
                Debug.Log("Merging)");
                if (!Merging)
                {
                    SmallCellMerge = false;
                }

                yield return null;
            }

            ClearPlatetSize(0, 1);
            MergerReference = merger;
            DestroyMerger(merger);
            SpawnMedPlatelet = true;
        }
        else
        {
            //Debug.Log("Spawn Small Platelets");
            SpawnSinglePlatelet(0, 1);
            canBuy = true;
            yield break;
        }

        bool MediumCellMerge = false;
        if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity >= 5)
        {
            myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity = 0;
            myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[2].Quantity++;
            MediumCellMerge = true;
            GameObject merger = SpawnMerger(screenReference, plateletScreens[screenReference].PlatletHolder.transform, MergerReference.transform.position);
            CurrentMerger = merger;
            for (int i = 0; i < plateletScreens[screenReference].MediumPlatelets.Count; i++)
            {
                GameObject plateletObject = plateletScreens[screenReference].MediumPlatelets[i];
                plateletObject.TryGetComponent(out Platelet platelet);
                if (platelet != null)
                {
                    platelet.StartMerge(merger.transform, this, false);
                }
            }
            Merging = true;
            while (MediumCellMerge)
            {
                Debug.Log("Merging)");
                if (!Merging)
                {
                    MediumCellMerge = false;
                }
                yield return null;
            }

            ClearPlatetSize(1, 1);
            SpawnSinglePlatelet(2, 1, true, merger.transform.position);
            DestroyMerger(merger);
            //Debug.Log("Spawn Big Platelets");

        }
        else
        {
            if (SpawnMedPlatelet)
            {
                Debug.Log("Spawn Med Platelets");
                SpawnSinglePlatelet(1, 1, true, MergerReference.transform.position);
                DestroyMerger(MergerReference);
                canBuy = true;
                yield break;
            }
        }

        canBuy = true;
    }

    public void UpdatePlateletHolderPosition(bool Left)
    {
        for (int i = 0; i < plateletScreens.Length; i++)
        {
            if (!Left)
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

    void CancelMerge()
    {
        if (plataletInstancesRoutine != null)
        {
            StopCoroutine(plataletInstancesRoutine);
        }
        CheckMetaData();
        if (CurrentMerger != null)
        {
            Destroy(CurrentMerger);
        }
        canBuy = true;

    }

    void DestroyMerger(GameObject merger)
    {
        LTDescr l = LeanTween.alpha(merger, 0, 0.5f);
        LeanTween.scale(merger, Vector3.zero, 1f);
        l.setOnComplete(DestroyMergerObject);
        void DestroyMergerObject()
        {
            Destroy(merger);
        }
    }
    public void CheckMetaData()
    {
        if (myOrganManager.activeOrganType != 12)
        {
            if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity >= 5)
            {
                myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[0].Quantity = 0;
                myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity++;
            }
            if (myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity >= 5)
            {
                myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[1].Quantity = 0;
                myOrganManager.organTypes[myOrganManager.activeOrganType].platletSizes[2].Quantity++;
            }
        }
    }
}
