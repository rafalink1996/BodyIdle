using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

namespace Idle
{
    public class CR_CellView_CellManager : MonoBehaviour
    {
        // ---POOLING VARIABLES--- //
        [System.Serializable]
        public class cellTypePool
        {
            [System.Serializable]
            public class cellPool
            {
                public string tag;
                public CR_CellviewCell prefab;
                public int size;
            }

            public string name;
            public List<cellPool> _pools;
        }
        public cellTypePool[] _typePools;

        [SerializeField] Transform _cellHolder;
        public Dictionary<string, Queue<CR_CellviewCell>> PoolDictionary;
        //--- POOLING VARIABLES--- //

        [System.Serializable]
        public class Lists_CellTypes
        {
            public string name;
            [System.Serializable]
            public class Lists_CellSizes
            {
                public List<CR_CellviewCell> cells = new List<CR_CellviewCell>();
            }
            public Lists_CellSizes[] lists_CellSizes;

        }
        public Lists_CellTypes[] lists_CellTypes;


        [System.Serializable]
        public class MergeObject
        {
            public Transform ObjectPF;
            public Sprite[] typeSprites;
        }
        [SerializeField] MergeObject mergeObject;

        Vector2 MaxSpawnPos = new Vector2(4f, 4f);
        Transform mergeTransform;
        public bool _merging;


        private void Awake()
        {
            SpawnPools();
        }



        void SpawnPools()
        {
            PoolDictionary = new Dictionary<string, Queue<CR_CellviewCell>>();
            for (int i = 0; i < _typePools.Length; i++)
            {
                for (int o = 0; o < _typePools[i]._pools.Count; o++)
                {
                    Queue<CR_CellviewCell> ObjectPool = new Queue<CR_CellviewCell>();
                    for (int p = 0; p < _typePools[i]._pools[o].size; p++)
                    {
                        CR_CellviewCell obj = Instantiate(_typePools[i]._pools[o].prefab);
                        obj.gameObject.SetActive(false);
                        obj.transform.SetParent(_cellHolder.transform);
                        obj.InitializeCell((CR_CellBase.CellSize)o, (CR_CellBase.CellType)(i + 1));
                        ObjectPool.Enqueue(obj);
                    }
                    PoolDictionary.Add(_typePools[i]._pools[o].tag, ObjectPool);
                }
            }
        }

        CR_CellviewCell SpawnFroomPool(string tag)
        {
            if (!PoolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("pool With tag" + tag + " doesn't exist");
                return null;
            }
            CR_CellviewCell ObjectToSpawn = PoolDictionary[tag].Dequeue();
            ObjectToSpawn.gameObject.SetActive(true);
            PoolDictionary[tag].Enqueue(ObjectToSpawn);
            return ObjectToSpawn;
        }

        public void SpawnCells(int organType, int organNumber)
        {
            CR_Data data = CR_Data.data;
            CR_Data.OrganType.OrganInfo organ = data.organTypes[organType].organs[organNumber];
            for (int i = 0; i < organ.CellTypes.Length; i++)
            {
                for (int o = 0; o < organ.CellTypes[i].cellSizes.Count; o++)
                {
                    for (int p = 0; p < organ.CellTypes[i].cellSizes[o].CellsInfos.Count; p++)
                    {
                        CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info = organ.CellTypes[i].cellSizes[o].CellsInfos[p];
                        CR_CellviewCell cell = SpawnCell((CR_CellBase.CellType)(i + 1), (CR_CellBase.CellSize)(o), info);
                        float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
                        float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
                        cell.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);
                    }
                }
            }
        }

        CR_CellviewCell SpawnCell(CR_CellBase.CellType type, CR_CellBase.CellSize size, CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info)
        {
            CR_CellviewCell cell = SpawnFroomPool(_typePools[((int)type) - 1]._pools[(int)size].tag);
            cell.SetCellInfo(info);
            cell.StartCell();
            cell.setCellSize(size);
            lists_CellTypes[((int)type) - 1].lists_CellSizes[(int)size].cells.Add(cell);
            return cell;
        }

        public void CreateNewCell(CR_CellBase.CellType type, CR_CellBase.CellSize size, CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo newInfo)
        {
            var cell = SpawnCell(type, size, newInfo);
            float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
            float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
            cell.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);

        }
        public void CreateNewCell(CR_CellBase.CellType type, CR_CellBase.CellSize size, CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo newInfo, Vector3 pos)
        {
            var cell = SpawnCell(type, size, newInfo);
            cell.transform.localPosition = pos;
        }



        CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo CreateNewCellInfo(CR_CellBase.CellType type, CR_CellBase.CellSize size)
        {
            int maxLife = 1;
            switch (type)
            {
                case CR_CellBase.CellType.RedBlood:
                case CR_CellBase.CellType.White:
                    switch (size)
                    {
                        case CR_CellBase.CellSize.Small:
                            maxLife = 1;
                            break;
                        case CR_CellBase.CellSize.Medium:
                            maxLife = 10;
                            break;
                        case CR_CellBase.CellSize.Big:
                            maxLife = 100;
                            break;
                        default:
                            break;
                    }
                    break;
                case CR_CellBase.CellType.Helper:
                    switch (size)
                    {
                        case CR_CellBase.CellSize.Small:
                            maxLife = 1;
                            break;
                        case CR_CellBase.CellSize.Medium:
                            maxLife = 3;
                            break;
                        case CR_CellBase.CellSize.Big:
                            maxLife = 6;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info = new CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo
            {
                alive = true,
                maxHealth = maxLife,
                health = maxLife,
                timer = 0
            };
            CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[((int)type) - 1].cellSizes[(int)size].CellsInfos.Add(info);
            return info;

        }

        void ClearCellInfos(CR_CellBase.CellType type, CR_CellBase.CellSize size)
        {
            CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[((int)type) - 1].cellSizes[(int)size].CellsInfos.Clear();
        }

        public IEnumerator CheckCells()
        {
            int organType = CR_Idle_Manager.instance.CurrentOrganType;
            int organNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            CR_Data.OrganType.OrganInfo currentorgan = CR_Data.data.organTypes[organType].organs[organNumber];

            switch (CR_CellViewManager.instance._cellSelected)
            {
                case CR_CellViewManager.cellType.RedBloodCell:
                    if (currentorgan.CellTypes[0].cellSizes[0].CellsInfos.Count < 9)
                    {
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small);
                        CreateNewCell(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[0].cellSizes[1].CellsInfos.Count < 9)
                    {
                        ClearCellInfos(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Medium);
                        yield return StartMerge(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[0].cellSizes[2].CellsInfos.Count < 10)
                    {
                        ClearCellInfos(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small);
                        ClearCellInfos(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Medium);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Big);
                        yield return StartMerge(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small, newInfo);
                        yield return StartMerge(CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Medium, newInfo);
                    }
                    else
                    {
                        Debug.Log("Max Blood Cells Reached");
                    }
                    break;
                case CR_CellViewManager.cellType.WhiteBloodCell:
                    if (currentorgan.CellTypes[1].cellSizes[0].CellsInfos.Count < 9)
                    {
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small);
                        CreateNewCell(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[1].cellSizes[1].CellsInfos.Count < 9)
                    {
                        ClearCellInfos(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.White, CR_CellBase.CellSize.Medium);
                        yield return StartMerge(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[1].cellSizes[2].CellsInfos.Count < 10)
                    {
                        ClearCellInfos(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small);
                        ClearCellInfos(CR_CellBase.CellType.White, CR_CellBase.CellSize.Medium);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.White, CR_CellBase.CellSize.Big);
                        yield return StartMerge(CR_CellBase.CellType.White, CR_CellBase.CellSize.Small, newInfo);
                        yield return StartMerge(CR_CellBase.CellType.White, CR_CellBase.CellSize.Medium, newInfo);
                    }
                    else
                    {
                        Debug.Log("Max White Cells Reached");
                    }
                    break;
                case CR_CellViewManager.cellType.HelperTCell:
                    if (currentorgan.CellTypes[2].cellSizes[0].CellsInfos.Count < 2)
                    {
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small);
                        CreateNewCell(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[2].cellSizes[1].CellsInfos.Count < 2)
                    {
                        ClearCellInfos(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Medium);
                        yield return StartMerge(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small, newInfo);
                    }
                    else if (currentorgan.CellTypes[2].cellSizes[2].CellsInfos.Count < 3)
                    {
                        ClearCellInfos(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small);
                        ClearCellInfos(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Medium);
                        var newInfo = CreateNewCellInfo(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Big);
                        yield return StartMerge(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small, newInfo);
                        yield return StartMerge(CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Medium, newInfo);
                    }
                    else
                    {
                        Debug.Log("Max Helper Cells Reached");
                    }
                    break;
            }
        }

        IEnumerator StartMerge(CR_CellBase.CellType type, CR_CellBase.CellSize size, CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo newInfo)
        {
            _merging = true;
            CR_CellViewManager.instance.canBuy = false;
            Transform mergeTransform = Instantiate(mergeObject.ObjectPF, _cellHolder);
            this.mergeTransform = mergeTransform;
            if (mergeTransform.TryGetComponent(out SpriteRenderer renderer))
            {
                Sprite sprite = null;
                switch (type)
                {
                    case CR_CellBase.CellType.RedBlood:
                        sprite = mergeObject.typeSprites[0];
                        break;
                    case CR_CellBase.CellType.White:
                        sprite = mergeObject.typeSprites[1];
                        break;
                    case CR_CellBase.CellType.Helper:
                        sprite = mergeObject.typeSprites[2];
                        break;

                }
                if (sprite != null) renderer.sprite = sprite;
            }
            float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
            float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
            mergeTransform.localPosition = new Vector3(randomPosX, randomPosY, 0);
            bool animationDone = false;

            var cells = lists_CellTypes[(int)(type - 1)].lists_CellSizes[(int)size].cells;
            for (int i = 0; i < cells.Count; i++)
            {
                yield return cells[i].Merge(mergeTransform, .8f);
                bool MergeScaleCompleted = false;
                LeanTween.scale(mergeTransform.gameObject, mergeTransform.localScale * 1.1f, 0.2f).setEase(LeanTweenType.easeOutBounce).setOnComplete(done => { MergeScaleCompleted = true; });
                while (!MergeScaleCompleted)
                {
                    yield return null;
                }
            }

            LeanTween.scale(mergeTransform.gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeInElastic).setOnComplete(done =>
            {
                CreateNewCell(type, (CR_CellBase.CellSize)((int)(size + 1)), newInfo, mergeTransform.localPosition);
                Destroy(mergeTransform.gameObject);
                animationDone = true;
                ClearCells(type, size);
            });

            while (!animationDone)
            {
                yield return null;
            }

            yield return null;
            CR_CellViewManager.instance.canBuy = true;
            _merging = false;

        }

        public IEnumerator CheckBuyMaxCells(int amount)
        {

            var manager = CR_Idle_Manager.instance;
            var cellViewManager = CR_CellViewManager.instance;
            var currentCells = CR_Data.data.GetTotalCells(manager.CurrentOrganType, manager.CurrentOrganNumber, (int)cellViewManager._cellSelected);
            var totalCellAmount = currentCells + amount;
            var currentCellSelected = (int)CR_CellViewManager.instance._cellSelected;
            Debug.Log("Total cells: " + totalCellAmount);

            cellViewManager.canBuy = false;
            int bigCells = 0;
            int medCells = 0;
            int smallCells = 0;

            int bigCellDivider = 100;
            int mediumCellDivider = 10;

            switch (currentCellSelected)
            {
                case 0:
                case 1:
                default:
                    bigCellDivider = 100;
                    mediumCellDivider = 10;
                    break;
                case 2:
                    bigCellDivider = 9;
                    mediumCellDivider = 3;
                    break;

            }
                
            while (totalCellAmount >= bigCellDivider)
            {
                totalCellAmount -= bigCellDivider;
                bigCells++;
            }
            while (totalCellAmount >= mediumCellDivider)
            {
                totalCellAmount -= mediumCellDivider;
                medCells++;
            }
            smallCells = totalCellAmount;

            BuymaxSetCellInfos(0, smallCells);
            BuymaxSetCellInfos(1, medCells);
            BuymaxSetCellInfos(2, bigCells);

            
            yield return ResetCells(true, currentCellSelected);
            SpawnCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, currentCellSelected);
            cellViewManager.canBuy = true;


        }
        void BuymaxSetCellInfos(int sizeIndex, int cellAmount)
        {
            var data = CR_Data.data;


            var currentOrganType = CR_Idle_Manager.instance.CurrentOrganType; ;
            var currentOrganNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            var currentCellSelected = (int)CR_CellViewManager.instance._cellSelected;


            var cellSizes = data.organTypes[currentOrganType].organs[currentOrganNumber].CellTypes[currentCellSelected].cellSizes;
            var cellList = lists_CellTypes[currentCellSelected].lists_CellSizes[sizeIndex].cells;

            var size = CR_CellBase.CellSize.Small;
            switch (sizeIndex)
            {
                case 0:
                    size = CR_CellBase.CellSize.Small;
                    break;
                case 1:
                    size = CR_CellBase.CellSize.Medium;
                    break;
                case 2:
                    size = CR_CellBase.CellSize.Big;
                    break;
            }
            CR_CellBase.CellType type = CR_CellBase.CellType.RedBlood;
            switch (currentCellSelected)
            {
                case 0:
                    type = CR_CellBase.CellType.RedBlood;
                    break;
                case 1:
                    type = CR_CellBase.CellType.White;
                    break;
                case 2:
                    type = CR_CellBase.CellType.Helper;
                    break;
            }

            while (cellSizes[sizeIndex].CellsInfos.Count > cellAmount)
            {
                if (cellSizes[sizeIndex].CellsInfos.Count > 0)
                {
                    cellSizes[sizeIndex].CellsInfos.RemoveAt(cellSizes[sizeIndex].CellsInfos.Count - 1);
                }
            }
            while (cellSizes[sizeIndex].CellsInfos.Count < cellAmount)
            {
                CreateNewCellInfo(type, size);
            }

        }

        void ClearCells(CR_CellBase.CellType type, CR_CellBase.CellSize size)
        {
            var infos = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[(int)(type - 1)].cellSizes[(int)size].CellsInfos;
            infos.Clear();
            var cellObjects = lists_CellTypes[(int)(type - 1)].lists_CellSizes[(int)size].cells;
            for (int i = 0; i < cellObjects.Count; i++)
            {
                cellObjects[i].gameObject.SetActive(false);
            }
            cellObjects.Clear();
        }
        public void ResetCells()
        {
            if (mergeTransform != null) Destroy(mergeTransform.gameObject);
            for (int i = 0; i < lists_CellTypes.Length; i++)
            {
                for (int o = 0; o < lists_CellTypes[i].lists_CellSizes.Length; o++)
                {
                    for (int p = 0; p < lists_CellTypes[i].lists_CellSizes[o].cells.Count; p++)
                    {
                        lists_CellTypes[i].lists_CellSizes[o].cells[p].gameObject.SetActive(false);
                    }
                    lists_CellTypes[i].lists_CellSizes[o].cells.Clear();
                }
            }
        }

        public IEnumerator ResetCells(bool animation, int celltype)
        {
            if (mergeTransform != null) Destroy(mergeTransform.gameObject);

            for (int o = 0; o < lists_CellTypes[celltype].lists_CellSizes.Length; o++)
            {
                int cellNumber = lists_CellTypes[celltype].lists_CellSizes[o].cells.Count;
                for (int p = 0; p < lists_CellTypes[celltype].lists_CellSizes[o].cells.Count; p++)
                {
                    lists_CellTypes[celltype].lists_CellSizes[o].cells[p].DespawnCell(() => { cellNumber--; });
                }
                while (cellNumber > 0)
                {
                    yield return null;
                }
                lists_CellTypes[celltype].lists_CellSizes[o].cells.Clear();
            }
        }


        public void SpawnCells(int organType, int organNumber, int celltype)
        {
            CR_Data data = CR_Data.data;
            CR_Data.OrganType.OrganInfo organ = data.organTypes[organType].organs[organNumber];

            for (int o = 0; o < organ.CellTypes[celltype].cellSizes.Count; o++)
            {
                for (int p = 0; p < organ.CellTypes[celltype].cellSizes[o].CellsInfos.Count; p++)
                {
                    CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info = organ.CellTypes[celltype].cellSizes[o].CellsInfos[p];
                    CR_CellviewCell cell = SpawnCell((CR_CellBase.CellType)(celltype + 1), (CR_CellBase.CellSize)(o), info);
                    float randomPosX = Random.Range(-MaxSpawnPos.x, MaxSpawnPos.x);
                    float randomPosY = Random.Range(-MaxSpawnPos.y, MaxSpawnPos.y);
                    cell.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);
                }
            }

        }
    }


}
