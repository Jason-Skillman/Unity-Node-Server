using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private static NetworkManager INSTANCE;

    private static SocketIOComponent socket;

    public GameObject clientPrefab;
    public GameObject otherClientPrefab;
    public Transform playerSpawnPos;

    //This client's player game object
    private GameObject clientPlayer;


    public bool HasConnected {
        get;
        private set;
    }


    private NetworkManager() { }


    void Awake() {
        if(INSTANCE == null) INSTANCE = this;

        socket = GetComponent<SocketIOComponent>();
    }

    void Start() {
        socket.On("open", OnConnect);
        socket.On("connectInitialize", OnConnectInitialize);
        //socket.On("clientConnected", OnClientConnected);
    }

    /// <summary>
    /// Called when this client has connected to the server
    /// Note: Put any initializing in the OnConnectInitialize instead
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnect(SocketIOEvent obj) {
        Debug.Log("Client connected to server");

        //Tell the server that we have connected
        socket.Emit("connected");
    }

    /// <summary>
    /// Called by the server after OnConnected
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnectInitialize(SocketIOEvent obj) {
        clientPlayer = Instantiate(clientPrefab);
        clientPlayer.transform.position = playerSpawnPos.position;
    }

    /// <summary>
    /// Called when another client has connected to the server
    /// </summary>
    /// <param name="obj"></param>
    private void OnClientConnected(SocketIOEvent obj) {
        /*GameObject player = Instantiate(playerPrefab);
        players.Add(obj.data["id"].ToString(), player);

        //Debug.Log("Player id: " + obj.data["id"]);
        Debug.Log("Current player count: " + players.Count);*/
    }

    /// <summary>
    /// Returns the singleton instance
    /// </summary>
    /// <returns></returns>
    public static NetworkManager GetInstance() {
        if(INSTANCE == null) INSTANCE = new NetworkManager();
        return INSTANCE;
    }

    /// <summary>
    /// Starts the socket.io connection with the server
    /// </summary>
    public void StartServer() {
        //Has the connection already been started
        if(HasConnected) {
            Debug.Log("Server connection already started");
            return;
        }

        HasConnected = true;
        socket.Connect();
    }

}
