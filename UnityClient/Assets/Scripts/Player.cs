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
        
    }

    public void MoveTo(Vector3 pos) {
        agent.SetDestination(pos);
    }

}
