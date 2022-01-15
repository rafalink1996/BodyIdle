using UnityEngine;
using UnityEngine.iOS;
using System.Collections.Generic;
using System;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using LoginResult = PlayFab.ClientModels.LoginResult;


public class PlayfabNoEmailLogin : MonoBehaviour
{
    [Header("Instance")]
    public static PlayfabNoEmailLogin instance;

    [Header("Screens")]
    [SerializeField] GameObject SignInScreen;
    [Space(5)]

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI LoginErrorMessage;
    [SerializeField] TextMeshProUGUI welcomeMessage;
    [Space(5)]
    [SerializeField] TextMeshProUGUI ConfirmCreateAccountBody1;
    [SerializeField] TextMeshProUGUI ConfirmCreateAccountBody2;

    [Header("Buttons")]
    [SerializeField] GameObject ConfirmFacebookAccountButton;
    [SerializeField] GameObject ConfirmGoogleAccountButton;
    [SerializeField] GameObject ConfirmAppleAccountButton;

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
    [Space(5)]
    [SerializeField] bool AppleLogin;
    [SerializeField] bool hasAppleLinked;
    [SerializeField] bool appleLoginChecked;

    [Header("Refrences")]
    [SerializeField] AppleAuthentication appleAuth;
    [SerializeField] GameObject loadingCircle;

    [Header("Retry")]
    int loginRetryCount = 0;
    [Header("Other")]
    [SerializeField] string username;



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
        Debug.Log("Start");
        if (Application.platform == RuntimePlatform.Android)
        {
            ConfirmAppleAccountButton.SetActive(false);
            ConfirmGoogleAccountButton.SetActive(true);

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ConfirmAppleAccountButton.SetActive(true);
            ConfirmGoogleAccountButton.SetActive(false);

        }
        else
        {
            //ConfirmAppleAccountButton.SetActive(false);
            //ConfirmGoogleAccountButton.SetActive(false);

        }
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
            if (result.AccountInfo.AppleAccountInfo != null)
            {
                Debug.Log("Has Apple Linked");
                hasAppleLinked = true;
            }
            else
            {
                Debug.Log("Apple not Linked");
                hasAppleLinked = false;
            }

            // check if user has pending links
            if (Application.platform == RuntimePlatform.IPhonePlayer || SystemInfo.deviceModel.StartsWith("IPad"))
            {
                if (hasFacebookLinked && hasAppleLinked)
                {
                    Debug.Log("Everything linked");
                    EndLogin();
                }
                else
                {
                    ShowSignInScreen();
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (hasGoogleLinked && hasFacebookLinked)
                {
                    Debug.Log("Everything linked");
                    EndLogin();
                }
                else
                {
                    ShowSignInScreen();
                }
            }
            else
            {
                if (hasFacebookLinked)
                {
                    Debug.Log("Everything linked");
                    EndLogin();
                }
                else
                {
                    ShowSignInScreen();
                }

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
        else if (Application.platform == RuntimePlatform.IPhonePlayer || SystemInfo.deviceModel.StartsWith("IPad")) // tratar de poner ipad
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
        else // Editor: Remove this in build todo
        {
            if (Application.isEditor)
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
            else
            {
                Manager.instance.gameloader.LoadGameScene();
            }
             
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
            if (Application.isEditor)
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
        if (Application.platform == RuntimePlatform.IPhonePlayer || SystemInfo.deviceModel.StartsWith("IPad"))
        {
            var unlinkIOSRequest = new UnlinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkIOSDeviceID(unlinkIOSRequest, success =>
            {
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
            }, failure =>
            {
                DisplayError(ErrorCode.errorFacebookLoginIn, failure.Error.ToString());
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var unlinkAndroidRequest = new UnlinkAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkAndroidDeviceID(unlinkAndroidRequest, sucess =>
            {
                PlayFabClientAPI.ForgetAllCredentials();
                mobileLogin = false;

            }, failure =>
            {
                if (facebook)
                {
                    PlayfabLoginWithFacebook();
                }
                else
                {
                    //Login con google
                }
            });

        }
    }

    void LinkMobileID()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID(), ForceLink = true};
            PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
            {
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                switch (failed.Error)
                {
                    case PlayFabErrorCode.DeviceAlreadyLinked:

                        break;
                    default:
                        EndLogin();
                        break;
                }
                Debug.Log("failed to link Mobile ID");
                });
            }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var linkAndroidRequest = new LinkAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), ForceLink = true };
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
            if (Application.isEditor)
            {
                var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID() , ForceLink = true};
                PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
                {
                    Debug.Log("Linked Mobile ID");
                    EndLogin();

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
    }



    #endregion API Methods
    #region OnClick Methods
    public void OnClickSignInWithFacebook()
    {
        LoginWithFacebook();
        loadingCircle.SetActive(true);
        HideSignInScreen();
    }



    public void OnClickSignInWithGoogle()
    {

    }

    public void OnClickSignInWithApple()
    {
        SignInWithApple();
        loadingCircle.SetActive(true);
        HideSignInScreen();
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

    public void OnClickFirstSkip()
    {
      PopupManager.instance.ShowPopUp(PopupManager.PopUp.Disclaimer);
    }

    public void OnClickBackToLoginScreen()
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
                return;
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
        PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
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
                   default:
                       ShowSignInScreen();
                       PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
                       DisplayError(ErrorCode.errorLoginIn, OnPlayfabFacebookAuthFailed.Error.ToString());
                       break;
               }
           });
    }
    void LinkFacebookAccount()
    {
        PlayFabClientAPI.LinkFacebookAccount(new LinkFacebookAccountRequest { AccessToken = AccessToken.CurrentAccessToken.TokenString},
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
                    PlayFabClientAPI.ForgetAllCredentials();
                    PlayfabLoginWithFacebook();
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
            if (result.Cancelled)
            {
                Debug.Log("acess token null");
                ShowSignInScreen();
                PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
                DisplayError(ErrorCode.errorFacebookLoginIn, "User cancelled login, access token is null");
                return;

            }
            if (AccessToken.CurrentAccessToken == null)
            {
                Debug.Log("acess token null");
                ShowSignInScreen();
                PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
                DisplayError(ErrorCode.errorFacebookLoginIn, "User cancelled login, access token is null");
                return;
            }

            Debug.Log("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
            if (result.ResultDictionary.ContainsKey("first_name"))
            {
                username = "" + result.ResultDictionary["first_name"];
            }
            GameData.data.FacebookLogin = true;
            CheckFacebookPlayfabAccount();
        }
        else
        {
            // If Facebook authentication failed, we stop the cycle with the message
            FacebookLoginChecked = false;
            Debug.Log("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult);
            ShowSignInScreen();
            PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
            DisplayError(ErrorCode.errorFacebookLoginIn, result.Error.ToString());
        }
    }



    #endregion FACEBOOK
    #region Apple
    public void SignInWithApple()
    {
        appleAuth.ApplesSignin(this);

    }

    public void CheckPlayfabAppleAccount(string Itoken, string username)
    {
        if (mobileLogin && !hasAppleLinked)
        {
            LinkAppleAccount(Itoken, username);
        }
        else if (FacebookLogin)
        {
            LinkAppleAccount(Itoken, username);
        }
        else if (GoogleLogin)
        {
            LinkAppleAccount(Itoken, username);
        }
        else
        {
            PlayfabAppleSignIn(Itoken, username, true);
        }
    }
    void LinkAppleAccount(string Itoken, string username)
    {
        var request = new LinkAppleRequest { IdentityToken = Itoken };
        PlayFabClientAPI.LinkApple(request, sucess =>
        {
            this.username = username;
            EndLogin();

        }, Failed =>
        {
            DisplayError(ErrorCode.errorAppleLogin, " Message: " + Failed.ErrorMessage + " Error: " + Failed.Error + " details: " + Failed.ErrorDetails);
            ShowSignInScreen();
            PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
        });
    }
    void PlayfabAppleSignIn(string Itoken, string username, bool createAccount)
    {
        var request = new LoginWithAppleRequest { IdentityToken = Itoken, CreateAccount = createAccount };
        PlayFabClientAPI.LoginWithApple(request,
            Complete =>
            {
                this.username = username;
                EndLogin();
                Debug.Log("Apple Playfab login success: " + "\n");
            },
            Failed =>
            {
                DisplayError(ErrorCode.errorAppleLogin, " Message: " + Failed.ErrorMessage + " Error: " + Failed.Error + " details: " + Failed.ErrorDetails);
                ShowSignInScreen();
                PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
            });
    }
    #endregion Apple
    #region Other Methods
    public void ShowSignInScreen()
    {
        if (!SignInScreen.activeSelf)
        {
            Debug.Log("activating screen");
            SignInScreen.SetActive(true);
            SignInScreen.transform.localScale = Vector3.zero;
            LeanTween.scale(SignInScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
        else
        {
            Debug.Log("Screen is active");
        }
    }

    void HideSignInScreen()
    {
        if (SignInScreen.activeSelf)
        {
            SignInScreen.SetActive(false);
            LeanTween.scale(SignInScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>
            {
                SignInScreen.SetActive(false);
            });
        }
    }
    enum ErrorCode
    {
        errorLoginIn,
        errorFacebookLoginIn,
        errorAppleLogin,
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
            case ErrorCode.errorAppleLogin:
                ErrorStart = "Error loging in with Apple:";
                break;

        }
        LoginErrorMessage.text = ErrorStart + ErrorMessage;
    }

    #endregion Other Methods

    void EndLogin()
    {
        //Revisar data
        if (string.IsNullOrEmpty(username))
        {
            welcomeMessage.text = "welcome " + username;
        }
        else
        {
            welcomeMessage.text = "welcome";
        }
        PopupManager.instance.ShowPopUp(PopupManager.PopUp.welcome);
        Debug.Log("End Loggin");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
}

