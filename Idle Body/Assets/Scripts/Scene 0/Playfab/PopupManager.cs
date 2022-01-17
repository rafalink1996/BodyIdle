using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{

    [Header("instance")]
    public static PopupManager instance;
    [Space(10)]
    [SerializeField] GameObject popUpManager;
    [Space(10)]
    [Header("PopUp Objects")]
    [SerializeField] GameObject DisclaimerPopUp;
    [SerializeField] GameObject NoAccountconfirmPopUp;
    [SerializeField] GameObject ConnectionErrorPopUp;
    [SerializeField] GameObject SigninErrorPopUp;
    [SerializeField] GameObject WelcomePopUp;

    [Header("PopUp Buttons")]
    [SerializeField] GameObject accountConfimrGoogle;
    [SerializeField] GameObject accountConfirmFacebook;

    [Header("PopUp Texts")]
    [SerializeField] TextMeshProUGUI DisclaimerBody;
    [Space(5)]
    [SerializeField] TextMeshProUGUI NoAccountconfirmBody1;
    [SerializeField] TextMeshProUGUI NoAccountconfirmBody2;


    [Header("Strings")]
    string GoogleNameWithColors = "<color=#4185F4>G</color><color=#E94234>O</color><color=#FBBC04>O</color><color=#4185F4>G</color><color=#33A853>L</color><color=#E94234>E</color>";
    string FacebookNameWithColors = "<color=#3b5998>Facebook</color>";
    string AppleNameWithColors = "<color=#989898> Apple </color>";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetTexts();
    }
    public void ClosePopUps()
    {
        PlayfabNoEmailLogin.instance.ShowSignInScreen();
        DisclaimerPopUp.SetActive(false);
        NoAccountconfirmPopUp.SetActive(false);
        ConnectionErrorPopUp.SetActive(false);
        SigninErrorPopUp.SetActive(false);
        popUpManager.SetActive(false);
    }

    void SetTexts()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DisclaimerBody.text = "By not signing in with a " + FacebookNameWithColors + " or " + AppleNameWithColors + " account, you risk your data being lost. Are you sure you want to continue?";
        }else if(Application.platform == RuntimePlatform.Android)
        {
            DisclaimerBody.text = "By not signing in with a " + FacebookNameWithColors + " or " + GoogleNameWithColors + " account, you risk your data being lost. Are you sure you want to continue?";
        }else
        {
            DisclaimerBody.text = "By not signing in with a " + FacebookNameWithColors + " account, you risk your data being lost. Are you sure you want to continue?";
        }
    }

    public enum PopUp {Disclaimer, ConfirmCreate, connectionError, LoginError, welcome};
    public void ShowPopUp(PopUp popType)
    {
       
        GameObject popUp = null;
        switch(popType)
        {
            case PopUp.Disclaimer:
                popUp = DisclaimerPopUp;
                break;
            case PopUp.ConfirmCreate:
                popUp = NoAccountconfirmPopUp;
                break;
            case PopUp.connectionError:
                popUp = ConnectionErrorPopUp;
                break;
            case PopUp.LoginError:
                popUp = SigninErrorPopUp;
                break;
            case PopUp.welcome:
                popUp = WelcomePopUp;
                break;
        }
        
        popUpManager.SetActive(true);
        popUp.SetActive(true);
        popUp.transform.localScale = Vector3.zero;
        LeanTween.scale(popUp, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
    }

    public void OnClickFirstSkip()
    {
        ShowPopUp(PopUp.Disclaimer);
    }

    public enum AccountType
    {
        facebook,
        gooogle,
    }
    public void ShowConfimCreateAccount(AccountType accountType)
    {
        switch (accountType)
        {
            case AccountType.facebook:
                NoAccountconfirmBody1.text = "There isn't a user registered to this " + FacebookNameWithColors + " account.";
                accountConfirmFacebook.SetActive(true);
                accountConfimrGoogle.SetActive(false);
                break;
            case AccountType.gooogle:
                NoAccountconfirmBody1.text = "There isn't a user registered to this " + GoogleNameWithColors + " account.";
                accountConfirmFacebook.SetActive(false);
                accountConfimrGoogle.SetActive(true);
                break;
        }
        popUpManager.SetActive(true);
        NoAccountconfirmPopUp.SetActive(true);
    }

}
