using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewController : MonoBehaviour
{
    public Animator ScrollviewAnimator;
    private bool Expanded;

    // cells
    public GameObject CellAdministratorItemContainer;
    public Animator CellAdministratorAnimator;


    
   
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void ExpandCellAdministratorView()
    {
        if (!Expanded)
        {
            StartCoroutine(expandCellAdministratorViewCo());
            Expanded = true;
        }
        else
        {
            StartCoroutine(shrinkCellAdministratorViewCo());
            Expanded = false;
        }
        
    }
    IEnumerator expandCellAdministratorViewCo()
    {
        ScrollviewAnimator.SetTrigger("Expand");
        yield return new WaitForSeconds(0.5f);
        CellAdministratorItemContainer.SetActive(true);
        
    }

    IEnumerator shrinkCellAdministratorViewCo()
    {
        CellAdministratorAnimator.SetTrigger("Shrink");
        yield return new WaitForSeconds(1);
        CellAdministratorItemContainer.SetActive(false);
        ScrollviewAnimator.SetTrigger("Shrink");


    }
    

}
