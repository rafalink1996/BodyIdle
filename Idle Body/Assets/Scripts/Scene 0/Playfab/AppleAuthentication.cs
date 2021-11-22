using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using System.Text;
using AppleAuth.Interfaces;

public class AppleAuthentication : MonoBehaviour
{
    private IAppleAuthManager appleAuthManager;
    public string IToken;
    public bool isLoggedInwithApple;

    void Start()
    {
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native response
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer this.appleAuthManager = new AppleAuthManager(deserializer);
            this.appleAuthManager = new AppleAuthManager(deserializer);
        }
    }

    void Update()
    {
        // Updates the AppleAuthManager instance to execute // pending callbacks inside Unity's execution loop if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    void QuickAppleLogin()
    {
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

    void ApplesSignin()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.None);
        this.appleAuthManager.LoginWithAppleId(loginArgs,
              credential =>
              {
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
                  }
              },
        error =>
        {
            // Something went wrong
            var authorizationErrorCode = error.GetAuthorizationErrorCode();
        });
    }


    void CheckCredentialStatus(string userId)
    {
        this.appleAuthManager.GetCredentialState(userId,
state => {
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
error => {
    // Something went wrong
});

    }
}
