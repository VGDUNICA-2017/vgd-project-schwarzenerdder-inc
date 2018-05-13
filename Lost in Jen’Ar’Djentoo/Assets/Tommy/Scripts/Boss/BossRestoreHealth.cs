using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRestoreHealth : MonoBehaviour {

	private HUDSystem hud;
	private float nextUse;
	public float cooldown;
	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			hud.centralBoxText("Premi E per darti una rinfrescata. E occhio alle spalle!");
			hud.centralBoxEnabler (true);
		};
	}

	void OnTriggerStay (Collider other){
		if (other.gameObject.CompareTag ("Player") && Input.GetButtonDown("Open Door") && Time.time > nextUse) {
			nextUse = Time.time + cooldown;
			InventorySystem inventory = other.GetComponent<InventorySystem> ();
			inventory.healDamage(70);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) hud.centralBoxEnabler (false);
	}
}
