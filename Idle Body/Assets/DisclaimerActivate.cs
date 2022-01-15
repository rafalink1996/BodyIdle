using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisclaimerActivate : MonoBehaviour
{
    [SerializeField] Button skipButton;
    // Start is called before the first frame update
    private void OnEnable()
    {
        skipButton.interactable = false;
        Invoke("ActivateButton", 1.5f);

    }
    void ActivateButton()
    {
        skipButton.interactable = true;
    }

    public void OnClickCancellInvoke()
    {
        CancelInvoke();
    }

}
