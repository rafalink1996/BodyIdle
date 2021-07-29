using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUiManager : MonoBehaviour
{
    public GameObject UiCellCountcontainer;
    [SerializeField] GameObject SlotPrefab;

    [SerializeField] Image BuyButton, BuyButtonCost, BuyButtonCellImage;

    [SerializeField] CellsSO RedBloodCell, WhiteBloodCell, HelperCell;
    UIBotLeanTween MyUILeanTween;


    // Pooling
    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    string WhiteBloodCellTag = "CellSlotWhiteCell";
    string RedBloodCellTag = "CellSlotRedCell";
    string HelperTCellTag = "CellSlotHelperCell";

    [SerializeField] int RedBloodCell1;
    [SerializeField] int RedBloodCell10;
    [SerializeField] int RedBloodCell100;
    public float RedBloodCellTotal;

    [SerializeField] int whiteBloodCell1;
    [SerializeField] int whiteBloodCell10;
    [SerializeField] int whiteBloodCell100;
    public float WhiteBloodCellTotal;

    [SerializeField] int HelperCell1;
    [SerializeField] int HelperCell10;
    [SerializeField] int HelperCell100;
    public float HelperTCellTotal;

    private void Start()
    {
        MyUILeanTween = GetComponent<UIBotLeanTween>();
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (pool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(UiCellCountcontainer.transform);
                ObjectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, ObjectPool);
        }
    }
    public GameObject SpawnFroomPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool With tag" + tag + "deosn't exist");
            return null;
        }
        GameObject ObjectToSpawn = PoolDictionary[tag].Dequeue();
        ObjectToSpawn.SetActive(true);
        PoolDictionary[tag].Enqueue(ObjectToSpawn);
        return ObjectToSpawn;

    }
    public void ChangeCellType(int CellType)
    {
        for (int i = 0; i < UiCellCountcontainer.transform.childCount; i++)
        {
           GameObject CellSlot = UiCellCountcontainer.transform.GetChild(i).gameObject;
            CellSlot.SetActive(false);
        }

       if (CellType == 1)
        {
            for (int i = 0; i < RedBloodCell1; i++)
            {
                GameObject RedCell = SpawnFroomPool(RedBloodCellTag);
                //RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                RedCell.transform.localScale = new Vector3(1, 1, 1);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx1;
            }

            for (int i = 0; i < RedBloodCell10; i++)
            {
                GameObject RedCell = SpawnFroomPool(RedBloodCellTag);
                // RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                RedCell.transform.localScale = new Vector3(1, 1, 1);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx10;
            }

            for (int i = 0; i < RedBloodCell100; i++)
            {
                GameObject RedCell = SpawnFroomPool(RedBloodCellTag);
                //RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                RedCell.transform.localScale = new Vector3(1, 1, 1);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx100;
            }

            BuyButton.color = RedBloodCell.MyColor;
            BuyButtonCost.color = RedBloodCell.MyColor;
            BuyButtonCellImage.sprite = RedBloodCell.Cellx1;

            

        }

        if (CellType == 2)
        {
            for (int i = 0; i < whiteBloodCell1; i++)
            {
                GameObject WhiteCell = SpawnFroomPool(WhiteBloodCellTag);
                // WhiteCell.transform.parent = UiCellCountcontainer.transform;
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                WhiteCell.transform.localScale = new Vector3(1, 1, 1);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx1;
            }

            for (int i = 0; i < whiteBloodCell10; i++)
            {
                GameObject WhiteCell = SpawnFroomPool(WhiteBloodCellTag);
                //WhiteCell.transform.parent = UiCellCountcontainer.transform;
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                WhiteCell.transform.localScale = new Vector3(1, 1, 1);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx10;

            }

            for (int i = 0; i < whiteBloodCell100; i++)
            {
                GameObject WhiteCell = SpawnFroomPool(WhiteBloodCellTag); 
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                WhiteCell.transform.localScale = new Vector3(1, 1, 1);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx100;


            }

            BuyButton.color = WhiteBloodCell.MyColor;
            BuyButtonCost.color = WhiteBloodCell.MyColor;
            BuyButtonCellImage.sprite = WhiteBloodCell.Cellx1;
        }

        if (CellType == 3)
        {
            for (int i = 0; i < HelperCell1; i++)
            {
                GameObject HelperCellO = SpawnFroomPool(HelperTCellTag);
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                HelperCellO.transform.localScale = new Vector3(1, 1, 1);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;
            }

            for (int i = 0; i < HelperCell10; i++)
            {
                GameObject HelperCellO = SpawnFroomPool(HelperTCellTag);
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                HelperCellO.transform.localScale = new Vector3(1, 1, 1);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;

            }

            for (int i = 0; i < HelperCell100; i++)
            {
                GameObject HelperCellO = SpawnFroomPool(HelperTCellTag);
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                HelperCellO.transform.localScale = new Vector3(1, 1, 1);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;
            }

            BuyButton.color = HelperCell.MyColor;
            BuyButtonCost.color = HelperCell.MyColor;
            BuyButtonCellImage.sprite = HelperCell.Cellx1;
        }

        
        MyUILeanTween.FinishChangeCellType();

    }


    public float CheckCellTotal(int celltype)
    {
        float CellTotal = 0;
        if (celltype == 1)
        {
            CellTotal = RedBloodCell1 + RedBloodCell10 + RedBloodCell100;
        }
        else if (celltype == 2)
        {
            CellTotal = whiteBloodCell1 + whiteBloodCell10 + whiteBloodCell100;
        }
        else if (celltype == 3)
        {
            CellTotal = HelperCell1 + HelperCell10 + HelperCell100;
        }

        return CellTotal;

    }
}
