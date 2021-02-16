using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrganSystemUI : MonoBehaviour
{
    // organSystemUI
    public Organ[] organScriptableObject;
    public GameObject[] UIOrgans;
    public List<Organ> AvailableOrgans;
    public int OrganLevelClearance;

    // Ui Show
    bool Expanded;
    public Animator UIAnimator;
    public GameObject ItemContainer;


    private void Start()
    {
        OrganLevelClearance = Stats.stats.BodyLevel;
        Expanded = false;
        CheckAvailableOrgans();
        OrganAddDisplay();
    }

    private void CheckAvailableOrgans()
    {
        for (int i = 0; i < OrganLevelClearance; i++)
        {
            AvailableOrgans.Add(organScriptableObject[i]);
            Debug.Log("added" + organScriptableObject[i].OrganName);

        }

    }

    public void OrganAddDisplay()
    {
        
        for (int i = 0; i < UIOrgans.Length; i++)
        {
            for (int j = 0; j < AvailableOrgans.Count; j++)
            {
                if (i <= AvailableOrgans[j].OrganLevelClearence)
                {
                    UIOrgans[i].SetActive(true);
                }
                else
                {
                    UIOrgans[i].SetActive(false);
                }
            }
        }

     }

    public void ExpandUI()
    {
        StartCoroutine(ExpandAndShrinkUI());
    }

    IEnumerator ExpandAndShrinkUI()
    {
        if (Expanded)
        {
            UIAnimator.SetTrigger("Shrink");
            yield return new WaitForSeconds(1);
            ItemContainer.SetActive(false);
            Expanded = false;
               
        }
        else
        {
            ItemContainer.SetActive(true);
            yield return new WaitForSeconds(1);
            Expanded = true;
        }
    }


   

}
