using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Manager;
using Settings;
namespace Player
{
    public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
    {

        [SerializeField]
        public NetworkPlayer NetworkPlayer;
        //Other components
        CharacterInputHandler CharacterInputHandler { get; set; }

        private UserManager userManager;
        PlayerList playerList;

        public PhotonManager photonManager;

        // Start is called before the first frame update

        private void Awake()
        {

            userManager = UserManager.FindInstance();
            photonManager = PhotonManager.FindInstance();
        }

        /// <summary>
        /// When a player enters, spawns with the selected prefab and gives it the network properties.
        /// Initialice WaitPlayer
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            //If the player has joined the game, his character is spawned
            if (player == runner.LocalPlayer)
            {
                GameObject handler = GameObject.Find("NetworkRunnerHandler");
                Debug.Log("Spawning Player");

                Debug.Log(NetworkPlayer);
                Debug.Log(player);

                runner.Spawn(NetworkPlayer, handler.transform.position, Quaternion.identity, player, OnBeforeSpawn);
            }
            else
            {
                CharacterInputHandler.InitializeAsync();
            }

            StartCoroutine(WaitPlayer());
        }
        /// <summary>
        /// Detects that the user has left, to remove them from the list of players.
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="player"></param>
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            //I think this update the player list
            StartCoroutine(WaitPlayer());
        }

        IEnumerator WaitPlayer()
        {
            //Needed??
            yield return new WaitForSeconds(2);
            playerList = GameObject.Find("Menus").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<PlayerList>();
            playerList.ListPlayers();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            //It gets the Player Input

            if (CharacterInputHandler == null && NetworkPlayer.Local != null)
                CharacterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

            if (CharacterInputHandler != null)
            {
                input.Set(CharacterInputHandler.GetNetworkInput());

            }
        }

        /// <summary>
        /// Initialises the individual properties of each user
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="obj"></param>
        public void OnBeforeSpawn(NetworkRunner runner, NetworkObject obj)
        {

            obj.GetComponent<NetworkPlayer>().avatar = photonManager.avatarNumber;
            obj.GetComponent<NetworkPlayer>().ActorID = photonManager.Runner.LocalPlayer;
            obj.GetComponent<NetworkPlayer>().nickname = userManager.Username;
            photonManager.CurrentPlayer = obj.gameObject;

            //What's this?
            obj.transform.GetChild(2).gameObject.SetActive(true);
        }







        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected to a room");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            throw new NotImplementedException();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            throw new NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            throw new NotImplementedException();
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            throw new NotImplementedException();
        }



        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            throw new NotImplementedException();
        }





        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("Scene load");
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            Debug.Log("Loading scene");
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            throw new NotImplementedException();
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log("Leaving Room");
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            throw new NotImplementedException();
        }


    }
}
