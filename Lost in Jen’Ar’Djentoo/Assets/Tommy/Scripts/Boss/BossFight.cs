using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFight : MonoBehaviour {

    private HUDSystem hud;
    public bool fightStarted = false;

	// Use this for initialization
	void Start () {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            (GameObject.Find("Jen'ni")).GetComponent<NavMeshAgent>().enabled = true;
            fightStarted = true;
            //hud.hudMinimap(false);
        }
            

    }
}
