using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReloadAmmo : MonoBehaviour {

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
			hud.centralBoxText("Premi E per avere giusto una possibilità in più di battere quel bestione");
			hud.centralBoxEnabler (true);
		};
	}

	void OnTriggerStay (Collider other){
		if (other.gameObject.CompareTag ("Player") && Input.GetButtonDown("Open Door") && Time.time > nextUse) {
			nextUse = Time.time + cooldown;
			InventorySystem inventory = other.GetComponent<InventorySystem> ();
			int carriedWeapon;
			Animator anim = other.gameObject.GetComponent<Animator>();
			if (anim.GetBool ("Pistol"))
				carriedWeapon = 0;
			else if (anim.GetBool ("Smg"))
				carriedWeapon = 2;
			else
				carriedWeapon = -1;
			inventory.ammoPickup (16, 0, carriedWeapon);
			inventory.ammoPickup (60, 2, carriedWeapon);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player")) hud.centralBoxEnabler (false);
	}
}
