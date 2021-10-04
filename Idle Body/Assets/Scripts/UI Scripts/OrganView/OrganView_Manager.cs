using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrganView_Manager : MonoBehaviour
{
    GameManager gameManager = GameManager.gameManager;
    OrganManager organManager;
    OrganView_UI_Animation myOrganViewAnimation;


    [Header("Organ References")]

    [SerializeField] List<int> UnlockedOrgans;
    [SerializeField] int currentOrganType;

    [Space(10)]
    [Header("Buttons and UI")]
    [SerializeField] TextMeshProUGUI OrganName;
    [System.Serializable]
    public class BuyButtom
    {
        public TextMeshProUGUI CostText;
        public Button MainButton;
        public GameObject EnergyCostObject;
    }

    [SerializeField] BuyButtom PlatletBuyButton;
    [SerializeField] BuyButtom BuyOrganButton;
    [SerializeField] BuyButtom UpgradeMultiplierButton;


    [System.Serializable]
    public class OrganHolder
    {
        public int position;
        public GameObject[] OrganObjects;
    }
    [Header("New Organ UI")]
    public bool newOrganUI;
    [System.Serializable]
    public class NewOrganObject
    {
        public string name;
        public CanvasGroup EnergyCostCG;
        public TextMeshProUGUI EnergyCost;
        public TextMeshProUGUI ComplexityCost;
        public GameObject Object;
    }
    [SerializeField] NewOrganObject[] newOrganObjects;

    [Header("Holders")]
    [SerializeField] List<OrganHolder> organHolders;
    public int MiddleOrganHolder = 1;

    [SerializeField] Sprite[] OrganSprites;
    [SerializeField] Vector2[] OrganEyePositions;

    [Header("Organ Indicator")]
    [SerializeField] GameObject[] organIndicators;
    [SerializeField] Vector2[] organIndicatorsPos;
    [SerializeField] GameObject OrganIndicatorObject;

    [Header("Platelets")]
    PlatletManager plateletManager;
    [Header("Testing")]
    public string LeftOrganString;
    public string MiddleOrganString;
    public string RightOrganString;
    public bool AllOrgansUnlocked;

    [Header("OrganInfo")]
    OrganInfo organDispalyInfo;

    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.gameManager;
        organManager = gameManager.organManager;
        

    }
    public void CustomStart()
    {
        if (gameManager == null)
            gameManager = GameManager.gameManager;
        organManager = gameManager.organManager;
        myOrganViewAnimation = transform.GetComponent<OrganView_UI_Animation>();
        plateletManager = FindObjectOfType<PlatletManager>();
        organDispalyInfo = FindObjectOfType<OrganInfo>();
        

        plateletManager.CustomStart();
        UpdateUnlockedOrgans();
        UpdateOrgans();
        UpdateButtons();
        SetOrganIndicator();
        UpdateNewOrganUI();
        AudioManager.Instance.Play("Music");
    }

    public void UpdateOrgans(bool anim = true)
    {
        //bool ChangeUI = false;
        for (int o = 0; o < organHolders.Count; o++) // go through all 3 organ holders 
        {
            bool NewOrganScreen = false;
            int organType = 0;

            switch (organHolders[o].position)
            {
                case 0: // if position is left (previous organ)
                    int t = currentOrganType - 1;
                    if (t < 0)
                    {
                        t = UnlockedOrgans.Count - 1;
                        if (!AllOrgansUnlocked)
                        {
                            NewOrganScreen = true;
                        }
                    }
                    if (NewOrganScreen)
                    {
                        LeftOrganString = "New Organ";
                        plateletManager.ClearScreen(0);
                    }
                    else
                    {
                        organType = UnlockedOrgans[t];
                        LeftOrganString = organManager.organTypes[UnlockedOrgans[t]].Name;
                        plateletManager.UpdatePlatelets(0, UnlockedOrgans[t]);
                    }
                    break;
                case 1: // if position is Middle (Current in screen)
                    if (currentOrganType == UnlockedOrgans.Count - 1)
                    {
                        if (!AllOrgansUnlocked)
                        {
                            NewOrganScreen = true;
                            newOrganUI = true;
                            MiddleOrganString = "New Organ";
                            OrganName.text = "New Organ";
                            plateletManager.ClearScreen(1);

                        }
                        else
                        {

                            newOrganUI = false;
                            organType = UnlockedOrgans[currentOrganType];
                            MiddleOrganString = organManager.organTypes[organType].Name;
                            OrganName.text = organManager.organTypes[organType].Name;
                            Debug.Log("Clearing Middle Shit");
                            plateletManager.UpdatePlatelets(1, organType);
                        }
                    }
                    else
                    {

                        newOrganUI = false;
                        organType = UnlockedOrgans[currentOrganType];
                        MiddleOrganString = organManager.organTypes[organType].Name;
                        OrganName.text = organManager.organTypes[organType].Name;
                       
                    }
                    break;
                case 2: // if position is Right (next Organ)
                    int t2 = currentOrganType + 1;
                    if (t2 > UnlockedOrgans.Count - 1)
                    {

                        t2 = 0;

                    }
                    if (t2 == UnlockedOrgans.Count - 1)
                    {
                        if (!AllOrgansUnlocked)
                        {
                            NewOrganScreen = true;
                        }
                    }
                    if (NewOrganScreen)
                    {
                        RightOrganString = "New Organ";
                        plateletManager.ClearScreen(2);
                    }
                    else
                    {
                        organType = UnlockedOrgans[t2];
                        RightOrganString = organManager.organTypes[organType].Name;
                        plateletManager.UpdatePlatelets(2, UnlockedOrgans[t2]);
                    }
                    break;
            } // End case


            for (int i = 0; i < organHolders[o].OrganObjects.Length; i++) // go through the 5 organ objects inside each organ holder
            {
                if (!NewOrganScreen)
                {
                    if (organManager.organTypes[organType].organs.Count != 0)
                    {
                        if (i < organManager.organTypes[organType].organs.Count)
                        {
                            if (!organHolders[o].OrganObjects[i].activeSelf)
                            {
                                organHolders[o].OrganObjects[i].SetActive(true);
                                organHolders[o].OrganObjects[i].TryGetComponent(out OrganObject organObject);
                                bool lungs = false;
                                if (organType == 2)
                                {
                                    lungs = true;
                                }
                                organObject.CustomStart(OrganSprites[organType], OrganEyePositions[organType], lungs);
                                organHolders[o].OrganObjects[i].transform.localScale = Vector3.zero;
                                if (i == 3 || i == 1)
                                {
                                    if (anim)
                                    {
                                        LTDescr l = LeanTween.scale(organHolders[o].OrganObjects[i], new Vector3(-1, 1, 1), 1).setEase(LeanTweenType.easeInOutExpo);
                                    }
                                    else
                                    {
                                        organHolders[o].OrganObjects[i].transform.localScale = new Vector3(-1, 1, 1);
                                    }

                                }
                                else
                                {
                                    if (anim)
                                    {
                                        LTDescr l = LeanTween.scale(organHolders[o].OrganObjects[i], Vector3.one, 1).setEase(LeanTweenType.easeInOutExpo);
                                    }
                                    else
                                    {
                                        organHolders[o].OrganObjects[i].transform.localScale = new Vector3(1, 1, 1);
                                    }
                                }

                            }
                            else
                            {
                                organHolders[o].OrganObjects[i].TryGetComponent(out OrganObject organObject);
                                bool lungs = false;
                                if (organType == 2)
                                {
                                    lungs = true;
                                }
                                organObject.CustomStart(OrganSprites[organType], OrganEyePositions[organType], lungs);
                            }
                        }
                        else
                        {
                            organHolders[o].OrganObjects[i].SetActive(false);
                        }
                    }
                    else
                    {
                        organHolders[o].OrganObjects[i].SetActive(false);
                    }
                }
                else
                {
                    organHolders[o].OrganObjects[i].SetActive(false);
                }
            }
        }
        ToggleNewOrganUI();

    }

    public void ToggleOrgansButtons(bool Interactable)
    {
        for (int o = 0; o < organHolders.Count; o++)
        {
            for (int i = 0; i < organHolders[o].OrganObjects.Length; i++)
            {
                organHolders[o].OrganObjects[i].TryGetComponent(out ButtonHold button);
                if (Interactable)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
            }
        }
    }

    public void newOrgan(int organType)
    {
        UpdateUnlockedOrgans();
        int index = 12;
        int[] Unlocked = UnlockedOrgans.ToArray();
        if (UnlockedOrgans.Contains(organType))
        {
            index = System.Array.IndexOf(Unlocked, organType);
        }
        currentOrganType = index;
        organManager.activeOrganType = UnlockedOrgans[currentOrganType];
        UpdateOrgans();
        ToggleNewOrganUI();
        SetOrganIndicator();
        UpdateNewOrganUI();
        
    }
    private void UpdateNewOrganUI()
    {
        for (int i = 0; i < newOrganObjects.Length; i++)
        {
            if (organManager.organTypes[i].unlocked)
            {
                newOrganObjects[i].Object.SetActive(false);
            }
            else
            {
                newOrganObjects[i].ComplexityCost.text = organManager.organTypes[i].ComplexityCost[0].ToString();
                newOrganObjects[i].EnergyCost.text = AbbreviationUtility.AbbreviateNumber(organManager.organTypes[i].PointCost[0]);
                newOrganObjects[i].Object.SetActive(true);
                if (gameManager.pointsManager.totalPoints < organManager.organTypes[i].PointCost[0])
                {
                    newOrganObjects[i].EnergyCostCG.alpha = 0.7f;
                }
                else
                {
                    newOrganObjects[i].EnergyCostCG.alpha = 1;
                }
                int ComplexityCost = gameManager.pointsManager.ComplexityPoints + Mathf.FloorToInt(organManager.organTypes[i].ComplexityCost[0]);
                newOrganObjects[i].Object.TryGetComponent(out CanvasGroup CG);
                if (CG != null)
                {
                    if (ComplexityCost > gameManager.pointsManager.ComplexityMaxPoints)
                    {
                        CG.alpha = 0.5f;
                    }
                    else
                    {
                        CG.alpha = 1f;
                    }
                }

            }
        }
    }
    public void UpdateOrganViews(bool left)
    {

        if (left)
        {
            currentOrganType -= 1;
            if (currentOrganType < 0)
            {
                currentOrganType = UnlockedOrgans.Count - 1;
            }
            UpdateOrganIndicator(true);
            plateletManager.UpdatePlateletHolderPosition(true);

        }
        else
        {
            currentOrganType += 1;
            if (currentOrganType > UnlockedOrgans.Count - 1)
            {
                currentOrganType = 0;
            }
            UpdateOrganIndicator(false);
            plateletManager.UpdatePlateletHolderPosition(false);
        }
        organManager.activeOrganType = UnlockedOrgans[currentOrganType];

        for (int i = 0; i < organHolders.Count; i++)
        {
            if (!left)
            {
                //Debug.Log("Move Left");
                organHolders[i].position -= 1;
                if (organHolders[i].position < 0)
                {
                    organHolders[i].position = 2;
                }

            }
            else
            {
                //Debug.Log("Move right");
                organHolders[i].position += 1;
                if (organHolders[i].position > 2)
                {
                    organHolders[i].position = 0;
                }
            }
        }
        UpdateOrgans(false);
        UpdateButtons();
    }


    void UpdateUnlockedOrgans()
    {
        UnlockedOrgans.Clear();
        for (int i = 0; i < organManager.organTypes.Length; i++)
        {
            if (organManager.organTypes[i].unlocked)
            {
                UnlockedOrgans.Add(i);
            }
            if (i == organManager.activeOrganType)
            {
                if (UnlockedOrgans.Count != 0)
                {
                    currentOrganType = UnlockedOrgans.Count - 1;
                }
            }
        }
        if (UnlockedOrgans.Count < 12)
        {
            AllOrgansUnlocked = false;
            UnlockedOrgans.Add(12);
        }
        else
        {
            AllOrgansUnlocked = true;
        }
    }

    void ToggleNewOrganUI()
    {
        if (newOrganUI)
        {
            if (myOrganViewAnimation != null)
            {
                myOrganViewAnimation.GoToBuyOrganUI();
                toggleButtonsInteractive(false);
                PositionOrganIndicator(2);
            }
        }
        else
        {
            if (myOrganViewAnimation != null)
            {
                myOrganViewAnimation.GoToNormalOrganUI();
                toggleButtonsInteractive(true);
                if (myOrganViewAnimation.UIHidden)
                {
                    PositionOrganIndicator(1, true);
                }
                else
                {
                    PositionOrganIndicator(0, true);
                }

            }

        }
    }

    public void PositionOrganIndicator(int pos, bool delay = false)
    {
        float delayTime = 0;
        if (delay)
        {
            delayTime = 0.5f;
        }
        switch (pos)
        {
            case 0: // normal
                LeanTween.cancel(OrganIndicatorObject);
                LeanTween.moveLocal(OrganIndicatorObject, new Vector3(0, -900, 0), 1f).setEase(LeanTweenType.easeOutElastic).setDelay(delayTime);
                break;
            case 1: // hidden
                LeanTween.cancel(OrganIndicatorObject);
                LeanTween.moveLocal(OrganIndicatorObject, new Vector3(0, -1300, 0), 1f).setEase(LeanTweenType.easeOutElastic).setDelay(delayTime);
                break;
            case 2: //new organ
                LeanTween.cancel(OrganIndicatorObject);
                LeanTween.moveLocal(OrganIndicatorObject, new Vector3(0, -700, 0), 1f).setEase(LeanTweenType.easeOutElastic).setDelay(0.5f);
                break;
        }
    }

    void SetOrganIndicator()
    {
        for (int i = 0; i < organIndicators.Length; i++)
        {
            organIndicators[i].TryGetComponent(out OrganIndicator ind);
            if (ind != null)
            {
                int organTypeID = currentOrganType;
                Vector2 scale = new Vector2();
                Vector2 position = new Vector2();
                Color color = new Color();
                switch (ind.Pos)
                {
                    case -2:
                        for (int o = 0; o < 2; o++)
                        {
                            organTypeID -= 1;
                            if (organTypeID < 0)
                            {
                                organTypeID = UnlockedOrgans.Count - 1;
                            }
                        }
                        scale = new Vector2(0f, 0f);
                        position = organIndicatorsPos[0];
                        color = new Color(1, 1, 1, 0f);
                        break;
                    case -1:
                        organTypeID -= 1;
                        if (organTypeID < 0)
                        {
                            organTypeID = UnlockedOrgans.Count - 1;
                        }
                        scale = new Vector2(152f, 173f);
                        position = organIndicatorsPos[1];
                        color = new Color(1, 1, 1, 0.5f);
                        break;
                    case 0:
                        organTypeID = currentOrganType;
                        scale = new Vector2(201f, 229f);
                        position = organIndicatorsPos[2];
                        color = new Color(1, 1, 1, 1);
                        break;
                    case 1:
                        organTypeID += 1;
                        if (organTypeID > UnlockedOrgans.Count - 1)
                        {
                            organTypeID = 0;
                        }
                        scale = new Vector2(152f, 173f);
                        position = organIndicatorsPos[3];
                        color = new Color(1, 1, 1, 0.5f);
                        break;
                    case 2:
                        for (int o = 0; o < 2; o++)
                        {
                            organTypeID += 1;
                            if (organTypeID > UnlockedOrgans.Count - 1)
                            {
                                organTypeID = 0;
                            }
                        }
                        scale = new Vector2(0f, 0f);
                        position = organIndicatorsPos[4];
                        color = new Color(1, 1, 1, 0f);
                        break;
                }
                ind.setImage(OrganSprites[UnlockedOrgans[organTypeID]]);
                LeanTween.cancel(organIndicators[i]);
                LeanTween.moveLocal(organIndicators[i], position, 0).setEase(LeanTweenType.easeInOutExpo);
                organIndicators[i].TryGetComponent(out RectTransform rect);
                if (rect != null)
                {
                    LeanTween.size(rect, scale, 0).setEase(LeanTweenType.easeInOutExpo);
                }
                organIndicators[i].TryGetComponent(out CanvasGroup cg);
                if (cg != null)
                {
                    LeanTween.alphaCanvas(cg, color.a, 0);
                }
            }
        }
    }

    public void UpdateOrganIndicator(bool left, float animTime = 0.5f)
    {
        for (int i = 0; i < organIndicators.Length; i++)
        {
            organIndicators[i].TryGetComponent(out OrganIndicator ind);
            if (ind != null)
            {
                Vector2 scale = new Vector2();
                Vector2 position = new Vector2();
                Color color = new Color();
                int newPos;
                bool updateImage = false;
                if (!left)
                {
                    newPos = ind.Pos - 1;
                    if (newPos < -2)
                    {
                        newPos = 2;
                        updateImage = true;
                    }
                }
                else
                {
                    newPos = ind.Pos + 1;
                    if (newPos > 2)
                    {
                        newPos = -2;
                        updateImage = true;

                    }
                }
                switch (newPos)
                {
                    case 0: //Middle
                        scale = new Vector2(201f, 229f);
                        position = organIndicatorsPos[2];
                        color = new Color(1, 1, 1, 1);
                        break;
                    case 1: //right
                        scale = new Vector2(152f, 173f);
                        position = organIndicatorsPos[3];
                        color = new Color(1, 1, 1, 0.5f);
                        break;
                    case 2: //right 2
                        scale = new Vector2(0f, 0f);
                        position = organIndicatorsPos[4];
                        color = new Color(1, 1, 1, 0f);
                        break;
                    case -1: //left
                        scale = new Vector2(152f, 173f);
                        position = organIndicatorsPos[1];
                        color = new Color(1, 1, 1, 0.5f);
                        break;
                    case -2: //left 2
                        scale = new Vector2(0f, 0f);
                        position = organIndicatorsPos[0];
                        color = new Color(1, 1, 1, 0f);
                        break;
                }

                if (updateImage)
                {
                    if (newPos == -2)
                    {
                        int OrganFarLeft = currentOrganType;
                        for (int o = 0; o < 2; o++)
                        {
                            OrganFarLeft -= 1;
                            if (OrganFarLeft < 0)
                            {
                                OrganFarLeft = UnlockedOrgans.Count - 1;
                            }
                        }
                        //Debug.Log(OrganFarRight);
                        //Debug.Log("set image far right to: " + UnlockedOrgans[OrganFarRight]);
                        ind.setImage(OrganSprites[UnlockedOrgans[OrganFarLeft]]);
                    }
                    if (newPos == 2)
                    {
                        int OrganFarRight = currentOrganType;
                        for (int o = 0; o < 2; o++)
                        {
                            OrganFarRight += 1;
                            if (OrganFarRight > UnlockedOrgans.Count - 1)
                            {
                                OrganFarRight = 0;
                            }
                        }
                        //Debug.Log(OrganFarLeft);
                        //Debug.Log("set image far left to: " + UnlockedOrgans[OrganFarLeft]);
                        ind.setImage(OrganSprites[UnlockedOrgans[OrganFarRight]]);
                    }
                }
                ind.Pos = newPos;
                LeanTween.cancel(organIndicators[i]);
                LeanTween.moveLocal(organIndicators[i], position, animTime).setEase(LeanTweenType.easeInOutExpo);
                organIndicators[i].TryGetComponent(out RectTransform rect);
                if (rect != null)
                {
                    LeanTween.size(rect, scale, animTime).setEase(LeanTweenType.easeInOutExpo);
                    LeanTween.alpha(organIndicators[i], color.a, animTime);
                }
                organIndicators[i].TryGetComponent(out CanvasGroup cg);
                if (cg != null)
                {
                    LeanTween.alphaCanvas(cg, color.a, animTime);
                }

            }

        }
    }

    public void toggleButtonsInteractive(bool interactable)
    {
        if (interactable)
        {
            PlatletBuyButton.MainButton.interactable = true;
            BuyOrganButton.MainButton.interactable = true;
            UpgradeMultiplierButton.MainButton.interactable = true;
        }
        else
        {
            PlatletBuyButton.MainButton.interactable = false;
            BuyOrganButton.MainButton.interactable = false;
            UpgradeMultiplierButton.MainButton.interactable = false;
        }
    }

    public void UpdateButtons()
    {
        UpdateButtonColors();
        UpdateCosts();
    }
    private void UpdateCosts()
    {
        if (organManager.activeOrganType < 12)
        {
            BuyOrganButton.CostText.text = AbbreviationUtility.AbbreviateNumber(organManager.organTypes[organManager.activeOrganType].PointCost[organManager.organTypes[organManager.activeOrganType].organs.Count]);
            UpgradeMultiplierButton.CostText.text = AbbreviationUtility.AbbreviateNumber(organManager.organTypes[organManager.activeOrganType].pointMultiplierCost);
            PlatletBuyButton.CostText.text = AbbreviationUtility.AbbreviateNumber(organManager.organTypes[organManager.activeOrganType].plateletCost);
        }
    }
    private void UpdateButtonColors()
    {
        if (organManager.activeOrganType < 12)
        {
            if (gameManager.pointsManager.totalPoints < organManager.organTypes[organManager.activeOrganType].PointCost[organManager.organTypes[organManager.activeOrganType].organs.Count])
            {
                BuyOrganButton.EnergyCostObject.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 0.7f;
                ColorBlock CB = BuyOrganButton.MainButton.colors;
                CB.normalColor = new Color(1f, 1f, 1f, 0.5f);
                CB.selectedColor = new Color(1f, 1f, 1f, 0.5f);
                BuyOrganButton.MainButton.colors = CB;
                //Debug.Log("whoppsies: " + AbbreviationUtility.AbbreviateNumber(gameManager.pointsManager.totalPoints) + "/" + AbbreviationUtility.AbbreviateNumber(organManager.organTypes[organManager.activeOrganType].PointCost[organManager.organTypes[organManager.activeOrganType].organs.Count]));
            }
            else
            {
                BuyOrganButton.EnergyCostObject.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 1f;
                ColorBlock CB = BuyOrganButton.MainButton.colors;
                CB.normalColor = new Color(1f, 1f, 1f, 1f);
                CB.selectedColor = new Color(1f, 1f, 1f, 1f);
                BuyOrganButton.MainButton.colors = CB;
            }

            if (gameManager.pointsManager.totalPoints < organManager.organTypes[organManager.activeOrganType].pointMultiplierCost)
            {
                UpgradeMultiplierButton.EnergyCostObject.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 0.7f;
                ColorBlock CB = UpgradeMultiplierButton.MainButton.colors;
                CB.normalColor = new Color(1f, 1f, 1f, 0.5f);
                CB.selectedColor = new Color(1f, 1f, 1f, 0.5f);
                UpgradeMultiplierButton.MainButton.colors = CB;
            }
            else
            {
                UpgradeMultiplierButton.EnergyCostObject.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 0.7f;
                ColorBlock CB = UpgradeMultiplierButton.MainButton.colors;
                CB.normalColor = new Color(1f, 1f, 1f, 1f);
                CB.selectedColor = new Color(1f, 1f, 1f, 1f);
                UpgradeMultiplierButton.MainButton.colors = CB;
            }
        }
    }
    public void BuyPlatelet()
    {

        plateletManager.BuyPlatelet();
        UpdateCosts();
    }

    public void ShowOrganInfo(int OrganType, int OrganNumber)
    {
        organDispalyInfo.SetInfo(OrganType, OrganNumber);
        organDispalyInfo.Show();
    }

    #region backupCode

    //------- Previois UpdateOrgan------//
    /*
    void ShowOrgans2(bool anim = true)
    {
        for (int o = 0; o < organHolders.Count; o++) // go through all 3 organ holders 
        {
            for (int i = 0; i < organHolders[o].OrganObjects.Length; i++) // go through the 5 organ objects inside each organ holder
            {
                int organType = 0;
                bool NewOrganScreen = false;
                switch (organHolders[o].position) // check organ position
                {
                    case 0: // if position is left (previous organ)
                        int t = currentOrganType - 1;
                        if (t < 0)
                        {
                            t = UnlockedOrgans.Count - 1;
                            NewOrganScreen = true;
                        }
                        if (NewOrganScreen)
                        {
                            LefOrganString = "New Organ";
                        }
                        else
                        {
                            organType = UnlockedOrgans[t];
                            LefOrganString = organManager.organTypes[UnlockedOrgans[t]].Name;
                        }
                        break;
                    case 1: // if position is Middle (Current in screen)
                        if (currentOrganType == UnlockedOrgans.Count - 1)
                        {
                            NewOrganScreen = true;
                            MiddleOrganString = "New Organ";
                        }
                        else
                        {
                            organType = UnlockedOrgans[currentOrganType];
                            MiddleOrganString = organManager.organTypes[organType].Name;
                        }
                        break;
                    case 2: // if position is Right (next Organ)
                        int t2 = currentOrganType + 1;
                        if (t2 > UnlockedOrgans.Count - 1)
                        {
                            t2 = 0;
                        }
                        else if (t2 == UnlockedOrgans.Count - 1)
                        {
                            NewOrganScreen = true;
                        }
                        if (NewOrganScreen)
                        {
                            RightOrganString = "New Organ";
                        }
                        else
                        {
                            organType = UnlockedOrgans[t2];
                            RightOrganString = organManager.organTypes[organType].Name;
                        }
                        break;
                }

                if (!NewOrganScreen)
                {
                    if (organManager.organTypes[organType].organs.Count != 0)
                    {
                        if (i < organManager.organTypes[organType].organs.Count)
                        {
                            if (!organHolders[o].OrganObjects[i].activeSelf)
                            {
                                organHolders[o].OrganObjects[i].SetActive(true);
                                organHolders[o].OrganObjects[i].transform.localScale = Vector3.zero;
                                if (i == 3 || i == 1)
                                {
                                    if (anim)
                                    {
                                        LTDescr l = LeanTween.scale(organHolders[o].OrganObjects[i], new Vector3(-1, 1, 1), 1).setEase(LeanTweenType.easeInOutExpo);
                                    }
                                    else
                                    {
                                        organHolders[o].OrganObjects[i].transform.localScale = new Vector3(-1, 1, 1);
                                    }
                                }
                                else
                                {
                                    if (anim)
                                    {
                                        LTDescr l = LeanTween.scale(organHolders[o].OrganObjects[i], Vector3.one, 1).setEase(LeanTweenType.easeInOutExpo);
                                    }
                                    else
                                    {
                                        organHolders[o].OrganObjects[i].transform.localScale = new Vector3(1, 1, 1);
                                    }
                                }

                            }
                        }
                        else
                        {
                            organHolders[o].OrganObjects[i].SetActive(false);
                        }
                    }
                    else
                    {
                        organHolders[o].OrganObjects[i].SetActive(false);
                    }
                }
                else
                {
                    organHolders[o].OrganObjects[i].SetActive(false);
                    Debug.Log("screen " + o + " is new organ screen");
                }
            }

        }


    }

     */
    #endregion backupCode
}
