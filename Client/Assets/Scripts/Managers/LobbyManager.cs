using Fusion;

using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    //Name of the session/room
    [SerializeField] private TMP_Text sessionName;
    [SerializeField] private TMP_Text sessionNamePanel;

    //Game Manager of the game
    private GameManager gameManager;


    //List of rooms
    private List<RoomItem> sessionItemsList = new List<RoomItem>();

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
    public void setSessionList(List<SessionInfo> sessionList)
    {
        //We clean the list
        Debug.Log("Setting session list");
        cleanSessions();
        //We instantiate the items in the interface.

        foreach(SessionInfo session in sessionList)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetSessionInfo(session);
            sessionItemsList.Add(newRoom);

        }
     
    }


    //It sets the panel of players when you enter a session
    public void setPlayerPanel(string sessionName)
    {
        this.sessionNamePanel.text = sessionName;
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
        
    }

    
    //Function when a user leaves the session
    public void onClickLeaveSession()
    {
        setLobbyPanel();
        gameManager.LeaveSession();
    }

    public void cleanSessions()
    {

        foreach (RoomItem item in sessionItemsList)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        sessionItemsList.Clear();
       
    }

   
}