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

    //All clients connected on server
    private Dictionary<string, GameObject> clients;
    private bool flagConnectedOnce;

    //This client's player game object
    public GameObject ClientPlayer {
        get;
        private set;
    }

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
        //socket.On("move", OnClientMove);
    }

    void OnDestroy() {
        StopConnection();
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
        ClientPlayer = Instantiate(clientPrefab);
        ClientPlayer.transform.position = playerSpawnPos.position;
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
        string id = obj.data["id"].ToString();
        Debug.Log("Player " + id + " is moving");

        //Get the game object of the moving player
        GameObject gameObjectPlayer = clients[id];
        Player player = gameObjectPlayer.GetComponent<Player>();
        player.MoveTo(JsonToVector3(obj.data));
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
    public void StartConnection() {
        //Has the connection already been started
        if(HasConnected) {
            Debug.Log("Server connection already started");
            return;
        }

        HasConnected = true;
        socket.Connect();
    }

    /// <summary>
    /// Stops the socket.io connection with the server
    /// </summary>
    public void StopConnection() {
        HasConnected = false;
        socket.Close();
    }

    /// <summary>
    /// Moves the player on the server
    /// </summary>
    /// <param name="pos">The new position of the player</param>
    public void MovePlayer(Vector3 pos) {
        socket.Emit("move", VectorToJson(pos));
    }

    /// <summary>
    /// Converts a vector3 position to json
    /// </summary>
    /// <param name="vector">The position to convert</param>
    /// <returns>The json object</returns>
    public static JSONObject VectorToJson(Vector3 vector) {
        return new JSONObject(string.Format(@"{{""x"":""{0}"",""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y, vector.z));
    }

    public static Vector3 JsonToVector3(JSONObject json) {
        float x = float.Parse(json["x"].ToString().Replace("\"", ""));
        float y = float.Parse(json["y"].ToString().Replace("\"", ""));
        float z = float.Parse(json["z"].ToString().Replace("\"", ""));
        Debug.Log("x: " + x + ", z: " + z);
        return new Vector3(x, 0, z);
    }

}
