using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

namespace Idle
{
    public class CR_OrganView_PlatletsManager : MonoBehaviour
    {
        Vector2 MaxSpawnPos = new Vector2(4f, 4f);
        [SerializeField] int DebugPlatletNumber;

        bool canBuy = true;

        [System.Serializable]
        public class PlatletSizes
        {
            public string name;
            public List<CR_CellBase> ObjectsReferences = new List<CR_CellBase>();
        }
        public PlatletSizes[] platletsLists = new PlatletSizes[] {
            new PlatletSizes{
                name = "Small",
            },
              new PlatletSizes{
                name = "Medium",
            },
                new PlatletSizes{
                name = "Big",
            }
        };

        public enum Sizes
        {
            Small,
            Medium,
            Big
        }

        [SerializeField] Transform _platletMergePF;
        [SerializeField] float _platletCostMultiplier;

        // ======= Pooling variables ======//

        [SerializeField] CR_CellBase platletPF;
        [SerializeField] Transform platletHolder;
        [SerializeField] int platletSizeCount;
        public Dictionary<string, Queue<CR_CellBase>> PoolDictionary;
        string[] platletTag = new string[] { "PS", "PM", "PB" };
        // ===== end Pooling variables ====//


        private void Awake()
        {
            PoolDictionary = new Dictionary<string, Queue<CR_CellBase>>();
            InsantiatePools();
        }

        void InsantiatePools()
        {
            var data = CR_Data.data;
            for (int p = 0; p < platletTag.Length; p++)
            {
                Queue<CR_CellBase> ObjectPool = new Queue<CR_CellBase>();
                for (int i = 0; i < platletSizeCount; i++)
                {
                    CR_CellBase obj = Instantiate(platletPF);
                    obj.InitializeCell((CR_CellBase.CellSize)p);
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(platletHolder.transform);
                    obj.transform.localPosition = Vector3.zero;
                    ObjectPool.Enqueue(obj);
                }
                PoolDictionary.Add(platletTag[p], ObjectPool);
            }


            //Debug.Log("Added pool with key " + pool.tag);

        }

        CR_CellBase SpawnFroomPool(string tag)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("pool With tag" + tag + " doesn't exist");
                return null;
            }
            CR_CellBase ObjectToSpawn = PoolDictionary[tag].Dequeue();
            ObjectToSpawn.gameObject.SetActive(true);
            PoolDictionary[tag].Enqueue(ObjectToSpawn);
            return ObjectToSpawn;
        }

        private void Start()
        {
            // SpawnPlatlets();
        }


        public void InitializeOrgan()
        {
            ClearPlatlets();
            SpawnPlatlets();
        }

        void ClearPlatlets(bool All = false, Sizes Size = Sizes.Small)
        {
            if (All)
            {
                for (int i = 0; i < platletsLists.Length; i++)
                {
                    for (int o = 0; o < platletsLists[i].ObjectsReferences.Count; o++)
                    {
                        platletsLists[i].ObjectsReferences[o].gameObject.SetActive(false);

                    }
                    platletsLists[i].ObjectsReferences.Clear();
                }
            }
            else
            {
                for (int o = 0; o < platletsLists[(int)Size].ObjectsReferences.Count; o++)
                {
                    platletsLists[(int)Size].ObjectsReferences[o].gameObject.SetActive(false);
                }
                platletsLists[(int)Size].ObjectsReferences.Clear();
            }

        }

        void SpawnPlatlets()
        {
            var data = CR_Data.data;
            int CurrentOrgan = CR_Idle_Manager.instance.CurrentOrganType;
            int platletNumber = data.organTypes[CurrentOrgan].plateletInfo.platletNumber;
            //int platletNumber = DebugPlatletNumber;

            int BigPlatlets = 0;
            int MediumPlatlets = 0;
            int SmallPlatlets = 0;

            if (platletNumber >= 25)
            {
                BigPlatlets = platletNumber / 25;
                platletNumber = platletNumber - (BigPlatlets * 25);
            }
            if (platletNumber >= 5)
            {
                MediumPlatlets = platletNumber / 5;
                platletNumber = platletNumber - (MediumPlatlets * 5);
            }
            if (platletNumber < 5)
            {
                SmallPlatlets = platletNumber;
            }

           // Debug.Log("BP: " + BigPlatlets + " MP: " + MediumPlatlets + " SP: " + SmallPlatlets);

            for (int BP = 0; BP < BigPlatlets; BP++)
            {
                SpawnPlatlet(Sizes.Big);
            }
            for (int MP = 0; MP < MediumPlatlets; MP++)
            {
                SpawnPlatlet(Sizes.Medium);
            }
            for (int SP = 0; SP < SmallPlatlets; SP++)
            {
                SpawnPlatlet(Sizes.Small);
            }


        }

        public void OnClickBuyPlatlet()
        {
            if (!canBuy) return;
            var data = CR_Data.data;
            int CurrentOrgan = CR_Idle_Manager.instance.CurrentOrganType;
            if (data.organTypes[CurrentOrgan].plateletInfo.platletNumber < 125)
            {
                if (true) //data._energy >= data.organTypes[CurrentOrgan].plateletCost todo
                {
                    data.organTypes[CurrentOrgan].plateletInfo.platletNumber++;
                    //SpawnPlatlet(0);
                    Debug.Log("buy");
                    StartCoroutine(CheckPlatlets());
                    data.organTypes[CurrentOrgan].plateletInfo.plateletCost = data.organTypes[CurrentOrgan].plateletInfo.plateletInitialCost * BigDouble.Pow(_platletCostMultiplier, data.organTypes[CurrentOrgan].plateletInfo.platletNumber);
                    CR_OrganView_Manager.instance.UpdateUI();
                   

                }
                else
                {
                    Debug.Log("Not enough energy: " + data._energy + "/" + data.organTypes[CurrentOrgan].plateletInfo.plateletCost);
                }
            }
            else
            {
                Debug.Log("Max platlets reached");
            }
        }

        IEnumerator CheckPlatlets()
        {
            canBuy = false;
            Debug.Log("check");
            if (platletsLists[0].ObjectsReferences.Count < 4)
            {
                Debug.Log("Spawn S platlet");
                SpawnPlatlet(0);
                yield return null;

            }
            else if (platletsLists[1].ObjectsReferences.Count < 4)
            {
                yield return StartMerge(Sizes.Small); // Merge small 
            }
            else if (platletsLists[2].ObjectsReferences.Count < 5)
            {
                yield return StartMerge(Sizes.Small); // Merge small
                yield return StartMerge(Sizes.Medium); // Merge small 
            }
            canBuy = true;
        }

        IEnumerator StartMerge(Sizes size)
        {   
            Transform mergeTransform = Instantiate(_platletMergePF, platletHolder);
            float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
            float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
            bool animationDone = false;

            mergeTransform.localPosition = new Vector3(randomPosX, randomPosY, 0);

            for (int i = 0; i < platletsLists[(int)size].ObjectsReferences.Count; i++)
            {
                yield return platletsLists[(int)size].ObjectsReferences[i].Merge(mergeTransform);
                platletsLists[(int)size].ObjectsReferences[i].gameObject.SetActive(false);
                //mergeTransform.localScale = mergeTransform.localScale * 1.2f;
                LeanTween.scale(mergeTransform.gameObject, mergeTransform.localScale * 1.1f, 0.4f).setEase(LeanTweenType.easeOutBounce);
            }
            ClearPlatlets(false, size);
            Sizes NewPlatletSize = (Sizes)((int)size + 1);
            LeanTween.scale(mergeTransform.gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeInElastic).setOnComplete(done =>
            {
                Destroy(mergeTransform.gameObject);
                SpawnPlatlet(NewPlatletSize, mergeTransform);
                animationDone = true;
            });
            while (!animationDone)
            {
                yield return null;
            }
            
        }

        void SpawnPlatlet(Sizes size, Transform transform = null)
        {
            if ((int)size > platletTag.Length - 1) return;
            CR_CellBase platlet = SpawnFroomPool(platletTag[(int)size]);
            if(transform != null)
            {
                platlet.transform.position = transform.position;
            }
            else
            {
                float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
                float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
                platlet.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);
            }
            platlet.StartCell();
            platletsLists[(int)size].ObjectsReferences.Add(platlet);

        }



    }
}

