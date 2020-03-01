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

                    //Move the player on the local client
                    GameObject clientPlayer = NetworkManager.GetInstance().ClientPlayer;
                    Player player = clientPlayer.GetComponent<Player>();
                    player.MoveTo(hitPos);

                    //Move the player on the server
                    NetworkManager.GetInstance().MovePlayer(hitPos);
                }
            }
        }
    }

}
