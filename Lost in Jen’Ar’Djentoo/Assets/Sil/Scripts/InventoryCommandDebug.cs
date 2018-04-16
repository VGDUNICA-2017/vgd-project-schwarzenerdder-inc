using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* SOLO PER TESTING
 * SE PER SBAGLIO FINISCE IN UN PACK, IGNORATELO
 */
public class InventoryCommandDebug : MonoBehaviour {

	private InventorySystem inventario;
	private HUDSystem interfaccia;
	private int weaponIndex = 0;

	void Start () {
		inventario = gameObject.GetComponent<InventorySystem> ();
		interfaccia = gameObject.GetComponent<HUDSystem> ();
	}

	void Update () {
		//Visualizzatore
		if (Input.GetKeyDown (KeyCode.Q)) {
			Debug.Log ("Vita: " + inventario.getHealth() + "/100; MedKit: " + inventario.medKitsLeft() +
				"; Mappa: " + inventario.getMappa() +
				"; Cesoie: " + inventario.getCesoie() + 
				"; Torcia: " + inventario.getTorcia() +
				"\nPistola: " + inventario.getWeapon(0) + ", " + inventario.ammoLeft(0) + "/" + inventario.ammoInvLeft(0) +
				"; Shotgun: " + inventario.getWeapon(1) + ", " + inventario.ammoLeft(1) + "/" + inventario.ammoInvLeft(1) +
				"; SMG: " + inventario.getWeapon(2) + ", " + inventario.ammoLeft(2) + "/" + inventario.ammoInvLeft(2)
			);
		}

		//Cambio arma
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			inventario.changeWeaponHUD (0);
			weaponIndex = 0;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			inventario.changeWeaponHUD (1);
			weaponIndex = 1;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			inventario.changeWeaponHUD (2);
			weaponIndex = 2;
		}

		//Sparo
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			inventario.shot (weaponIndex);
		}

		//Ricarica
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			inventario.reloadWeapon (weaponIndex);
		}

		//Pistola
		if (Input.GetKeyDown (KeyCode.U)) {
			inventario.startAmmo (0);
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			inventario.ammoPickup (12, 0, weaponIndex);
		}

		//Shotgun
		if (Input.GetKeyDown (KeyCode.I)) {
			inventario.startAmmo (1);
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			inventario.ammoPickup (5, 1, weaponIndex);
		}

		//SMG
		if (Input.GetKeyDown (KeyCode.O)) {
			inventario.startAmmo (2);
		}
		if (Input.GetKeyDown(KeyCode.L)) {
			inventario.ammoPickup (30, 2, weaponIndex);
		}

		//Danno e cura
		if (Input.GetKeyDown(KeyCode.T)) {
			inventario.takeDamage (10);
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			inventario.takeDamage (1);
		}
		if (Input.GetKeyDown(KeyCode.Y)) {
			inventario.healDamage (10);
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			inventario.healDamage (1);
		}

		//MedKit
		if (Input.GetKeyDown (KeyCode.Z)) {
		 	inventario.useMedKit ();
	 	}
	 	if (Input.GetKeyDown (KeyCode.X)) {
			inventario.medkitPickup ();
		}

		//Reticolo
		if (Input.GetKeyDown(KeyCode.C)) {
			interfaccia.reticleEnabler (true);
		}
		if (Input.GetKeyDown(KeyCode.V)) {
			interfaccia.reticleEnabler (false);
		}

		//Minimappa
		if (Input.GetKeyDown(KeyCode.M)) {
			inventario.setMappa (true);
		}
	}
}
