using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class. Handle just the PlayfabLogin Method
/// </summary>
public class LoginScript : MonoBehaviour
{
    //Inputs
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;

    //Login Manager
    private GameObject LoginManager;

    private void Start()
    {
        LoginManager = GameObject.Find("LoginManager");
    }

    /// <summary>
    /// PlayFab. Sends a request to PlayFab with the login values (Email, Password).
    /// </summary>
    public void LoginButton()
    {
        if (LoginManager.TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            
            loginPlayFab.Login(EmailInput.text, PasswordInput.text);

        }
        //TODO: Add ELSE which will do the login without playfab
    }
}
