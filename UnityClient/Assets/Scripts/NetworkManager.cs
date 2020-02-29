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

    //All clients connected
    private Dictionary<string, GameObject> clients;

    private bool flagConnectedOnce;


    public bool HasConnected {
        get;
        private set;
    }


    private NetworkManager() { }


    void Awake() {
        if(INSTANCE == null) INSTANCE = this;

        socket = GetComponent<SocketIOComponent>();
        clients = new Dictionary<string, GameObject>();
    }

    void Start() {
        socket.On("open", OnConnect);
        socket.On("connectInitialize", OnConnectInitialize);
        socket.On("clientConnect", OnClientConnect);
        socket.On("move", OnClientMove);
    }

    /// <summary>
    /// Called when this client has connected to the server
    /// Note: Put any initializing in the OnConnectInitialize instead
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnect(SocketIOEvent obj) {
        //Prevent the client from connecting more then once
        if(flagConnectedOnce) return;
        flagConnectedOnce = true;

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
    private void OnClientConnect(SocketIOEvent obj) {
        //Create the other client's game object and fetch their id
        GameObject otherClient = Instantiate(otherClientPrefab);
        otherClient.transform.position = playerSpawnPos.position;
        string id = obj.data["id"].ToString();
        clients.Add(id, otherClient);

        Debug.Log("Client " + id + " has connected");
    }

    /// <summary>
    /// Called when another client has moved
    /// </summary>
    /// <param name="obj"></param>
    private void OnClientMove(SocketIOEvent obj) {
        Debug.Log("Network player is moving " + obj.data);

        string id = obj.data["id"].ToString();
        //Debug.Log("Player Id moving: " + id);

        GameObject player = clients[id];
        //Vector3 pos = new Vector3(GetFloatFromJson(obj.data, "x"), 0, GetFloatFromJson(obj.data, "z"));
        //NaviagatePos navigatePos = player.GetComponent<NaviagatePos>();
        //navigatePos.NavigateTo(pos);
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

    /// <summary>
    /// Moves the player on the server
    /// </summary>
    /// <param name="pos">The new position of the player</param>
    public void MovePlayer(Vector3 pos) {
        socket.Emit("move", VectorToJson(pos));
    }


    public static JSONObject VectorToJson(Vector3 vector) {
        return new JSONObject(string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y, vector.z));
    }

}
