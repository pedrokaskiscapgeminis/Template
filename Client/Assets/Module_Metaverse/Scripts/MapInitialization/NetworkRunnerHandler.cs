using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;
    GameManager gameManager;
    public string map;

    private void Awake()
    {
        gameManager = GameManager.FindInstance();
    }

    private void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner";

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

       
        Debug.Log("Server NetworkRunner Started");
    }

    /// <summary>
    /// Function that initializes the network runner
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="gameMode"></param>
    /// <param name="address"></param>
    /// <param name="scene"></param>
    /// <param name="initialized"></param>
    /// <returns></returns>

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {

            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;
        GameManager.FindInstance().SetRunner(runner);

         SessionProps props = new SessionProps();
        props.StartMap = gameManager.currentMap;
        props.RoomName = gameManager.GetRoomName();
        props.AllowLateJoin = true;
        props.PlayerLimit = gameManager.playerCount;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            CustomLobbyName = "Lobby_Play",
            SessionName = gameManager.GetRoomName() + "-" + map,
            PlayerCount = gameManager.playerCount,
            Initialized = initialized,
            SceneManager = sceneManager,
            SessionProperties = props.Properties
        }); ;

    }
}