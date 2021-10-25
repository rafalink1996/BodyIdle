using UnityEngine;
using UnityEngine.iOS;
using System.Collections.Generic;
using System;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

using Facebook.Unity;
using LoginResult = PlayFab.ClientModels.LoginResult;


public class PlayfabNoEmailLogin : MonoBehaviour
{

    [Header("Screens")]
    [SerializeField] GameObject SignInScreen;
    [Space(5)]

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI LoginErrorMessage;
    [Space(5)]
    [SerializeField] TextMeshProUGUI ConfirmCreateAccountBody1;
    [SerializeField] TextMeshProUGUI ConfirmCreateAccountBody2;

    [Header("Buttons")]
    [SerializeField] GameObject ConfirmFacebookAccountButton;
    [SerializeField] GameObject ConfirmGoogleAccountButton;

    [Header("Status Bools")]
    [SerializeField] bool mobileLogin;
    [SerializeField] bool signInScreenShown;
    [Space(5)]
    [SerializeField] bool FacebookLogin;
    [SerializeField] bool FacebookInitInitialized;
    [SerializeField] bool FacebookLoginChecked;
    [SerializeField] bool hasFacebookLinked;
    [Space(5)]
    [SerializeField] bool GoogleLogin;
    [SerializeField] bool hasGoogleLinked;
    [SerializeField] bool GoogleLoginChecked;


    [Header("Retry")]
    int loginRetryCount = 0;


    private void Awake()
    {

    }

    private void Start()
    {
        Debug.Log("Start");
        // ShowErrorScreen(ErrorScreenType.Google, ErrorScreenCode.errorLoginIn);
    }

    public void Initialize()
    {

        FB.Init(OnFBInitComplete, OnFBHideUnity => { });
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
                Debug.Log("Has facebook Linked");
                if (FB.IsInitialized)
                {
                    if (FB.IsLoggedIn)
                    {
                        EndLogin();
                    }
                }
                hasFacebookLinked = true;
            }
            else
            {
                Debug.Log("facebook not Linked");
                hasFacebookLinked = false;
            }
            if (result.AccountInfo.GoogleInfo != null)
            {
                Debug.Log("Has Google Linked");
                hasGoogleLinked = true;
            }
            else
            {
                Debug.Log("Google not Linked");
                hasGoogleLinked = false;
            }

            if (hasGoogleLinked && hasFacebookLinked)
            {
                Debug.Log("Everything linked");
                EndLogin();
            }
            else
            {
                ShowSignInScreen();
            }
        },
        failure =>
        {
            ShowSignInScreen();
            Debug.LogWarning("Failed to retrieve user account info");
        });
    }
    IEnumerator MobileLoginRetry(bool initial, string errorMesage, bool CreateAccount = false)
    {
        yield return new WaitForSeconds(1);
        loginRetryCount++;
        if (loginRetryCount > 3)
        {
            if (initial)
            {
                ShowSignInScreen();
            }
            else
            {
                DisplayError(ErrorCode.errorLoginIn, errorMesage);
            }
        }
        else
        {
            if (initial)
            {
                InitialMobileLogin();
            }
            else
            {
                MobileLogin(CreateAccount);
            }
        }
    }
    void InitialMobileLogin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), CreateAccount = false };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, result =>
            {
                Debug.Log("Initial Mobile login was a success");
                GameData.data.PlayfabLogin = true;
                loginRetryCount = 0;
                mobileLogin = true;
                CheckUserAccountInfo(result.PlayFabId);
            }, PlayFabError =>
            {
                switch (PlayFabError.Error)
                {
                    case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
                    case PlayFabErrorCode.APIConcurrentRequestLimitExceeded:
                    case PlayFabErrorCode.ConcurrentEditError:
                    case PlayFabErrorCode.DataUpdateRateExceeded:
                    case PlayFabErrorCode.DownstreamServiceUnavailable:
                    case PlayFabErrorCode.OverLimit:
                    case PlayFabErrorCode.ServiceUnavailable:
                        StartCoroutine(MobileLoginRetry(true, PlayFabError.Error.ToString()));
                        break;
                    default:
                        ShowSignInScreen();
                        loginRetryCount = 0;
                        break;
                }
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
                loginRetryCount = 0;
                mobileLogin = true;
                CheckUserAccountInfo(result.PlayFabId);
            }, PlayFabError =>
            {
                switch (PlayFabError.Error)
                {
                    case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
                    case PlayFabErrorCode.APIConcurrentRequestLimitExceeded:
                    case PlayFabErrorCode.ConcurrentEditError:
                    case PlayFabErrorCode.DataUpdateRateExceeded:
                    case PlayFabErrorCode.DownstreamServiceUnavailable:
                    case PlayFabErrorCode.OverLimit:
                    case PlayFabErrorCode.ServiceUnavailable:
                        StartCoroutine(MobileLoginRetry(true, PlayFabError.Error.ToString()));
                        break;
                    default:
                        ShowSignInScreen();
                        loginRetryCount = 0;
                        break;
                }
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
                loginRetryCount = 0;
                mobileLogin = true;
                CheckUserAccountInfo(result.PlayFabId);
            }, PlayFabError =>
            {
                switch (PlayFabError.Error)
                {
                    case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
                    case PlayFabErrorCode.APIConcurrentRequestLimitExceeded:
                    case PlayFabErrorCode.ConcurrentEditError:
                    case PlayFabErrorCode.DataUpdateRateExceeded:
                    case PlayFabErrorCode.DownstreamServiceUnavailable:
                    case PlayFabErrorCode.OverLimit:
                    case PlayFabErrorCode.ServiceUnavailable:
                        StartCoroutine(MobileLoginRetry(true, PlayFabError.Error.ToString()));
                        break;
                    default:
                        loginRetryCount = 0;
                        ShowSignInScreen();
                        break;
                }
                Debug.Log("Initial Mobile login error");
            });
        }
    }
    void MobileLogin(bool MobileCreateAccount = false, bool facebookLink = false, bool googleLink = false, bool skip = false)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), CreateAccount = MobileCreateAccount };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, result =>
            {
                GameData.data.PlayfabLogin = true;
                mobileLogin = true;
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    LinkAccounts();
                }

            }, PlayFabError =>
            {
                GameData.data.PlayfabLogin = false;
                mobileLogin = false;
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    DisplayError(ErrorCode.errorLoginIn, PlayFabError.Error.ToString());
                }
            });
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = MobileCreateAccount };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, result =>
            {
                GameData.data.PlayfabLogin = true;
                mobileLogin = true;
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    LinkAccounts();
                }
            }, PlayFabError =>
            {
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    DisplayError(ErrorCode.errorLoginIn, PlayFabError.Error.ToString());
                }
            });
        }
        else // Editor: Remove this in build
        {
            var requestEditor = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = MobileCreateAccount };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestEditor, result =>
            {
                GameData.data.PlayfabLogin = true;
                mobileLogin = true;
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    LinkAccounts();
                }
            }, PlayFabError =>
            {
                if (skip)
                {
                    EndLogin();
                }
                else
                {
                    DisplayError(ErrorCode.errorLoginIn, PlayFabError.Error.ToString());
                }

            });
        }

        void LinkAccounts()
        {
            if (googleLink)
            {

            }
            if (facebookLink)
            {
                if (FB.IsInitialized)
                {
                    if (FB.IsLoggedIn)
                    {
                        LinkFacebookAccount();
                    }
                }
            }
            if (!facebookLink && !googleLink)
            {
                Debug.Log("Mobile Login No Linking detected: Ending Login");
                EndLogin();
            }
        }
    }
    void UnlinkMobileID(bool facebook)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var unlinkIOSRequest = new UnlinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkIOSDeviceID(unlinkIOSRequest, success => {
                PlayFabClientAPI.ForgetAllCredentials();
                mobileLogin = false;
                if (facebook)
                {
                    PlayfabLoginWithFacebook();
                }
                else
                {
                    //Login con google
                }
            }, failure => {
                DisplayError(ErrorCode.errorFacebookLoginIn, failure.Error.ToString());
            });
        }else if(Application.platform == RuntimePlatform.Android)
        {
            
        }
    }

    void LinkMobileID()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
            {
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                switch (failed.Error)
                {
                    default:
                        EndLogin();
                        break;
                }
                Debug.Log("failed to link Mobile ID");
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var linkAndroidRequest = new LinkAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID() };
            PlayFabClientAPI.LinkAndroidDeviceID(linkAndroidRequest, result =>
            {
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                switch (failed.Error)
                {
                    default:
                        EndLogin();
                        break;
                }
                Debug.Log("failed to link Mobile ID");
            });
        }
        else
        {
            var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
            {
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                switch (failed.Error)
                {
                    default:
                        EndLogin();
                        break;
                }
                Debug.Log("failed to link Mobile ID");
            });
        }
    }



    #endregion API Methods
    #region OnClick Methods
    public void OnClickSignInWithFacebook()
    {
        LoginWithFacebook();
    }



    public void OnClickSignInWithGoogle()
    {

    }
    public void OnClickSkip()
    {
        if (mobileLogin)
        {
            EndLogin();
        }
        else
        {
            MobileLogin(true, false, false, true);
        }
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
    #region FACEBOOK

    private void OnFBInitComplete()
    {
        FacebookInitInitialized = true;
    }
    void LoginWithFacebook()
    {
        if (FB.IsInitialized)
        {
            // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
            if (FB.IsLoggedIn)
            {
                Debug.Log("User is logged in");
                CheckFacebookPlayfabAccount();
                //FB.LogOut();
            }
            else
            {
                // We invoke basic login procedure and pass in the callback to process the result
                FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
            }
        }
        else
        {
            FB.Init(FBCompleted, OnFBHideUnity => { });
            void FBCompleted()
            {
                if (FB.IsLoggedIn)
                {
                    Debug.Log("User is logged in");
                    CheckFacebookPlayfabAccount();
                }
                else
                {
                    FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
                }
            }
        }
    }

    void CheckFacebookPlayfabAccount()
    {
        if (mobileLogin)
        {
            if (hasFacebookLinked)
            {
                FacebookLoginChecked = true;
                FacebookLogin = true;
                Debug.Log("Has Facebook Linked");
                EndLogin();
            }
            else
            {
                LinkFacebookAccount();
            }
        }
        else
        {
            PlayfabLoginWithFacebook();
        }
    }

    void PlayfabLoginWithFacebook()
    {
        PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = false, AccessToken = AccessToken.CurrentAccessToken.TokenString },
           OnPlayfabFacebookAuthComplete =>
           {
               FacebookLoginChecked = true;
               FacebookLogin = true;
               hasFacebookLinked = true;
               Debug.Log("Facebook Account Login Completed with no mobile id linked");
               LinkMobileID();

           }, OnPlayfabFacebookAuthFailed =>
           {
               FacebookLoginChecked = true;
               FacebookLogin = false;
               Debug.Log("Facebook Account Login failed" + OnPlayfabFacebookAuthFailed.Error);
               Debug.Log("No facebook nor mobile account" + OnPlayfabFacebookAuthFailed.Error);
               switch (OnPlayfabFacebookAuthFailed.Error)
               {
                   case PlayFabErrorCode.AccountNotFound:
                       MobileLogin(true, true, false);
                       break;
                   default:
                       DisplayError(ErrorCode.errorLoginIn, OnPlayfabFacebookAuthFailed.Error.ToString());
                       break;
               }
           });
    }
    void LinkFacebookAccount()
    {
        PlayFabClientAPI.LinkFacebookAccount(new LinkFacebookAccountRequest { AccessToken = AccessToken.CurrentAccessToken.TokenString },
        Result =>
        {
            Debug.Log("Facebook Account Link Completed");
            EndLogin();

        }, Error =>
        {
            Debug.Log("Facebook Account Link failed " + Error.Error);
            switch (Error.Error)
            {
                case PlayFabErrorCode.LinkedAccountAlreadyClaimed:
                    UnlinkMobileID(true);
                    break;
                default:
                    DisplayError(ErrorCode.errorFacebookLoginIn, Error.Error.ToString());
                    break;
            }
        });
    }

    private void OnFacebookLoggedIn(ILoginResult result)
    {
        // If result has no errors, it means we have authenticated in Facebook successfully
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
            GameData.data.FacebookLogin = true;
            CheckFacebookPlayfabAccount();
        }
        else
        {
            // If Facebook authentication failed, we stop the cycle with the message
            FacebookLoginChecked = false;
            Debug.Log("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
            DisplayError(ErrorCode.errorFacebookLoginIn, result.Error.ToString());
        }
    }



    #endregion FACEBOOK
    #region Other Methods
    void ShowSignInScreen()
    {
        if (!SignInScreen.activeSelf)
        {
            SignInScreen.SetActive(true);
            SignInScreen.transform.localScale = Vector3.zero;
            LeanTween.scale(SignInScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
    }
    enum ErrorCode
    {
        errorLoginIn,
        errorFacebookLoginIn,
    }

    void DisplayError(ErrorCode errorCode, string ErrorMessage)
    {
        string ErrorStart;
        switch (errorCode)
        {
            default:
                ErrorStart = "<color=#FF4747>Error:</color> ";
                break;
            case ErrorCode.errorLoginIn:
                ErrorStart = "<color=#FF4747>Error loging in:</color> ";
                break;
            case ErrorCode.errorFacebookLoginIn:
                ErrorStart = "<color=#FF4747>Error loging in with facebook:</color> ";
                break;

        }
        LoginErrorMessage.text = ErrorStart + ErrorMessage;
    }

    #endregion Other Methods

    void EndLogin()
    {
        Debug.Log("End Loggin");
    }
}

