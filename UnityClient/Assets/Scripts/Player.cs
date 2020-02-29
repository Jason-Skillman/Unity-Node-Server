using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    private NavMeshAgent agent;


    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                //Debug.Log("hit " + hit.collider.gameObject.name);

                GameObject gameObjectHit = hit.collider.gameObject;

                if(gameObjectHit.CompareTag("Walkable")) {
                    Vector3 hitPos = hit.point;
                    NavigateTo(hitPos);
                }
            }
        }
    }

    public void NavigateTo(Vector3 pos) {
        agent.SetDestination(pos);

        NetworkManager.GetInstance().MovePlayer(pos);

        //NetworkManager.GetInstance().Emit("move", NetworkManager.VectorToJson(pos));
    }

}
