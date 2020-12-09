using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoopPartnersSystem : MonoBehaviour
{
/*
    public Image[] CoopPartnersImages;
    public int CoopPartners;
    public GameManager gameManager;
    public TextMeshProUGUI CoopParnterExessText;

    public int PlayAmnount;
    public float coopPartnerCost;
    public int coopPartnerCostTier;
    public float coopPartnerBaseCost = 100;
    public float quantity;

    public GameObject moreCoopPartnersBuuton;
    public Button firstCoopPartnerButton;

    public float timeBetweenPlays;


    public int CoopPartnersPlaying;


    public float CoopPartnersGPPS;
    public int CoopPartnersGPPSTier;

    //Display

    public TextMeshProUGUI LevelDisplay;
    public TextMeshProUGUI CostDisplay;
    string costText;



    



    // Start is called before the first frame update
    void Start()
    {
        

        coopPartnerBaseCost = 15;
        coopPartnerCost = 15;
        //InvokeRepeating("CoopPartnerPlay", 1, 3);
        //StartCoroutine(CoopPlaying());

    }

    // Update is called once per frame
    void Update()
    {

        CoopPartnersGPPS = (CoopPartners * (gameManager.GPPC * Mathf.Pow(1000, gameManager.GGPCTier- gameManager.GGPCTier)))/3;

        if (CoopPartners > 1)
        {
            timeBetweenPlays = 2.8f / CoopPartners;
        }
        else
        {
            timeBetweenPlays = 2.8f;
        }
        

        if (coopPartnerCost >= 1000)
        {
            coopPartnerCost /= 1000;
            coopPartnerCostTier++;

        }

        if (coopPartnerCost > 0 && coopPartnerCost < 1)
        {
            coopPartnerCost *= 1000;
            coopPartnerCostTier--;
        }


       

        coopPartnerCostTier = Mathf.Clamp(coopPartnerCostTier, 0, gameManager.Sufix.Length);


        for (int i = 0; i < CoopPartnersImages.Length; i++)
        {
            if ( i < CoopPartners)
            {
                CoopPartnersImages[i].enabled = true;
            }
            else
            {
                CoopPartnersImages[i].enabled = false;
            }
        }

        if (CoopPartners <= 25)
        {
            CoopParnterExessText.enabled = false;
        }
        else
        {
            CoopParnterExessText.enabled = true;
        }

        CoopParnterExessText.text = "x" + (CoopPartners - 25).ToString();

        // display

        LevelDisplay.text = "CoopPartners" + " " + CoopPartners.ToString();
        costText = CurrencyText(costText, coopPartnerCost, coopPartnerCostTier);
        CostDisplay.text = costText + " GM";

    }
    public void addCoopPartner()
    {
        if (gameManager.GamingPointsTier >= coopPartnerCostTier)
        {
            if (gameManager.GamingPointsTier == coopPartnerCostTier)
            {
                if (gameManager.GamingPoints >= coopPartnerCost)
                {
                    gameManager.GamingPoints -= coopPartnerCost;
                    quantity++;
                    coopPartnerCost *= 1.15f;
                    CoopPartners++;
                }
            }
            else
            {
                
                
                    gameManager.GamingPoints -= coopPartnerCost / Mathf.Pow(1000, gameManager.GamingPointsTier - coopPartnerCostTier);
                    quantity++;
                    coopPartnerCost *= Mathf.Pow(1.15f, gameManager.GamingPointsTier - coopPartnerCostTier);
                // coopPartnerCost *=  Mathf.Pow(1.05f, quantity);
                CoopPartners++;
                
               
            }
        }


     
       
    }
    public void CoopPartnerStart()
    {
        if (gameManager.GamingPointsTier >= coopPartnerCostTier)
        {
            if (gameManager.GamingPointsTier == coopPartnerCostTier)
            {
                if (gameManager.GamingPoints >= coopPartnerCost)
                {
                    gameManager.GamingPoints -= coopPartnerCost;
                    quantity++;
                    coopPartnerCost *= 1.15f;
                    CoopPartners++;
                    StartCoroutine(CoopPlaying());
                    firstCoopPartnerButton.interactable = false;
                    moreCoopPartnersBuuton.SetActive(true);
                }
            }
            else
            {


                gameManager.GamingPoints -= coopPartnerCost / Mathf.Pow(1000, gameManager.GamingPointsTier - coopPartnerCostTier);
                quantity++;
                coopPartnerCost *= Mathf.Pow(1.15f, gameManager.GamingPointsTier - coopPartnerCostTier);
                // coopPartnerCost *=  Mathf.Pow(1.05f, quantity);
                CoopPartners++;
                StartCoroutine(CoopPlaying());
                firstCoopPartnerButton.interactable = false;
                moreCoopPartnersBuuton.SetActive(true);


            }
        }
        
        

    }

 

    IEnumerator CoopPlaying()
    {
       
        
        while (true)
        {
            if (CoopPartnersPlaying <= CoopPartners)
            {
                yield return new WaitForSeconds(timeBetweenPlays);
                gameManager.PressButton(5 * gameManager.TeamworkLevel);
                CoopPartnersPlaying++;
                //Debug.Log("time" + Time.time);
                

            }
            else
            {
                CoopPartnersPlaying = 0;
                yield return new WaitForSeconds((3f-(timeBetweenPlays * CoopPartners)));

            }
        }
    }



    private string CurrencyText(string currencyText, float currency, int tier)
    {
        if (tier - 1 > -1)
        {
            currencyText = currency.ToString("#.00") + " " + gameManager.Sufix[tier - 1];
        }
        else
        {
            currencyText = currency.ToString("#.00");
        }
        return currencyText;
    }


    */

}
