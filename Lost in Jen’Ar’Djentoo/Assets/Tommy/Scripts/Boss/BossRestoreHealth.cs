using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRestoreHealth : MonoBehaviour {

	private HUDSystem hud;
	private bool oneAndOnly = true; //variabile per assicurarsi che le casse siano utilizzate una volta sola
	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player") && oneAndOnly) { //se la cassa non è stata mai usata
			hud.centralBoxText("Premi E per darti una rinfrescata. Ma... chi è quella persona scusate?");
			hud.centralBoxEnabler (true);
		} else if (other.gameObject.CompareTag ("Player")) { //altrimenti
			hud.centralBoxText("Ehi, non essere ingordo");
			hud.centralBoxEnabler (true);
		};
	}

	void OnTriggerStay (Collider other){
		if (other.gameObject.CompareTag ("Player") && Input.GetButtonDown("Open Door") && oneAndOnly) {//se la cassa non è stata mai usata
			oneAndOnly = false; //disattiva oneAndOnly
			InventorySystem inventory = other.GetComponent<InventorySystem> ();
			inventory.healDamage(80); //cura il giocatore 
			hud.centralBoxEnabler (false); //disattiva l'hud
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) hud.centralBoxEnabler (false); //quando il giocatore esce dall'area disattiva l'hud
	}
}
