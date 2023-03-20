using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

using TMPro;

using EntityKey = PlayFab.GroupsModels.EntityKey;

using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_Text messageText;
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private GameManager gameManager;


    private EntityKey groupAdminEntity;

    [System.Serializable]
    public class PlayerDataUsername
    {
        public string getPlayerUsername;
    }
    public class PlayerDataId
    {
        public string getPlayerId;
    }
    private void Start()
    {
        gameManager = GameManager.FindInstance();
    }


    //Todo el tema de los paneles
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject ResetPanel;


    public void ChangeRegister()
    {
        RegisterPanel.SetActive(true);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(false);
    }
    public void ChangeLogin()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }
    public void ChangeReset()
    {
        RegisterPanel.SetActive(false);
        ResetPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }



    //Funci�n register
    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password too Short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucess, OnError);
    }
    //Cuando Register Funciona
    void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }  
      


    public void ResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "CB001",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
        LoginPanel.SetActive(true);
        ResetPanel.SetActive(false);
       
    }

    //Funcion Login
    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    //Cuando Login Funciona
    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "logged in!";
        //A�adir miembro
        var AddMem = new ExecuteCloudScriptRequest()
        {
            FunctionName = "addMember",
            FunctionParameter = new
            {
                GroupId = "77569033BA83F38B",
            },
            //GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(AddMem, OnAddMemberSuccess, OnAddMemberFailure);


        //Obtener Nombre
        var GetNa = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoUsername"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetNa, OnUsernameSuccess, OnError);

        //Obtener ID
        var GetID = new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerAccountInfoId"
        };

        PlayFabClientAPI.ExecuteCloudScript(GetID, OnIDSuccess, OnError);
         

        SceneManager.LoadSceneAsync("Lobby");
    }

    //Cuando Obtener ID funciona
    private void OnIDSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {
        string jsonString = result.FunctionResult.ToString();

        PlayerDataId playerDataId = JsonUtility.FromJson<PlayerDataId>(jsonString);

        string IDMaster = playerDataId.getPlayerId;

        Debug.Log("El Master ID es = " + IDMaster);
        gameManager.userID = IDMaster;
    }
    //Cuando a�adir miembtro funciona
    private void OnAddMemberSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {
        Debug.Log("Member added to group successfully." + result.ToJson());
    }
    //Cuando a�adir miembtro NO funciona
    private void OnAddMemberFailure(PlayFabError error)
    {
        Debug.LogError("Error adding member to group: " + error.ErrorMessage);
    }

    //Cuando Username funciona
    void OnUsernameSuccess(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {

        //Debug.Log(result.FunctionResult);



        string jsonString = result.FunctionResult.ToString();

        PlayerDataUsername playerDataUsername = JsonUtility.FromJson<PlayerDataUsername>(jsonString);

        string username = playerDataUsername.getPlayerUsername;

        gameManager.username = username;

        Debug.Log("El Master Nombre es = " + username); // output: "prueba1"


    }
    //Error General

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    void OnSucces(PlayFab.ClientModels.ExecuteCloudScriptResult result)
    {
        Debug.Log(result.ToJson());
    }
}