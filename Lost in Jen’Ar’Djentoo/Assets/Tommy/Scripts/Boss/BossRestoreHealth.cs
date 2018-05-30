using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRestoreHealth : MonoBehaviour {

	private HUDSystem hud;
	private bool oneAndOnly = true;
	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player") && oneAndOnly) {
			hud.centralBoxText("Premi E per darti una rinfrescata. Ma... chi è quella persona scusate?");
			hud.centralBoxEnabler (true);
		} else if (other.gameObject.CompareTag ("Player")) {
			hud.centralBoxText("Ehi, non essere ingordo");
			hud.centralBoxEnabler (true);
		};
	}

	void OnTriggerStay (Collider other){
		if (other.gameObject.CompareTag ("Player") && Input.GetButtonDown("Open Door") && oneAndOnly) {
			oneAndOnly = false;
			InventorySystem inventory = other.GetComponent<InventorySystem> ();
			inventory.healDamage(80);
			hud.centralBoxEnabler (false);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) hud.centralBoxEnabler (false);
	}
}
