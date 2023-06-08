using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetPasswordScript : MonoBehaviour
{

    //Info UI Text
    public TMP_Text MessageText;

    //Inputs
    public TMP_InputField EmailInput;

    //UI Manager
    public PanelManager PanelManager;

    //Login Manager
    private GameObject LoginManager;

    private void Start()
    {
        LoginManager = GameObject.Find("LoginManager");
    }

    /*Reset Password Functions*/




    /// <summary>
    /// PlayFab. Sends a request to PlayFab to reset the password.
    /// </summary>
    public void ResetPassword()
    {
        if (LoginManager.TryGetComponent(out LoginPlayFab loginPlayFab))
        {
            loginPlayFab.OnReset(EmailInput.text);
        }//else(Log
    }

}