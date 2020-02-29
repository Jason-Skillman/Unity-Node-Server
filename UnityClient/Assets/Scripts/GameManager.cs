using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    void Start() {
        NetworkManager.GetInstance().StartConnection();
    }

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                GameObject gameObjectHit = hit.collider.gameObject;

                if(gameObjectHit.CompareTag("Walkable")) {
                    Vector3 hitPos = hit.point;
                    GameObject clientPlayer = NetworkManager.GetInstance().ClientPlayer;
                    Player player = clientPlayer.GetComponent<Player>();

                    player.MoveTo(hitPos);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Debug.Log("Space");
            //NetworkManager.GetInstance().StartServer();
        }
    }

}
