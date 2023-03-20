using Fusion;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    //Name of the session/room
    [SerializeField] private TMP_Text sessionName;

    //Game Manager of the game
    private GameManager gameManager;

    //List of rooms
    private List<RoomItem> sessionItemsList = new List<RoomItem>();

    //List of players
    private List<PlayerItem> playerItemsList = new List<PlayerItem>();

    //Lobby panel
    [SerializeField] private RoomItem roomItemPrefab;
    [SerializeField] private Transform contentObject;
    [SerializeField] private GameObject lobbyPanel;
   

    //Players panel
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private GameObject roomPanel;

    [SerializeField] private Transform playerItemParent;

    //When the Lobby awakes, it tries to find the game manager
    private void Awake()
    {
        gameManager = GameManager.FindInstance();
        gameManager.setLobbyManager(this);
        
    }

    //Function when the user clicks on a session/room to join.
    public void OnClickJoinSession(SessionInfo sessionInfo)
    {

        gameManager.JoinSession(sessionInfo);
        
    }

    //Function when the user creates a room/session.
    public void OnClickCreateSession()
    {
        Debug.Log("Creating session");
        //Properties of the room WIP
        SessionProps props = new SessionProps();
        props.StartMap = "Mapa0";
        props.RoomName = sessionName.text;
        props.AllowLateJoin = true;
        
        gameManager.CreateSession(props);
    }

    //Function to add the new sessions to the list of sessions
    public void addSession(SessionInfo sessionInfo)
    {
        //We instantiate the item in the interface.
        RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
        if (newRoom != null)
        {
            newRoom.SetSessionInfo(sessionInfo);
            sessionItemsList.Add(newRoom);
        }

    }

    public void deleteSession(SessionInfo sessionInfo)
    {
        int index = sessionItemsList.FindIndex(x => x.sessionInfo.Name == sessionInfo.Name);
        if (index != -1)
        {
            Destroy(sessionItemsList[index].gameObject);
            sessionItemsList.RemoveAt(index);
        }

    }

    //It sets the panel of players when you enter a session
    public void setPlayerPanel()
    {

        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }

    }

    //It sets the LobbyPanel when the user leaves a session.
    public void setLobbyPanel()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        //We deactivate the panel of the room and Activate the panel of the Lobby interface.
        if (roomPanel != null & lobbyPanel != null)
        {
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }
    }

    //Add a new player to the room (Don't know if it works good 100% rn)
    public void addPlayer()
    {

        gameManager.spawnPlayerItem(playerItemPrefab);
    

        //Sets (only for the session player and not for the others in the room) the visible arrows to be able to select the avatar in the selector
        
        //playerItemsList.Add(playerItem);
    }

    //Function when a user leaves the session
    public void onClickLeaveSession()
    {
        setLobbyPanel();
        gameManager.LeaveSession();
    }

}
