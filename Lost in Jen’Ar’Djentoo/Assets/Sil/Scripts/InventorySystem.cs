using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
	//Altri script
	private HUDSystem hudScript;

	//Riferimenti vita
	private const int fullHealth = 100;
	private int currentHealth;
	private bool isDead;
	private int medKits;

    //Riferimenti armi (pistola = index 0; fucile a pompa = index 1; SMG = index 2)
    private int[] ammo = new int[] {0, 0, 0};
    private int[] invAmmo = new int[] {0, 0, 0};
	private bool[] weapons = new bool[] {false, false, false};
	private int[] magCapacity = new int[] {8, 5, 30};
	private int[] maxInvAmmo = new int[] {40, 20, 120};

	//Riferimenti oggetti
	private bool ascia;
	private bool torcia;
	private bool mappa;
	private bool cesoie;

	//Altri riferimenti
	private bool noWeapon = true;

    private PlaySound playsound;

	// Use this for initialization
	void Start () {
		//hudScript = gameObject.GetComponent<HUDSystem> ();
		hudScript = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();

		//Vita default
		currentHealth = fullHealth;
		isDead = false;
		medKits = 0;
		hudScript.radialHealthSet (currentHealth, fullHealth);
		hudScript.medKitSet (medKits);

		//Armi default
		setWeapon (false, 0);
		setWeapon (false, 1);
		setWeapon (false, 2);

		//Oggetti default
		setTorcia (false);
		setMappa (false);
		setCesoie (false);
		setAscia (false);

        playsound = GameObject.FindGameObjectWithTag("Player").GetComponent<PlaySound>();
    }

	// Update is called once per frame
	void Update () {
		if (this.getMappa()) {
			hudScript.minimapEnabler (true);
		}

		if ((getWeapon (0) || getWeapon (1) || getWeapon (2)) && noWeapon) {
			hudScript.hudShotsEnabler (true);
			noWeapon = false;
		} 
	}

	//Funzione per subire danni
	public void takeDamage(int damage) {
		currentHealth -= damage;

		hudScript.getDamage ();
		hudScript.radialHealthSet (currentHealth, fullHealth);
        playsound.PlayPlayerHitSound();

		if (currentHealth < 1) {
			isDead = true;
            playsound.PlayPlayerDeath();
		}
	}

	//Funzione per curare danni
	public void healDamage (int amount) {
		currentHealth += amount;

		if (currentHealth > fullHealth) {
			currentHealth = fullHealth;
		}

		hudScript.radialHealthSet (currentHealth, fullHealth);
	}

	//Getter punti vita
	public int getHealth(){
		return this.currentHealth;
	}

	//Getter status vita
	public bool getStatus () {
		return this.isDead;
	}

	//Funzione di uso del medKit
	public void useMedKit () {
		if (medKits > 0) {
			if (getHealth () < 100) {
				healDamage (50);
				medKits--;
				hudScript.medKitSet (medKits);
			}
		} else {
			hudScript.medKitSet (-1);
		}
	}

	//Getter dei medKit
	public int medKitsLeft () {
		return this.medKits;
	}

	//Funzione di raccolta medkit. Rende la risposta a "ho raccolto il medKit?"
	public bool medkitPickup () {
		if (medKits < 3) {
			medKits++;
			hudScript.medKitSet (medKits);
			return true;
		} else {
			return false;
		}
	}

	//Pausa caffe! u.u

	//Getter munizioni in canna
	public int ammoLeft (int indexArma) {
		return ammo [indexArma];
	}

	//Getter munizioni in inventario
	public int ammoInvLeft (int indexArma) {
		return invAmmo [indexArma];
	}

	//Funzione di inizializzazione arma
	//Setta che l'arma è in inventario, con un caricatore pieno e uno di scorta
	public void startAmmo (int indexArma) {
		setWeapon (true, indexArma);
		this.ammo [indexArma] = magCapacity[indexArma];
		this.invAmmo [indexArma] += magCapacity[indexArma];
        if (this.invAmmo [indexArma] > this.maxInvAmmo [indexArma]) {
			this.invAmmo [indexArma] = this.maxInvAmmo [indexArma];
		}

		hudScript.reloadWeapon (this.ammo [indexArma], this.invAmmo [indexArma]);
		hudScript.reticleEnabler (true);
	}

	//Funzione di sparo
	public void shot (int indexArma) {
		if (ammo [indexArma] > 0) {
			ammo [indexArma]--;
			hudScript.shotUpdate (ammo [indexArma]);
		} else {
			hudScript.emptyMag ();
		}
	}

	//Funzione di cambio arma nell'HUD
	public void changeWeaponHUD (int indexArma) {
		hudScript.reloadWeapon (ammo [indexArma], invAmmo [indexArma]);
	}

	//Funzione di ricarica
	public void reloadWeapon (int indexArma) {
		int passedShots;

		//Colpi da mettere nel caricatore
		if (invAmmo [indexArma] > magCapacity [indexArma]) {
			invAmmo [indexArma] -= magCapacity [indexArma];
			passedShots = magCapacity [indexArma];
		} else {
			passedShots = invAmmo [indexArma];
			invAmmo [indexArma] = 0;
		}

		//Colpi da estrarre dall'arma, se il suo caricatore ha ancora colpi
		if (ammo [indexArma] != 0) {
			passedShots += ammo [indexArma];
			ammo[indexArma] = 0;
			if (passedShots > magCapacity [indexArma]) {
				invAmmo [indexArma] += passedShots - magCapacity [indexArma];
				passedShots = magCapacity [indexArma];
			}
		}

		//Ricarica
		ammo [indexArma] = passedShots;
		hudScript.reloadWeapon (ammo [indexArma], invAmmo [indexArma]);
	}

	//Funzione di raccolta munizioni
	public int ammoPickup (int pickup, int indexArma, int currentWeapon) {
		int lasting = 0;

		//Ricarica inventario
		invAmmo [indexArma] += pickup;

		//Calcolo eventuali colpi eccedenti
		if (invAmmo [indexArma] > maxInvAmmo [indexArma]) {
			lasting = invAmmo [indexArma] - maxInvAmmo [indexArma];
			invAmmo [indexArma] = maxInvAmmo [indexArma];
		} else {
			lasting = 0;
		}

		//Setting
		if (currentWeapon == indexArma) {
			hudScript.invAmmoUpdate (invAmmo [indexArma]);
		}
		return lasting;
	}

	//Setter e getter armi
	public void setWeapon (bool state, int indexArma) {
		this.weapons [indexArma] = state;
	}
	public bool getWeapon (int indexArma) {
		return this.weapons [indexArma];
	}

	//Goditi un Togo. Togo è un piacere che puoi goderti sempre u.u

	//Setter e Getter ascia
	public void setAscia (bool state) {
		this.ascia = state;
	}
	public bool getAscia () {
		return this.ascia;
	}

	//Setter e Getter torcia
	public void setTorcia (bool state) {
		this.torcia = state;
	}
	public bool getTorcia () {
		return this.torcia;
	}

	//Setter e Gettere mappa
	public void setMappa (bool state) {
		this.mappa = state;
	}
	public bool getMappa () {
		return this.mappa;
	}

	//Setter e Getter cesoie
	public void setCesoie (bool state) {
		this.cesoie = state;
	}
	public bool getCesoie () {
		return this.cesoie;
	}
}
