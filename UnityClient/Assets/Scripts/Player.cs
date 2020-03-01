using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    public TextMeshProUGUI testDisplayName;

    private NavMeshAgent agent;

    public string Id {
        get; set;
    }

    public bool IsMainPlayer {
        get; set;
    }


    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start() {
        //Setup the display name
        if(!Id.Equals(string.Empty))
            testDisplayName.text = Id.Replace("\"", string.Empty);

        //Is this the main player
        if(IsMainPlayer) {
            testDisplayName.color = Color.green;
        }
    }

    void Update() {
        
    }

    public void MoveTo(Vector3 pos) {
        agent.SetDestination(pos);
    }

}
