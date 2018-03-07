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

    //Riferimenti armi (pistola = index 0; fucile a pompa = index 1; SMG = index 2)
    private int[] ammo = new int[] {0, 0, 0};
    private int[] invAmmo = new int[] {0, 0, 0};
	private bool[] weapons = new bool[] {false, false, false};
	private int[] magCapacity = new int[] {8, 5, 30};
	private int[] maxInvAmmo = new int[] {40, 20, 120};

	//Riferimenti oggetti
	private bool torcia;
	private bool mappa;
	private bool cesoie;

	// Use this for initialization
	void Start () {
		hudScript = gameObject.GetComponent<HUDSystem> ();

		//Vita default
		currentHealth = fullHealth;
		hudScript.radialHealthSet (currentHealth, fullHealth);
		isDead = false;

		//Armi default
		setWeapon (false, 0);
		setWeapon (false, 1);
		setWeapon (false, 2);

		//Oggetti default
		setTorcia (false);
		setMappa (false);
		setCesoie (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	//Funzione per subire danni
	public void takeDamage(int damage) {
		currentHealth -= damage;

		hudScript.getDamage ();
		hudScript.radialHealthSet (currentHealth, fullHealth);

		if (currentHealth < 0) {
			isDead = true;
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

	//Getter status vita
	public bool getStatus () {
		return this.isDead;
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
		this.invAmmo [indexArma] = magCapacity[indexArma];
		hudScript.reloadWeapon (this.ammo [indexArma], this.invAmmo [indexArma]);
		hudScript.hudReticle (true);
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

	public int ammoPickup (int pickup, int indexArma) {
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
		hudScript.invAmmoUpdate (invAmmo [indexArma]);
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
