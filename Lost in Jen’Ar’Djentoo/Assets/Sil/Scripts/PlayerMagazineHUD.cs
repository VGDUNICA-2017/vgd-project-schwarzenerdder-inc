using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagazineHUD : MonoBehaviour {


	public Text ammoText;
	public Text invAmmoText;

	public int ammo;
	public int magCapacity;
	public int invAmmo;

	public float nextFire;
	private const float cooldownTime = 0.5f;

	// Use this for initialization
	void Start () {
		ammo = 8;
		magCapacity = 8;
		invAmmo = 8;

		nextFire = 0.0f;

		ammoText.text = ammo.ToString();
		invAmmoText.text = invAmmo.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AdjAmmo (int shots) {
		ammo += shots;
		ammoText.text = ammo.ToString();
		//nextFire = Time.time + cooldownTime;
	}
		
	public void Reload () {
		int passedShots;

		if (invAmmo > magCapacity) {
			invAmmo -= magCapacity;
			passedShots = magCapacity;
		} else {
			passedShots = invAmmo;
			invAmmo = 0;
		}

		if (ammo != 0) {
			passedShots += ammo;
			ammo = 0;
			if (passedShots > magCapacity) {
				invAmmo += passedShots - magCapacity;
				passedShots = magCapacity;
			}
		}

		AdjAmmo (passedShots);
		invAmmoText.text = invAmmo.ToString();
		nextFire = Time.time + cooldownTime;
	}

}
