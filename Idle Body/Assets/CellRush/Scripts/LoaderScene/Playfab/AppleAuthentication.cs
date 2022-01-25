using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using System.Text;
using AppleAuth.Interfaces;
using TMPro;

public class AppleAuthentication : MonoBehaviour
{
    [SerializeField] private IAppleAuthManager appleAuthManager;
    public string IToken;
    public string userId;
    public bool isLoggedInwithApple;
    

    [Header("Debug")]
    [SerializeField] TextMeshProUGUI ErrorObject;


    void Start()
    {
        ErrorObject.text = "debug";
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            ErrorObject.text = "platform supported for apple login";
            // Creates a default JSON deserializer, to transform JSON Native response
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer this.appleAuthManager = new AppleAuthManager(deserializer);
            this.appleAuthManager = new AppleAuthManager(deserializer);

        }
        else
        {
            ErrorObject.text = "Error: platfrom not supported";
        }
    }

    void Update()
    {
        // Updates the AppleAuthManager instance to execute // pending callbacks inside Unity's execution loop
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    void QuickAppleLogin()
    {
        if (appleAuthManager == null)
        {
            ErrorObject.text = "Error: AppleAuthIsNull";
            PlayfabNoEmailLogin.instance.ShowSignInScreen();
            PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
            return;

        }

        var quickLoginArgs = new AppleAuthQuickLoginArgs();
        this.appleAuthManager.QuickLogin(quickLoginArgs,
              credential =>
              {
                  isLoggedInwithApple = true;
                  // Received a valid credential!
                  // Try casting to IAppleIDCredential or IPasswordCredential
                  // Previous Apple sign in credential
                  var appleIdCredential = credential as IAppleIDCredential;
                  // Saved Keychain credential (read about Keychain Items)
                  var passwordCredential = credential as IPasswordCredential;

                  if (appleIdCredential != null)
                  {
                      var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken,
                                  0, appleIdCredential.IdentityToken.Length);
                      IToken = identityToken;

                  }
              },
        error =>
        {
            // Quick login failed. The user has never used Sign in With Apple on you
        });

    }

    public void ApplesSignin(PlayfabNoEmailLogin loginCallback)
    {
        Debug.Log("Signing in");
        ErrorObject.text = "signing in";
        if (appleAuthManager == null)
        {
            Debug.Log("Error: AppleAuthIsNul");
            ErrorObject.text = "Error: AppleAuthIsNull";
            PlayfabNoEmailLogin.instance.ShowSignInScreen();
            PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
            return;

        }

        var loginArgs = new AppleAuthLoginArgs(LoginOptions.None);
        this.appleAuthManager.LoginWithAppleId(loginArgs,
              credential =>
              {
                  ErrorObject.text = "success login apple";
                  isLoggedInwithApple = true;
                  // Obtained credential, cast it to IAppleIDCredential
                  var appleIdCredential = credential as IAppleIDCredential;
                  if (appleIdCredential != null)
                  {
                      // Apple User ID
                      // You should save the user ID somewhere in the device 
                      var userId = appleIdCredential.User;
                      // Email (Received ONLY in the first login)
                      var email = appleIdCredential.Email;
                      // Full name (Received ONLY in the first login)
                      var fullName = appleIdCredential.FullName;
                      // Identity token
                      var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken,
                              0, appleIdCredential.IdentityToken.Length);


                      IToken = identityToken;
                      // Authorization code
                      var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode,
                              0, appleIdCredential.AuthorizationCode.Length);
                      // And now you have all the information to create/login a user in yo
                      loginCallback.CheckPlayfabAppleAccount(identityToken, userId);
                  }
              },
        error =>
        {
            // Something went wrong
            ErrorObject.text = "Error: Login Failed";
            var authorizationErrorCode = error.GetAuthorizationErrorCode();
            PlayfabNoEmailLogin.instance.ShowSignInScreen();
            PopupManager.instance.ShowPopUp(PopupManager.PopUp.LoginError);
        });

    }


    void CheckCredentialStatus(string userId)
    {
        this.appleAuthManager.GetCredentialState(userId,
state =>
{
    switch (state)
    {
        case CredentialState.Authorized:
            // User ID is still valid. Login the user.
            break;
        case CredentialState.Revoked:
            // User ID was revoked. Go to login screen.
            break;
        case CredentialState.NotFound:
            // User ID was not found. Go to login screen.
            break;
    }
},
error =>
{
    // Something went wrong
});

    }
}
