using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_OrganView_PlatletsManager : MonoBehaviour
    {
        Vector2 MaxSpawnPos = new Vector2(4f, 4f);
        [SerializeField] int DebugPlatletNumber;

        [System.Serializable]
        public class PlatletSizes
        {
            public string name;
            public List<CR_Platlet> ObjectsReferences = new List<CR_Platlet>();
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



        // ======= Pooling variables ======//

        [SerializeField] CR_Platlet platletPF;
        [SerializeField] Transform platletHolder;
        [SerializeField] int platletSizeCount;
        public Dictionary<string, Queue<CR_Platlet>> PoolDictionary;
        string[] platletTag = new string[] { "PS", "PM", "PB" };
        // ===== end Pooling variables ====//


        private void Awake()
        {
            PoolDictionary = new Dictionary<string, Queue<CR_Platlet>>();
            InsantiatePools();
        }
        void InsantiatePools()
        {
            var data = CR_Data.data;
            for (int p = 0; p < platletTag.Length; p++)
            {
                Queue<CR_Platlet> ObjectPool = new Queue<CR_Platlet>();
                for (int i = 0; i < platletSizeCount; i++)
                {
                    CR_Platlet obj = Instantiate(platletPF);
                    obj.SetPlatlet(p);
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(platletHolder.transform);
                    obj.transform.localPosition = Vector3.zero;
                    ObjectPool.Enqueue(obj);
                }
                PoolDictionary.Add(platletTag[p], ObjectPool);
            }


            //Debug.Log("Added pool with key " + pool.tag);

        }

        CR_Platlet SpawnFroomPool(string tag)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("pool With tag" + tag + " doesn't exist");
                return null;
            }
            CR_Platlet ObjectToSpawn = PoolDictionary[tag].Dequeue();
            ObjectToSpawn.gameObject.SetActive(true);
            PoolDictionary[tag].Enqueue(ObjectToSpawn);
            return ObjectToSpawn;
        }

        private void Start()
        {
            SpawnPlatlets();
        }

        void ClearPlatlets(bool All = false, Sizes Size = Sizes.Small)
        {
            if (All)
            {
                for (int i = 0; i < platletsLists.Length; i++)
                {
                    platletsLists[i].ObjectsReferences.Clear();
                }
            }
            else
            {
                platletsLists[(int)Size].ObjectsReferences.Clear();
            }
            
        }

        void SpawnPlatlets()
        {
            var data = CR_Data.data;
            int CurrentOrgan = CR_Idle_Manager.instance.currentOrganLoaded;
            //int platletNumber = data.organTypes[CurrentOrgan].platletNumber;
            int platletNumber = DebugPlatletNumber;

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

            Debug.Log("BP: " + BigPlatlets + " MP: " + MediumPlatlets + " SP: " + SmallPlatlets);

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
            var data = CR_Data.data;
            int CurrentOrgan = CR_Idle_Manager.instance.currentOrganLoaded;
            if (data.organTypes[CurrentOrgan].platletNumber < 125)
            {
                if (data._energy > data.organTypes[CurrentOrgan].plateletCost)
                {
                    data.organTypes[CurrentOrgan].platletNumber++;
                    SpawnPlatlet(0);
                }
                else
                {
                    Debug.Log("Not enough energy");
                }
            }
            else
            {
                Debug.Log("Max platlets reached");
            }
        }

        void SpawnPlatlet(Sizes size)
        {
            if ((int)size > platletTag.Length - 1) return;
            CR_Platlet platlet = SpawnFroomPool(platletTag[(int)size]);
            float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
            float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
            platlet.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);
            Debug.Log("Size: " + (int)size);
            platletsLists[(int)size].ObjectsReferences.Add(platlet);
        }



    }
}

