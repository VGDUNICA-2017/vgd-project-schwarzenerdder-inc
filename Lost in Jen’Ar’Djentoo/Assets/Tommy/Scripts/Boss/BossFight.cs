using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFight : MonoBehaviour {

    private HUDSystem hud;
    public bool fightStarted = false;
    private bool onetime = true;

	// Use this for initialization
	void Start () {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") && onetime) {
            (GameObject.Find("Jen'ni")).GetComponent<NavMeshAgent>().enabled = true;
            fightStarted = true;
            hud.bossBarEnabler(true);
            hud.bossNameSetter("Jen'ni");
            hud.bossBarSetter(500, 500);
            onetime = false;
            Destroy(gameObject.GetComponent<BoxCollider>(), 5f);
        }
            

    }
}
