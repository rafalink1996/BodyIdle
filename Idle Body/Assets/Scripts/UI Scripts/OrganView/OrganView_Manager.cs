using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrganView_Manager : MonoBehaviour
{
    GameManager gameManager = GameManager.gameManager;
    OrganManager organManager;
    [Header("Organ References")]
    [SerializeField] Sprite[] OrganSprites;


    [System.Serializable]
    public class OrganHolder
    {
        public int position;
        public GameObject[] OrganObjects;
    }
       
    [Header ("Holders")]
    [SerializeField] List <OrganHolder> organHolders;
    public int MiddleOrganHolder = 1;

    [Header("Testing")]
    public string LefOrganString;
    public string MiddleOrganString;
    public string RightOrganString;
 
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
        ShowOrgans();
    }

    public void ShowOrgans(bool anim = true)
    {
        for (int o = 0; o< organHolders.Count; o++)
        {
            for (int i = 0; i < organHolders[o].OrganObjects.Length; i++)
            {
                int organType = 0;
                switch (organHolders[o].position)
                {
                    case 0:
                        organType = organManager.activeOranType -1;
                        if(organManager.activeOranType - 1 < 0)
                        {
                            organType = 11;
                        }
                        LefOrganString = organManager.organTypes[organType].Name;
                        break;
                    case 1:
                        organType = organManager.activeOranType;
                        MiddleOrganString = organManager.organTypes[organType].Name;
                        break;
                    case 2:
                        organType = organManager.activeOranType + 1;
                        if (organManager.activeOranType + 1 > 11)
                        {
                            organType = 0;
                        }
                        RightOrganString = organManager.organTypes[organType].Name;
                        break;
                }
                if (organManager.organTypes[organType].organs.Count != 0) {
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

        }
        

    }
    public void ToggleOrgansButtons(bool Interactable)
    {
        for (int o = 0; o < organHolders.Count; o++)
        {
            for (int i = 0; i < organHolders[o].OrganObjects.Length; i++)
            {
                organHolders[o].OrganObjects[i].TryGetComponent(out Button button);
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

    public void UpdateOrganViews(bool left)
    {
        for(int i = 0; i < organHolders.Count;i++)
        {
            if (!left)
            {
                Debug.Log("Move Left");
                organHolders[i].position -= 1;
                if(organHolders[i].position < 0)
                {
                    organHolders[i].position = 2;
                }
            }
            else
            {
                Debug.Log("Move right");
                organHolders[i].position += 1;
                if (organHolders[i].position > 2)
                {
                    organHolders[i].position = 0;
                }
            }
        }
        ShowOrgans(false);
    }



}
