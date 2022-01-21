using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.AuthenticationModels;
using TMPro;
using System;

#if UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif



public class PlayFabLogin : MonoBehaviour
{

    [Header("User Register Info")]
    [SerializeField] private string UserEmail;
    [SerializeField] private string UserPassword;
    [SerializeField] private string UserConfirmPassword;
    [SerializeField] private string UserName;

    [Header("User Login Info")]
    [SerializeField] private string UserNameOrEmail;

    [Space(10)]
    [Header("Status Bools")]
    bool Login = true;
    bool mobileLogin = false;
    bool transitioningRegisterLogin = false;

    [Space(10)]
    [Header("Objects")]
    [Header("screens")]
    [SerializeField] GameObject SignInScreenObject;
    [SerializeField] GameObject ConfirmAccountScreenObject;
    [SerializeField] GameObject ContinueAsScreen;
    [SerializeField] GameObject AskQuestionScreen;
    [SerializeField] GameObject NoEmailAssosiatedScreen;
    [SerializeField] GameObject UserNotconfirmedScreen;
    [SerializeField] GameObject ConfirmInfoScreen;

    [Header("Buttons")]
    [SerializeField] GameObject loginRegisterButton;
    [SerializeField] GameObject loginObject;
    [SerializeField] GameObject RegisterObject;

    [Space(10)]
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI LoginRegisterButtonText;
    [SerializeField] TextMeshProUGUI AccountCuestionText;
    [Space(5)]
    [SerializeField] TextMeshProUGUI continueAsUsernameText;
    [Space(5)]
    [SerializeField] TextMeshProUGUI loginErrorText;
    [SerializeField] TextMeshProUGUI RegisterErrorText;
    [Space(5)]
    [SerializeField] TextMeshProUGUI ConfirmUsername;
    [SerializeField] TextMeshProUGUI ConfirmEmail;

    [Space(10)]
    [Header("inputfields")]
    [SerializeField] TMP_InputField UsernameOrEmailInputField;
    [SerializeField] TMP_InputField UsernameInputField;
    [SerializeField] TMP_InputField PasswordRegisterInputField;
    [SerializeField] TMP_InputField PasswordLoginInputField;
    [SerializeField] TMP_InputField ConfirmPasswordInputField;
    [SerializeField] TMP_InputField EmailInputField;

    [Space(10)]
    [Header("References")]

    Manager manager;

    public void GetReferences()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<Manager>();
        }
    }

    void testRemove()
    {
        var remove = new RemoveContactEmailRequest { };
        PlayFabClientAPI.RemoveContactEmail(remove, result =>
        {
            Debug.Log("Removed contact info");
        }, filed =>
        {
            Debug.Log("Failed to remove contact info");
        });

    }
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
        if (PlayerPrefs.HasKey("PASSWORD"))
        {
            if (PlayerPrefs.HasKey("USERNAME"))
            {
                UserName = PlayerPrefs.GetString("USERNAME");
                UserNameOrEmail = PlayerPrefs.GetString("USERNAME");
                UserPassword = PlayerPrefs.GetString("PASSWORD");
                var usernameRequest = new LoginWithPlayFabRequest { Username = UserName, Password = UserPassword };
                PlayFabClientAPI.LoginWithPlayFab(usernameRequest, result =>
                {
                    Debug.Log("Username login was a success");
                    GetPlayerData(result.PlayFabId);

                }, error =>
                {
                    InitialMobileLogin();
                    Debug.Log("email login error");
                });
            }
            else if (PlayerPrefs.HasKey("EMAIL"))
            {
                UserEmail = PlayerPrefs.GetString("EMAIL");
                UserNameOrEmail = PlayerPrefs.GetString("EMAIL");
                UserPassword = PlayerPrefs.GetString("PASSWORD");
                var mailRequest = new LoginWithEmailAddressRequest { Email = UserEmail, Password = UserPassword };
                PlayFabClientAPI.LoginWithEmailAddress(mailRequest, result =>
                {
                    Debug.Log("Email login was a success");
                    GetPlayerData(result.PlayFabId);


                }, error =>
                {
                    InitialMobileLogin();
                    Debug.Log("email login error");
                });
            }
            else
            {
                InitialMobileLogin();
            }
        }
        else
        {
            InitialMobileLogin();
        }

        void InitialMobileLogin()
        {
            Debug.Log("Dont have saved Credentials");
            if (Application.platform == RuntimePlatform.Android)
            {
                var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, result =>
                {

                    Debug.Log("Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserInfo(result.PlayFabId);

                }, PlayFabError =>
                {
                    ShowQuestionSceen();
                    Debug.Log("Mobile login error");
                });
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, result =>
                {

                    Debug.Log("Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserInfo(result.PlayFabId);

                }, PlayFabError =>
                {
                    ShowQuestionSceen();
                    Debug.Log("Mobile login error");
                });
            }
            else
            {
                var requestEditor = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = false };
                PlayFabClientAPI.LoginWithIOSDeviceID(requestEditor, result =>
                {

                    Debug.Log("Mobile login was a success");
                    GameData.data.PlayfabLogin = true;
                    mobileLogin = true;
                    CheckUserInfo(result.PlayFabId);

                }, PlayFabError =>
                {
                    ShowQuestionSceen();
                    Debug.Log("Mobile login error");
                });
            }
        }

        void CheckUserInfo(string playfabID)
        {
            var info = new GetAccountInfoRequest { PlayFabId = playfabID };
            PlayFabClientAPI.GetAccountInfo(info, result =>
            {
                if (result.AccountInfo.Username != null)
                {
                    var infoRequest = new GetPlayerProfileRequest { PlayFabId = playfabID, ProfileConstraints = new PlayerProfileViewConstraints { ShowContactEmailAddresses = true } };
                    PlayFabClientAPI.GetPlayerProfile(infoRequest, Profileresult =>
                    {
                        if (Profileresult.PlayerProfile.ContactEmailAddresses != null)
                        {
                            if (Profileresult.PlayerProfile.ContactEmailAddresses.Count != 0)
                            {
                                if (Profileresult.PlayerProfile.ContactEmailAddresses[0].VerificationStatus == EmailVerificationStatus.Confirmed)
                                {
                                    GameData.data.PlayfabLogin = true;
                                    ShowContinueAs(result.AccountInfo.Username);
                                }
                                else
                                {
                                    UserEmail = Profileresult.PlayerProfile.ContactEmailAddresses[0].EmailAddress;
                                    Debug.Log("User email not confirmed");
                                    ShowEmailSentScreen();
                                    //ShowConfirmScreen(true); TODO
                                }
                            }
                            else
                            {
                                NoContactEmail(result.AccountInfo.PlayFabId);
                            }
                        }
                        else
                        {
                            NoContactEmail(result.AccountInfo.PlayFabId);
                        }
                    }
                    , Infofailure =>
                    {
                        ShowSignInScreen();
                        Debug.Log(Infofailure.GenerateErrorReport());
                        Debug.Log("Failed to get player data");
                    });
                }
                else
                {
                    ShowQuestionSceen();
                }
            }, failure => { Debug.Log("Failed to get user account info"); });
        }
    }


    #region InputMethods

    public void OnClickLogin()
    {
        PlayfabLogin();
    }
    public void OnClickRegister()
    {
        if (!string.IsNullOrEmpty(UserPassword))
        {
            if (CheckIfpasswordsMatch())
            {
                if (ConfirmEmail != null)
                {
                    ConfirmEmail.text = UserEmail;
                }
                if (ConfirmUsername != null)
                {
                    ConfirmUsername.text = UserName;
                }
                CloseSignInScreen();
                ShowConfirmInfoScreen();
            }
            else
            {
                RegisterErrorText.text = "Passwords don't match";
            }
        }
        else
        {
            RegisterErrorText.text = "Password field is required";
        }
       
        
    }

    public void OnClickConfirm()
    {
        
        CloseConfitmInfoScreen();
        PlayfabRegister();
    }

    bool CheckIfpasswordsMatch()
    {
        if (string.Equals(UserPassword, UserConfirmPassword))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void testLogin()
    {
        var LoginWithEmailAddressRequest = new LoginWithEmailAddressRequest { Email = UserEmail, Password = "......(...................,,..........................,}.....g............................)........." };
        PlayFabClientAPI.LoginWithEmailAddress(LoginWithEmailAddressRequest, result =>
        {
            PlayFabClientAPI.ForgetAllCredentials();
        }, failed =>
        {
            switch (failed.Error)
            {
                case PlayFabErrorCode.AccountNotFound:
                    Debug.Log("Account not found");
                    break;
                default:
                    Debug.Log(failed.Error);
                    Debug.Log(failed.GenerateErrorReport());
                    break;
            }
        });
    }

    public void OnClickSkipLogin()
    {
        CloseContinueAs();
        if (GameData.data.PlayfabLogin)
        {
            EndLogin();
        }
        else
        {
            MobileLogin();
        }
    }
    public void OnClickContinueAs(bool Continue)
    {
        if (Continue)
        {
            CloseContinueAs();
            EndLogin();
        }
        else
        {
            CloseContinueAs();
            PlayFabClientAPI.ForgetAllCredentials();
            ShowSignInScreen();
        }
    }
    public void OnClickSignIn()
    {
        CloseConfitmInfoScreen();
        CloseEmailSentScreen();
        CloseQuestionScreen();
        ShowSignInScreen();
    }

    public void OnValueChangeEmail(string emailInput)
    {
        UserEmail = emailInput;
    }
    public void OnValueChangeUsername(string usernameInput)
    {
        UserName = usernameInput;
    }
    public void OnValueChangePassword(string passwordInput)
    {
        UserPassword = passwordInput;
    }
    public void OnValueChangeConfirmPassword(string confirmPasswordInput)
    {
        UserConfirmPassword = confirmPasswordInput;
    }
    public void OnValueChangeUsernameOrEmail(string usernameOrEmailInput)
    {
        UserNameOrEmail = usernameOrEmailInput;
    }

    public void OnClickToggleLoginRegister()
    {
        if (!transitioningRegisterLogin)
        {
            clearInputFields();
            transitioningRegisterLogin = true;
            if (Login)
            {
                LeanTween.scale(loginRegisterButton, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                LeanTween.scale(RegisterObject, Vector3.zero, 0);
                LTDescr l = LeanTween.scale(loginObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(NextTransition);
                void NextTransition()
                {

                    RegisterObject.SetActive(true);
                    loginObject.SetActive(false);
                    LoginRegisterButtonText.text = "Login";
                    AccountCuestionText.text = "Already Have an account?";
                    LeanTween.scale(loginRegisterButton, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                    LTDescr l2 = LeanTween.scale(RegisterObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                    l2.setOnComplete(finsihTransition);
                    void finsihTransition()
                    {
                        transitioningRegisterLogin = false;
                        Login = false;
                    }
                }
            }
            else
            {
                LeanTween.scale(loginRegisterButton, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                LeanTween.scale(loginObject, Vector3.zero, 0);
                LTDescr l = LeanTween.scale(RegisterObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(FinishTransition);
                void FinishTransition()
                {

                    RegisterObject.SetActive(false);
                    loginObject.SetActive(true);
                    LoginRegisterButtonText.text = "Register";
                    AccountCuestionText.text = "Don't have an account?";
                    LeanTween.scale(loginRegisterButton, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                    LTDescr l2 = LeanTween.scale(loginObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                    l2.setOnComplete(finsihTransition);
                    void finsihTransition()
                    {
                        transitioningRegisterLogin = false;
                        Login = true;
                    }
                }
            }
        }

    }
    #endregion InputMethods

    #region API Methods
    void MobileLogin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, result =>
            {

                Debug.Log("login was a success");
                GameData.data.PlayfabLogin = true;
                EndLogin();

            }, PlayFabError =>
            {
                GameData.data.PlayfabLogin = false;
                EndLogin();
            });
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, result =>
            {

                Debug.Log("login was a success");
                GameData.data.PlayfabLogin = true;
                EndLogin();

            }, PlayFabError =>
            {
                GameData.data.PlayfabLogin = false;
                EndLogin();
            });
        }
        else
        {
            var requestEditor = new LoginWithIOSDeviceIDRequest { DeviceId = GetMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestEditor, result =>
            {

                Debug.Log("login was a success");
                GameData.data.PlayfabLogin = true;
                EndLogin();

            }, PlayFabError =>
            {
                GameData.data.PlayfabLogin = false;
                EndLogin();
            });
        }
    }

    void PlayfabLogin()
    {
        if (UserNameOrEmail.Contains("@"))
        {
            Debug.Log("try login In with email");
            UserEmail = UserNameOrEmail;
            var mailRequest = new LoginWithEmailAddressRequest { Email = UserEmail, Password = UserPassword };
            PlayFabClientAPI.LoginWithEmailAddress(mailRequest, result =>
            {
                GameData.data.PlayfabLogin = true;
                PlayerPrefs.SetString("EMAIL", UserEmail);
                PlayerPrefs.SetString("PASSWORD", UserPassword);
                GetPlayerData(result.PlayFabId);
            }, OnLoginError);
        }
        else
        {
            Debug.Log("try login In with username");
            UserName = UserNameOrEmail;
            var usernameRequest = new LoginWithPlayFabRequest { Username = UserName, Password = UserPassword };
            PlayFabClientAPI.LoginWithPlayFab(usernameRequest, result =>
            {
                GameData.data.PlayfabLogin = true;
                PlayerPrefs.SetString("USERNAME", UserName);
                PlayerPrefs.SetString("PASSWORD", UserPassword);
                GetPlayerData(result.PlayFabId);

            }, OnLoginError);
        }
    }
    void GetPlayerData(string playfabID)
    {
        var infoRequest = new GetPlayerProfileRequest { PlayFabId = playfabID, ProfileConstraints = new PlayerProfileViewConstraints { ShowContactEmailAddresses = true } };
        PlayFabClientAPI.GetPlayerProfile(infoRequest, Profileresult =>
        {
            if (Profileresult.PlayerProfile.ContactEmailAddresses != null)
            {
                if (Profileresult.PlayerProfile.ContactEmailAddresses.Count != 0)
                {
                    if (Profileresult.PlayerProfile.ContactEmailAddresses[0].VerificationStatus == EmailVerificationStatus.Confirmed)
                    {
                        GameData.data.PlayfabLogin = true;
                        if (!mobileLogin)
                            LinkMobileID();
                        else
                            UnLinkMobileID();
                    }
                    else
                    {
                        //testRemove();
                        CloseSignInScreen();
                        UserEmail = Profileresult.PlayerProfile.ContactEmailAddresses[0].EmailAddress;
                        Debug.Log("User email not confirmed");
                        ShowEmailSentScreen();
                        //ShowConfirmScreen(true);
                    }
                }
                else
                {

                    NoContactEmail(playfabID);
                }
            }
            else
            {
                NoContactEmail(playfabID);
            }
        }
        , failure =>
        {
            PlayFabClientAPI.ForgetAllCredentials();
            ShowSignInScreen();
            Debug.Log(failure.GenerateErrorReport());
            Debug.Log("Failed to get player data");
        });


    }
    void NoContactEmail(string playfabID, string email = null)
    {
        Debug.Log("User doesnt have contact email");
        if (string.IsNullOrEmpty(email))
        {
            var infoRequest2 = new GetAccountInfoRequest { PlayFabId = playfabID };
            PlayFabClientAPI.GetAccountInfo(infoRequest2, InfoResult =>
            {
                if (InfoResult.AccountInfo.PrivateInfo.Email != null)
                {
                    AddOrUpdateContactEmail(InfoResult.AccountInfo.PrivateInfo.Email);
                }
                else
                {
                    ShowNoEmailAssosiated();
                    Debug.Log("There is no email");
                }
            }, InfoError => { });
        }
        else
        {
            AddOrUpdateContactEmail(email);
        }
    }

    void PlayfabRegister()
    {

        var registerRequest = new RegisterPlayFabUserRequest { Username = UserName, Email = UserEmail, Password = UserPassword };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, result =>
        {
            AddOrUpdateContactEmail(UserEmail);
        }, PlayFabError =>
        {
            Debug.Log(PlayFabError.Error);

        });
    }

    void AddOrUpdateContactEmail(string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest { EmailAddress = emailAddress };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            ShowConfirmScreen();
            Debug.Log("The player's account has been updated with a contact email");
        }, failure =>
        {
            Debug.Log("Error adding or updating contact email");
        });
    }
    void UnLinkMobileID(bool linkNew = false)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var unlinkIOSRequest = new UnlinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkIOSDeviceID(unlinkIOSRequest, result =>
            {
                if (linkNew)
                {
                    LinkMobileID();
                }
                Debug.Log("Unlinked Mobile ID");
            }, failed => { Debug.Log("failed to unlink Mobile ID"); });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var unlinkAndroidRequest = new UnlinkAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkAndroidDeviceID(unlinkAndroidRequest, result =>
            {
                if (linkNew)
                {
                    LinkMobileID();
                }
                Debug.Log("Unlinked Mobile ID");
            }, failed => { Debug.Log("failed to unlink Mobile ID"); });
        }
        else
        {
            var unlinkIOSRequest = new UnlinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.UnlinkIOSDeviceID(unlinkIOSRequest, result =>
            {
                if (linkNew)
                {
                    LinkMobileID();
                }
                Debug.Log("Unlinked Mobile ID");
            }, failed => { Debug.Log("failed to unlink Mobile ID" + failed.Error); });
        }
    }

    void LinkMobileID()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
            {
                CloseSignInScreen();
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                CloseSignInScreen();
                EndLogin();
                Debug.Log("failed to link Mobile ID");
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var linkAndroidRequest = new LinkAndroidDeviceIDRequest { AndroidDeviceId = GetMobileID() };
            PlayFabClientAPI.LinkAndroidDeviceID(linkAndroidRequest, result =>
            {

                CloseSignInScreen();
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                CloseSignInScreen();
                EndLogin();
                Debug.Log("failed to link Mobile ID");
            });
        }
        else
        {
            var linkIOSRequest = new LinkIOSDeviceIDRequest { DeviceId = GetMobileID() };
            PlayFabClientAPI.LinkIOSDeviceID(linkIOSRequest, result =>
            {

                CloseSignInScreen();
                EndLogin();
                Debug.Log("Linked Mobile ID");
            }, failed =>
            {
                CloseSignInScreen();
                EndLogin();
                Debug.Log("failed to link Mobile ID");
            });
        }
    }



    #endregion API Methods

    #region SuccessMethods
    private void OnLoginSuccess(LoginResult result)
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("EMAIL")))
        {
            if (!string.IsNullOrEmpty(UserEmail))
                PlayerPrefs.SetString("EMAIL", UserEmail);
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("PASSWORD")))
        {
            if (!string.IsNullOrEmpty(UserPassword))
                PlayerPrefs.SetString("PASSWORD", UserPassword);
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("USERNAME")))
        {
            if (!string.IsNullOrEmpty(UserPassword))
                PlayerPrefs.SetString("USERNAME", UserName);
        }
        Debug.Log("login was a success");
        GameData.data.PlayfabLogin = true;
        EndLogin();
    }
    #endregion SuccessMethods

    #region FailureMethods
    private void OnLoginError(PlayFabError error)
    {
        Debug.Log("Error Login in");
        Debug.Log(error.HttpCode);
        DisplayError(error, true);

    }
    #endregion FailureMethods

    #region GetMethods
    public static string GetMobileID()
    {
        string MobileId;
        if ((Application.platform == RuntimePlatform.IPhonePlayer) || SystemInfo.deviceModel.Contains("iPad"))
        {
#if UNITY_IOS
string DeviceID = Device.vendorIdentifier;
            MobileId = DeviceID;
#endif
            MobileId = null;

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
    public void GetPlayfabTime()
    {
        if (!GameData.data.PlayfabLogin)
        {
            return;
        }

        PlayFabClientAPI.GetTime(new GetTimeRequest(), result => { }, error => { });
    }

#endregion GetMethods
#region OtherMethods

    void DisplayError(PlayFabError error, bool login)
    {
        string errorString = "";
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                errorString = "Account Not Found";
                break;
            case PlayFabErrorCode.DeviceAlreadyLinked:
                errorString = "Device Already Linked";
                break;
            case PlayFabErrorCode.DeviceNotLinked:
                errorString = "Device Not Linked";
                break;
            case PlayFabErrorCode.UserisNotValid:
                errorString = "User is not valid";
                break;
            case PlayFabErrorCode.InvalidParams:
                errorString = "invalid username or password";
                break;
            default:
                //errorString = error.GenerateErrorReport();
                errorString = error.Error.ToString();
                break;
        }
        if (login)
        {
            if (loginErrorText != null)
            {
                loginErrorText.gameObject.SetActive(true);
                loginErrorText.text = "Error: " + errorString;
            }
        }
        else
        {
            if (RegisterErrorText != null)
            {
                RegisterErrorText.gameObject.SetActive(true);
                RegisterErrorText.text = "Error: " + errorString;
            }
        }

    }

    void ShowSignInScreen()
    {
        if (!SignInScreenObject.activeSelf)
        {
            SignInScreenObject.transform.localScale = Vector3.zero;
            SignInScreenObject.SetActive(true);
            LeanTween.scale(SignInScreenObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
    }
    void CloseSignInScreen()
    {
        if (SignInScreenObject != null)
        {
            if (SignInScreenObject.activeSelf)
            {
                LTDescr l = LeanTween.scale(SignInScreenObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    SignInScreenObject.SetActive(false);
                }
            }
        }
    }

    void ShowConfirmScreen(bool AccountCreated = false)
    {
        if (!ConfirmAccountScreenObject.activeSelf)
        {
            ConfirmAccountScreenObject.transform.localScale = Vector3.zero;
            ConfirmAccountScreenObject.SetActive(true);
            LeanTween.scale(ConfirmAccountScreenObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
    }

    void EndLogin()
    {
        Debug.Log("Ended login");
        SignInScreenObject.SetActive(false);
        //Manager.instance.CheckData();
    }

    void ShowNoEmailAssosiated()
    {
        if (NoEmailAssosiatedScreen != null)
        {
            if (!NoEmailAssosiatedScreen.activeSelf)
            {
                NoEmailAssosiatedScreen.SetActive(true);
                NoEmailAssosiatedScreen.transform.localScale = Vector3.zero;
                LeanTween.scale(NoEmailAssosiatedScreen, Vector3.one, 1f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        else
        {
            Debug.LogError("Error: no email assosiated screen is null");
        }
    }
    void CloseNoEmailAssosiated()
    {
        if (NoEmailAssosiatedScreen != null)
        {
            if (NoEmailAssosiatedScreen.activeSelf)
            {
                LTDescr l = LeanTween.scale(NoEmailAssosiatedScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    NoEmailAssosiatedScreen.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Error: no email assosiated screen is null");
        }

    }

    void ShowContinueAs(string usernameToShow)
    {
        if (ContinueAsScreen != null)
        {
            if (!ContinueAsScreen.activeSelf)
            {
                if (continueAsUsernameText != null)
                {
                    continueAsUsernameText.text = "Continue as " + usernameToShow + "?";
                }
                ContinueAsScreen.SetActive(true);
                ContinueAsScreen.transform.localScale = Vector3.zero;
                LeanTween.scale(ContinueAsScreen, Vector3.one, 1f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        else
        {
            Debug.LogError("Error: Continue screen is null");
        }
    }
    void CloseContinueAs()
    {
        if (ContinueAsScreen != null)
        {
            if (ContinueAsScreen.activeSelf)
            {
                LTDescr l = LeanTween.scale(ContinueAsScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    ContinueAsScreen.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Error: Continue screen is null");
        }

    }

    void ShowEmailSentScreen()
    {
        if (UserNotconfirmedScreen != null)
        {
            if (!UserNotconfirmedScreen.activeSelf)
            {
                UserNotconfirmedScreen.SetActive(true);
                UserNotconfirmedScreen.transform.localScale = Vector3.zero;
                LeanTween.scale(UserNotconfirmedScreen, Vector3.one, 1f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        else
        {
            Debug.LogError("Error: Continue screen is null");
        }
    }
    void CloseEmailSentScreen()
    {
        if (UserNotconfirmedScreen != null)
        {
            if (UserNotconfirmedScreen.activeSelf)
            {
                LTDescr l = LeanTween.scale(UserNotconfirmedScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    UserNotconfirmedScreen.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Error: Continue screen is null");
        }

    }


    void ShowQuestionSceen()
    {
        if (AskQuestionScreen != null)
        {
            if (!AskQuestionScreen.activeSelf)
            {
                AskQuestionScreen.SetActive(true);
                AskQuestionScreen.transform.localScale = Vector3.zero;
                LeanTween.scale(AskQuestionScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        else
        {
            Debug.LogError("Error: ask question screen is null");
        }
    }
    void CloseQuestionScreen()
    {
        if (AskQuestionScreen != null)
        {
            if (AskQuestionScreen.activeSelf)
            {
                LTDescr l = LeanTween.scale(AskQuestionScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    AskQuestionScreen.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Error: ask question screen is null");
        }

    }

    void ShowConfirmInfoScreen()
    {
        if (ConfirmInfoScreen != null)
        {
            if (!ConfirmInfoScreen.activeSelf)
            {
                ConfirmInfoScreen.SetActive(true);
                ConfirmInfoScreen.transform.localScale = Vector3.zero;
                LeanTween.scale(ConfirmInfoScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
            }
        }
        else
        {
            Debug.LogError("Error: ask question screen is null");
        }
    }
    void CloseConfitmInfoScreen()
    {
        if (ConfirmInfoScreen != null)
        {
            if (ConfirmInfoScreen.activeSelf)
            {
                LTDescr l = LeanTween.scale(ConfirmInfoScreen, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo);
                l.setOnComplete(EndClose);
                void EndClose()
                {
                    ConfirmInfoScreen.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Error: Confirm info screen is null");
        }

    }

    void clearInputFields()
    {
        UsernameOrEmailInputField.text = "";
        UsernameInputField.text = "";
        EmailInputField.text = "";
        PasswordLoginInputField.text = "";
        PasswordRegisterInputField.text = "";
        ConfirmPasswordInputField.text = "";
    }

#endregion OtherMethods



}
