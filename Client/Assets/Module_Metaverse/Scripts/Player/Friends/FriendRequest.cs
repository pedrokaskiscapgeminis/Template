using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequest : MonoBehaviour { 

    private string id;

    [SerializeField] private FriendList friendList;
    [SerializeField] private FriendManager friendManager;

    public string Id { get => id; set => id = value; }

    /// <summary>
    /// PlayFab - Accept Friend Request
    /// </summary>
    public void AcceptRequest()
    {
        friendList = gameObject.GetComponentInParent<FriendList>();
        friendManager = gameObject.GetComponentInParent<FriendManager>();
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = id
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSuccess, OnAddFriendFailure);
        
    }
    /// <summary>
    /// PlayFab - Deny Friend Request
    /// </summary>
    public void DenyRequest()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new
            {
                friendplayfabid = id
            },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnDeniFriendSuccess, OnAddFriendFailure);
    }

    /// <summary>
    /// Callback for successful CloudScript function call
    /// </summary>

    private void OnDeniFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Se rechazo");
        Destroy(this.gameObject);

    }
    private void OnAddFriendSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaa");
        friendManager.GetFriendsConfirmedList();



    }

    /// <summary>
    /// Callback for Failed CloudScript function call
    /// </summary>
    private void OnAddFriendFailure(PlayFabError error)
{
    Debug.LogError("Failed to retrieve Friends List: " + error.ErrorMessage);
    Destroy(this.gameObject);
    }
    
}