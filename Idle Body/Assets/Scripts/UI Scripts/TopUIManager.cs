using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUIManager : MonoBehaviour
{
    UITopLeanTween myUITopLeanTween;
    
   
    void Start()
    {
        myUITopLeanTween = GetComponent<UITopLeanTween>();
        
    }

    public void OpenStore()
    {
        myUITopLeanTween.TransitionIn();
        
        
    }

    private async void EnableStore()
    {

    }
    //IEnumerator 

    public void CloseStore()
    {

    }

    

  
}
