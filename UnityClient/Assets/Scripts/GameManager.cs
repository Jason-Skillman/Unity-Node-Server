using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    void Start() {
        NetworkManager.GetInstance().StartServer();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Debug.Log("Space");
        }
    }

}
