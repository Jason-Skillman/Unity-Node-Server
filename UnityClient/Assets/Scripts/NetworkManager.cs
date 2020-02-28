using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private static SocketIOComponent socket;


    public bool HasServerStarted {
        get;
        private set;
    }


    void Awake() {
        socket = GetComponent<SocketIOComponent>();
    }

    void Start() {
        
    }

    public void StartServer() {
        HasServerStarted = true;
    }

}
