using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSystem : MonoBehaviour {
	//Riferimenti
	public Image screenDamage;
	public Image radialHealth;
	public Text healthText;
	public GameObject shotsUI;
	public Text ammoText;
	public Text invAmmoText;
	public GameObject minimapSet;
	public GameObject reticle;

	//Supporti per il flash dello schermo
	private bool damaged;
	private float flashSpeed = 2.5f;
	public Color flashColor = new Color (1.0f, 0.0f, 0.0f, 0.1f);

	//Supporti per il radiale di vita
	private Color fullHealthColor = Color.green;
	private Color halfHealthColor = Color.yellow;
	private Color zeroHealthColor = Color.red;

	//Supporti per le munizioni
	private bool noAmmo;

	//Supporti per il mirino
	private RectTransform reticleTransform;
	private const float MovingSize = 50.0f;
	private const float StayingSize = 30.0f;

	//Supporti vari
	private bool moving;

	// Use this for initialization
	void Start () {
		//Disabilitazion elementi HUD
		shotsUI.SetActive (false);
		minimapSet.SetActive (false);
		reticle.SetActive (false);

		//Recupero elementi
		reticleTransform = (RectTransform)reticle.transform;

		//Set variabili di supporto
		damaged = false;
		noAmmo = false;
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Flash dello schermo dopo il danno. True = frame del danno
		if (damaged) {
			screenDamage.color = flashColor;
			damaged = false;
		} else {
			screenDamage.color = Color.Lerp (screenDamage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		//Flash dei colpi senza colpi in canna. True = frame dello sparo
		if (noAmmo) {
			ammoText.color = Color.red;
			ammoText.fontSize = 30;
			noAmmo = false;
		} else {
			ammoText.color = Color.Lerp (ammoText.color, Color.white, flashSpeed * Time.deltaTime);
			ammoText.fontSize = (int)Mathf.Lerp (ammoText.fontSize, 25, flashSpeed * Time.deltaTime);
		}

		//Ingrandimento del reticolo in movimento (true), e rimpicciolimento (false)
		if (moving) {
			reticleTransform.sizeDelta = new Vector2 (
				Mathf.Lerp (reticleTransform.rect.width, MovingSize, 0.2f), 
				Mathf.Lerp (reticleTransform.rect.height, MovingSize, 0.2f));
			//reticleTransform.rect.width = Mathf.Lerp (reticleTransform.rect.width, MovingSize, Time.deltaTime);
			//reticleTransform.rect.height = Mathf.Lerp (reticleTransform.rect.height, MovingSize, Time.deltaTime); 
		} else {
			reticleTransform.sizeDelta = new Vector2 (
				Mathf.Lerp (reticleTransform.rect.width, StayingSize, 0.2f), 
				Mathf.Lerp (reticleTransform.rect.height, StayingSize, 0.2f));
			//reticleTransform.rect.width = Mathf.Lerp (reticleTransform.rect.width, StayingSize, Time.deltaTime);
			//reticleTransform.rect.height = Mathf.Lerp (reticleTransform.rect.height, StayingSize, Time.deltaTime); 
		}
	}

	//Gestore del radiale di vita
	public void radialHealthSet (int health, int maxHealth) {
		//Testo nel radiale
		healthText.text = health + "%";

		//Percentile del radiale
		radialHealth.fillAmount = (float) health / maxHealth;

		if (health == maxHealth) {
			//Vita al 100%
			radialHealth.color = fullHealthColor;

		} else if ((health < maxHealth) && (health > (maxHealth / 2))) {
			//Vita tra il 99% e il 51%
			radialHealth.color = Color.Lerp (
				halfHealthColor, 
				fullHealthColor, 
				(float)(health - (maxHealth / 2)) / (maxHealth / 2));

		} else if (health == (maxHealth / 2)) {
			//Vita al 50%
			radialHealth.color = halfHealthColor;

		} else if ((health < (maxHealth / 2)) && (health > 0)) {
			//Vita tra il 49% e l'1%
			radialHealth.color = Color.Lerp (
				zeroHealthColor, 
				halfHealthColor, 
				(float)health / (maxHealth / 2));

		} else if (health <= 0) {
			//Vita allo 0%
			radialHealth.enabled = false;
		}
	}

	//setter per flash schermo
	public void getDamage () {
		this.damaged = true;
	}

	//Colpi in canna
	public void shotUpdate (int ammo) {
		ammoText.text = ammo.ToString ();
	}

	//Colpi in inventario
	public void invAmmoUpdate (int ammo) {
		invAmmoText.text = ammo.ToString ();
	}

	//Setter per flash colpi
	public void emptyMag () {
		noAmmo = true;
	}

	//Ricarica
	public void reloadWeapon (int ammo, int invAmmo) {
		shotUpdate (ammo);
		invAmmoUpdate (invAmmo);
	}

	//Setter di movimento
	public void movingState (bool state) {
		moving = state;
	}

	//Attiva HUD armi
	public void hudShots () {
		shotsUI.SetActive (true);
	}

	//Attiva minimappa
	public void hudMinimap () {
		minimapSet.SetActive (true);
	}

	//Attiva/disttiva mirino
	public void hudReticle (bool state) {
		reticle.SetActive (state);
	}
}
