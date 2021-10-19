using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.iOS;

public class PlayfabNoEmailLogin : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject SignInScreen;

    [Header("Status Bools")]
    [SerializeField] bool mobileLogin;
    [SerializeField] bool signInScreenShown;

    public void Initialize()
    {
        PlayerPrefs.DeleteAll();
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "231EE";
        }
        InitialMobileLogin();
        void InitialMobileLogin()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, result =>
                {
                    Debug.Log("Initial Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserAccountInfo(result.PlayFabId);
             

                }, PlayFabError =>
                {
                    Debug.Log("Initial Mobile login error");
                });
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, result =>
                {
                    Debug.Log("Initial Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserAccountInfo(result.PlayFabId);
                    

                }, PlayFabError =>
                {
                    Debug.Log("Initial Mobile login error");
                });
            }
            else // Editor: Remove this in build
            {
                var requestEditor = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithIOSDeviceID(requestEditor, result =>
                {
                    Debug.Log("Initial Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserAccountInfo(result.PlayFabId);

                }, PlayFabError =>
                {
                    Debug.Log("Initial Mobile login error");
                });
            }
        }
    }

    #region API Methods
    void CheckUserAccountInfo(string PlayfabID)
    {
        var Request = new GetAccountInfoRequest { PlayFabId = PlayfabID };
        PlayFabClientAPI.GetAccountInfo(Request,
        result =>
        {
            if (result.AccountInfo.FacebookInfo != null)
            {
                EndLogin();
                Debug.Log("Has facebook Linked");
            }
            else if (result.AccountInfo.GoogleInfo != null)
            {
                EndLogin();
                Debug.Log("Has Google Linked");
            }
            else
            {
                ShowSignInScreen();
                Debug.Log("Doesnt have linked account");
            }
        },
        failure =>
        {
            ShowSignInScreen();
            Debug.LogWarning("Failed to retrieve user account info");
        });
    }
    #endregion API Methods

    #region Screen Methods
    void ShowSignInScreen()
    {
        if (!signInScreenShown)
        {
            signInScreenShown = true;
            SignInScreen.transform.localScale = Vector3.zero;
            LeanTween.scale(SignInScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
        
    }

    #endregion Screen Methods

    #region OnClick Methods
    public void OnClickSignInWithFacebook()
    {
        if (mobileLogin)
        {
            var LinkRequest = new LinkFacebookAccountRequest { };
        }

    }
    public void OnClickSignInWithGoogle()
    {

    }
    #endregion OnClick Methods

    #region Get Methods
    public static string GetMobileID()
    {
        string MobileId;
        if ((Application.platform == RuntimePlatform.IPhonePlayer) || SystemInfo.deviceModel.Contains("iPad"))
        {
            string DeviceID = Device.vendorIdentifier;
            MobileId = DeviceID;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string DeviceID = SystemInfo.deviceUniqueIdentifier;
            MobileId = DeviceID;
        }
        else
        {
            string DeviceID = SystemInfo.deviceUniqueIdentifier;
            MobileId = DeviceID;
        }
        return MobileId;
    }
    #endregion Get Methods

    void EndLogin()
    {

    }
}

