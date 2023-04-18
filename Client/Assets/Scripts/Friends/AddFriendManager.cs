using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using TMPro;

public class AddFriendManager : MonoBehaviour
{
    public TMP_InputField inputFriendUsername; // The username of the friend to be added

    public void AddFriend()
    {
        // Call the PlayFab Cloud Script function to add the friend
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AddFriend", // Replace with the name of your Cloud Script function
            FunctionParameter = new { friendUsername = inputFriendUsername.text }, // Pass in any required parameters
            GeneratePlayStreamEvent = true // Set to true if you want PlayStream events to be generated for this API call
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
    }

    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Friend added successfully!");
        // Handle success, if necessary
    }

    private void OnAddFriendFailure(PlayFabError error)
    {
        Debug.LogError("Failed to add friend: " + error.ErrorMessage);
        // Handle failure, if necessary
    }
    public void GetFriendsList()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getFriendsList",
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnGetFriendsListSuccess, OnGetFriendsListFailure);
    }

    // Callback for successful CloudScript function call
    private void OnGetFriendsListSuccess(ExecuteCloudScriptResult result)
    {
     
        if (result.FunctionResult != null)
        {

            Debug.Log(result.FunctionResult);
            // Access the Friends List from 'result.FunctionResult["Friends"]'

        }
    }

    // Callback for failed CloudScript function call
    private void OnGetFriendsListFailure(PlayFabError error)
    {
        Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    }
}