using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomUiManager : MonoBehaviour
{
    public GameObject UiCellCountcontainer;
    [SerializeField] GameObject SlotPrefab;

    [SerializeField] Image BuyButton, BuyButtonCost, BuyButtonCellImage;

    [SerializeField] CellsSO RedBloodCell, WhiteBloodCell, HelperCell;
    [SerializeField] UIBotLeanTween MyUILeanTween;
    OrganManager myOrganManager;
    

    // References

    private void Start()
    {
        myOrganManager = GameManager.gameManager.organManager;
        MyUILeanTween = GetComponent<UIBotLeanTween>();
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (pool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(UiCellCountcontainer.transform);
                ObjectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, ObjectPool);
        }
    }
    #region Pooling

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

    #endregion Pooling
    public void ChangeCellType(int CellType)
    {
        for (int i = 0; i < UiCellCountcontainer.transform.childCount; i++)
        {
            GameObject CellSlot = UiCellCountcontainer.transform.GetChild(i).gameObject;
            CellSlot.SetActive(false);
        }

        if (CellType == 1)
        {
            if (myOrganManager.organs.Count != 0)
            {
                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[0].Cells.Count; i++)
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
                    RedCellImageObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[1].Cells.Count; i++)
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
                    RedCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[2].Cells.Count; i++)
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
                    RedCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

                    BuyButton.color = RedBloodCell.MyColor;
            BuyButtonCost.color = RedBloodCell.MyColor;
            BuyButtonCellImage.sprite = RedBloodCell.Cellx1;



        }

        if (CellType == 2)
        {
            if (myOrganManager.organs.Count != 0)
            {
                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[3].Cells.Count; i++)
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
                    WhiteCellImageObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[4].Cells.Count; i++)
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
                    WhiteCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);

                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[5].Cells.Count; i++)
                {
                    GameObject WhiteCell = SpawnFroomPool(WhiteBloodCellTag);
                    WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                    WhiteCell.transform.localScale = new Vector3(1, 1, 1);
                    Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                    WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                    GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                    Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                    WhiteCellImage.sprite = WhiteBloodCell.Cellx100;
                    WhiteCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);


                }
            }

            BuyButton.color = WhiteBloodCell.MyColor;
            BuyButtonCost.color = WhiteBloodCell.MyColor;
            BuyButtonCellImage.sprite = WhiteBloodCell.Cellx1;
        }

        if (CellType == 3)
        {
            if (myOrganManager.organs.Count != 0)
            {
                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[6].Cells.Count; i++)
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
                    HelperCellImageObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[7].Cells.Count; i++)
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
                    HelperCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);

                }

                for (int i = 0; i < myOrganManager.organs[myOrganManager.activeOrganID].lists[8].Cells.Count; i++)
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
                    HelperCellImageObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }

                BuyButton.color = HelperCell.MyColor;
                BuyButtonCost.color = HelperCell.MyColor;
                BuyButtonCellImage.sprite = HelperCell.Cellx1;
            }
        }


        MyUILeanTween.FinishChangeCellType();

    }


    public float CheckCellTotal(int celltype)
    {
        float CellTotal = 0;
        int MinRange;
        int maxRange;
        
        switch (celltype)
        {
            case 1:
                MinRange = 0;
                maxRange = 3;
                break;
            case 2:
                MinRange = 3;
                maxRange = 6;
                break;
            case 3:
                MinRange = 6;
                maxRange = 9;
                break;
            default:
                MinRange = 0;
                maxRange = 3;
                break;
        }
        
        if (myOrganManager.organs.Count != 0)
        {
            for(int i = MinRange; i < maxRange; i++)
            {
                CellTotal += myOrganManager.organs[myOrganManager.activeOrganID].lists[i].Cells.Count;
            }
        }

            return CellTotal;

    }
}





//    private void Start()
//    {




//    MyUILeanTween = GetComponent<UIBotLeanTween>();
//        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

//        foreach (pool pool in pools)
//        {
//            Queue<GameObject> ObjectPool = new Queue<GameObject>();
//            for(int i = 0; i<pool.size; i++)
//            {
//                GameObject obj = Instantiate(pool.prefab);
//    obj.SetActive(false);
//                obj.transform.SetParent(UiCellCountcontainer.transform);
//                ObjectPool.Enqueue(obj);
//            }
//ag, OblSlotRedCell ";
//    string HelperTCellTag = "CellSlotHelperCell";
//public GameObject SpawnFroomPool(string tag)
//{
//    if (!PoolDictionary.ContainsKey(tag))
//    {
//        Debug.LogWarning("pool With tag" + tag + "deosn't exist");
//        return null;
//    }
//    GameObject ObjectToSpawn = PoolDictionary[tag].Dequeue();
//    ObjectToSpawn.SetActive(true);
//    PoolDictionary[t);
//    return ObjectToSpawn;

//}
//#endregion Pooling


//public void ChangeCellType(int CellType)
//{
//    for (int i = 0; i < UiCellCountcontainer.transform.childCount; i++)
//    {
//        GameObject CellSlot = UiCellCountcontainer.transform.GetChild(i).gamet;
//        CellSlot.Setv yOrganeManager.organRedBloodCell1nager.organs[myOrganager.activeOrganIists[0].Cells.Count; i++)
//                {
//    GameOb RedCell = SpawnFroomPool(RedBloodCellTag);
//    //RedCell.trarm.parent = UiCellCountcontainer.transform;
//    RedCell.trans.SetParent(UiCellCountcontainer.transform);
//    RedCeransform.localScale = new Vector3(1, 1, 1);
//    Image ellBGColor = RedCell.GetComponent<Image>();
//    dCellBGColor.color = RedBloodCell.MyColor;

//    GameObject RedCellImageObj = RedCell.transform.GetChild(1).gameObject;
//    Image RedCellIm = RedCellImageObject.GetComponent<Image>()ge.gameObjectnsform.localSca new Vector3(.6f, .6RedBloodCell10nager.organs[myOrganager.activeOrganIists[1].Cells.Count; i++)
//                {
//        GameOb RedCell = SpawnFroomPool(RedBloodCellTag);
//        // RedCell.trarm.parent = UiCellCountcontainer.transform;
//        RedCell.trans.SetParent(UiCellCountcontainer.transform);
//        RedCeransform.localScale = new Vector3(1, 1, 1);
//        Image ellBGColor = RedCell.GetComponent<Image>();
//        dCellBGColor.color = RedBloodCell.MyColor;

//        GameObject RedCellImageObj = RedCell.transform.GetChild(1).gameObject;
//        Image RedCellIm = RedCellImageObject.GetComponent<Image>(); Image.gameObjtransform.locale = new Vector3(1f, RedBloodCell100nager.organs[myOrganager.activeOrganIists[2].Cells.Count; i++)
//                {
//            GameOb RedCell = SpawnFroomPool(RedBloodCellTag);
//            //RedCell.trarm.parent = UiCellCountcontainer.transform;
//            RedCell.trans.SetParent(UiCellCountcontainer.transform);
//            RedCeransform.localScale = new Vector3(1, 1, 1);
//            Image ellBGColor = RedCell.GetComponent<Image>();
//            dCellBGColor.color = RedBloodCell.MyColor;

//            GameObject RedCellImageObj = RedCell.transform.GetChild(1).gameObject;
//            Image RedCellIm = RedCellImageObject.GetComponent<Image>();
//            mage.gameObjercale = new Vec(1f, 1f, 1f);

//        }
//    }

//    BuyButton.color = RedBloodCell.MyColor;
//    BuyButtonCost.color = RedBloodCell.MyColor;

//    BuyButtonCeage.sprite = RedBloodCell.C1            iyOrganeManager.organwhiteBloodCell1nager.organs[myOrganager.activeOrganIists[3].Cells.Count; i++)
//                {
//        GameObjectteCell = SpawnFroomPool(WhiteBloodCellTag);
//        // WhiteCell.trarm.parent = UiCellCountcontainer.transform;
//        WhiteCell.trans.SetParent(UiCellCountcontainer.transform);
//        WhiteCeransform.localScale = new Vector3(1, 1, 1);
//        Image WhitlBGColor = WhiteCell.GetComponent<Image>();
//        WhellBGColor.color = WhiteBloodCell.MyColor;

//        GameObject WhiteCellImageObjecWhiteCell.transform.GetChild(1).gameObject;
//        Image WhiteCellImagWhiteCellImageObject.GetComponent<Image>();
//        WhiteCellImageite = WhiteBloodCellwhiteBloodCell10nager.organs[myOrganager.activeOrganIists[4].Cells.Count; i++)
//                {
//            GameObjectteCell = SpawnFroomPool(WhiteBloodCellTag);
//            //WhiteCell.trarm.parent = UiCellCountcontainer.transform;
//            WhiteCell.trans.SetParent(UiCellCountcontainer.transform);
//            WhiteCeransform.localScale = new Vector3(1, 1, 1);
//            Image WhitlBGColor = WhiteCell.GetComponent<Image>();
//            WhellBGColor.color = WhiteBloodCell.MyColor;

//            GameObject WhiteCellImageObjecWhiteCell.transform.GetChild(1).gameObject;
//            Image WhiteCellImagWhiteCellImageObject.GetComponent<Image>();
//            hiteCellImage.se = WhiteBloodCell.CwhiteBloodCell100nager.organs[myOrganager.activeOrganIists[5].Cells.Count; i++)
//                {
//                teCell = SpawnFroomPool(WhiteBloodCellTag);
//                WhiteCell.trans.SetParent(UiCellCountcontainer.transform);
//                WhiteCeransform.localScale = new Vector3(1, 1, 1);
//                Image WhitlBGColor = WhiteCell.GetComponent<Image>();
//                WhellBGColor.color = WhiteBloodCell.MyColor;

//                GameObject WhiteCellImageObjecWhiteCell.transform.GetChild(1).gameObject;
//                Image WhiteCellImagWhiteCellImageObject.GetComponent<Image>();
//                tite = WhiteBloll.Cellx100;


//            }
//        }

//        BuyButton.color = WhiteBloodCell.MyColor;
//        BuyButtonCost.color = WhiteBloodCell.MyColor;
//        BuyButtonCmage.sprite = WhiteBloodCell            iyOrganeManager.organHelperCell1nager.organs[myOrganager.activeOrganIists[6].Cells.Count; i++)
//                {
//            GameObjeclperCellO = SpawnFroomPool(HelperTCellTag);
//            //HelperCellO.trarm.parent = UiCellCountcontainer.transform;
//            HelperCellO.trans.SetParent(UiCellCountcontainer.transform);
//            HelperCelransform.localScale = new Vector3(1, 1, 1);
//            Image HelperCGColor = HelperCellO.GetComponent<Image>();
//            perCellBGColor.color = HelperCell.MyColor;

//            GameObject HelperCellImageObject lperCellO.transform.GetChild(1).gameObject;
//            Image HelperCellImageelperCellImageObject.GetComponent<Image>();
//            HelperCellI.sprite = HelperCellHelperCell10nager.organs[myOrganager.activeOrganIists[7].Cells.Count; i++)
//                {
//                GameObjeclperCellO = SpawnFroomPool(HelperTCellTag);
//                //HelperCellO.trarm.parent = UiCellCountcontainer.transform;
//                HelperCellO.trans.SetParent(UiCellCountcontainer.transform);
//                HelperCelransform.localScale = new Vector3(1, 1, 1);
//                Image HelperCGColor = HelperCellO.GetComponent<Image>();
//                perCellBGColor.color = HelperCell.MyColor;

//                GameObject HelperCellImageObject lperCellO.transform.GetChild(1).gameObject;
//                Image HelperCellImageelperCellImageObject.GetComponent<Image>();
//                HelperCellImsprite = HelperCell.HelperCell100nager.organs[myOrganager.activeOrganIists[8].Cells.Count; i++)
//                {
//                    GameObjeclperCellO = SpawnFroomPool(HelperTCellTag);
//                    //HelperCellO.trarm.parent = UiCellCountcontainer.transform;
//                    HelperCellO.trans.SetParent(UiCellCountcontainer.transform);
//                    HelperCelransform.localScale = new Vector3(1, 1, 1);
//                    Image HelperCGColor = HelperCellO.GetComponent<Image>();
//                    perCellBGColor.color = HelperCell.MyColor;

//                    GameObject HelperCellImageObject lperCellO.transform.GetChild(1).gameObject;
//                    Image HelperCellImageelperCellImageObject.GetComponent<Image>();
//                    mage.sprite = erCell.Cellx1;
//                }
//            }

//            BuyButton.color = HelperCell.MyColor;
//            BuyButtonCost.color = HelperCell.MyColor;
//            ttonCellImage.sprite = HelperCell.Cellx1;
//        }



//        MyUILeanTween.FinishChangeCelType();

//    }


//    public float if (celltype == 1)
//{
//    CellTotal = RedBloodCell1 + RedBloodCell10 + RedBloodCell100;
//}
//else if (celltype == 2)
//{
//    CellTotal = whiteBloodCell1 + whiteBloodCell10 + whiteBloodCell100;
//}
//else if (celltype == 3)
//{
//    CellTotal = HelperCell1 + HelperCell10 + HelperCell100;
//}
//{
//    Debug.Log("Organ Manager is null");
//}


//return CellTotal;

//    }
//}
