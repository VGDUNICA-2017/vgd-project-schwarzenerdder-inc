using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDSystem : MonoBehaviour {
	/// <summary>
	/// author: silvio
	/// </summary>

	//Supporti per il flash dello schermo
	public Image screenDamage;
	private bool damaged;
	private float flashSpeed = 2.5f;
	public Color flashColor = new Color (1.0f, 0.0f, 0.0f, 0.1f);

	//Supporti per il radiale di vita
	public Image radialHealth;
	public Text healthText;
	private Color fullHealthColor = Color.green;
	private Color halfHealthColor = Color.yellow;
	private Color zeroHealthColor = Color.red;

	//Supporti per i mark dei medkit
	public Image med1;
	public Image med2;
	public Image med3;
	private RectTransform med1Transform;
	private RectTransform med2Transform;
	private RectTransform med3Transform;
	private Color medFullColor = Color.green;
	private Color medEmptyColor = Color.black;
	private bool noMed;
	private bool[] medStatus = new bool[3];

	//Supporti per le munizioni
	public GameObject shotsUI;
	private Text ammoText;
	private Text invAmmoText;
	private bool noAmmo;

	//Supporti per la minimappa
	public GameObject minimapSet;

	//Supporti per il mirino
	public GameObject reticle;
	private RectTransform reticleTransform;
	private const float MovingSize = 50.0f;
	private const float StayingSize = 30.0f;
	private const float AimingSize = 20.0f;
	private bool moving;
	private bool aiming;

	//Supporti di testo
	public Text centralMessageBox;
	public Text sideMessageBox;

	//Supporti per la barra del boss
	public GameObject bossBar;
	public Slider bossHealth;
	public Text BossName;

	//Supporti per il death screen
	public Text deathBox;
	private bool death;
	private Color deathScreenColor = new Color (1.0f, 1.0f, 1.0f, 0.4f);

	//Supporti per il cambio di scena
	public GameObject loadingScreen;

	void Awake () {
		//Recupero elementi HUD
		ammoText = shotsUI.transform.Find ("ShotsLeft").transform.GetComponent<Text> ();
		invAmmoText = shotsUI.transform.Find ("ShotsInventory").transform.GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		screenDamage.color = Color.clear;

		//Disabilitazion elementi HUD
		hudShotsEnabler (false);
		minimapEnabler (false);
		reticleEnabler (false);
		bossBarEnabler (false);
		loadingScreen.SetActive(false);

		//Recupero transform
		reticleTransform = (RectTransform)reticle.transform;
		med1Transform = (RectTransform)med1.transform;
		med2Transform = (RectTransform)med2.transform;
		med3Transform = (RectTransform)med3.transform;

		//Set variabili di supporto
		damaged = false;
		noMed = false;
		noAmmo = false;
		moving = false;
		aiming = false;
		death = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Flash dello schermo dopo il danno. True = frame del danno
		if (death) {
			deathWork ();
		} else if (damaged) {
			screenDamage.color = flashColor;
			damaged = false;
		} else {
			screenDamage.color = Color.Lerp (screenDamage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		//Gestore interfaccia medkit
		if (noMed) {
			//Flash dei medkit quando non se ne hanno
			medFlash (med1, med1Transform, Color.red, medEmptyColor);
			medFlash (med2, med2Transform, Color.red, medEmptyColor);
			medFlash (med3, med3Transform, Color.red, medEmptyColor);
			noMed = false;
		} else {
			//MedKit 1
			if (medStatus [0]) {
				medFlash (med1, med1Transform, medFullColor, medFullColor);
			} else {
				medFlash (med1, med1Transform, medEmptyColor, medEmptyColor);
			}
			//Medkit 2
			if (medStatus [1]) {
				medFlash (med2, med2Transform, medFullColor, medFullColor);
			} else {
				medFlash (med2, med2Transform, medEmptyColor, medEmptyColor);
			}
			//Medkit 3
			if (medStatus [2]) {
				medFlash (med3, med3Transform, medFullColor, medFullColor);
			} else {
				medFlash (med3, med3Transform, medEmptyColor, medEmptyColor);
			}
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

		//Chiusura totale del reticolo in mira (true)
		if (aiming) {
			reticleTransform.sizeDelta = new Vector2 (
				Mathf.Lerp (reticleTransform.rect.width, AimingSize, 0.2f), 
				Mathf.Lerp (reticleTransform.rect.height, AimingSize, 0.2f));
		} else {
			//Ingrandimento del reticolo in movimento (true) o stabilizzaione (false)
			if (moving) {
				reticleTransform.sizeDelta = new Vector2 (
					Mathf.Lerp (reticleTransform.rect.width, MovingSize, 0.2f), 
					Mathf.Lerp (reticleTransform.rect.height, MovingSize, 0.2f));
			} else {
				reticleTransform.sizeDelta = new Vector2 (
					Mathf.Lerp (reticleTransform.rect.width, StayingSize, 0.2f), 
					Mathf.Lerp (reticleTransform.rect.height, StayingSize, 0.2f));
			}
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

	//Gestore del visualizzatore medKit
	public void medKitSet (int quantity) {
		switch (quantity) {
		case -1:
			noMed = true;
			break;
		case 0:
			medStatus [0] = false;
			medStatus [1] = false;
			medStatus [2] = false;
			break;
		case 1:
			medStatus [0] = true;
			medStatus [1] = false;
			medStatus [2] = false;
			break;
		case 2:
			medStatus [0] = true;
			medStatus [1] = true;
			medStatus [2] = false;
			break;
		case 3:
			medStatus [0] = true;
			medStatus [1] = true;
			medStatus [2] = true;
			break;
		}
	}

	//Pulser del medkti
	public void medFlash (Image medKit, RectTransform medTransform, Color flashColor, Color normalColor) {
		if (noMed) {
			medKit.color = flashColor;
			medTransform.sizeDelta = new Vector2 (15.0f, 15.0f);
		} else {
			medKit.color = Color.Lerp (medKit.color, normalColor, Time.deltaTime * 2.0f);
			medTransform.sizeDelta = new Vector2 (
				Mathf.Lerp (medTransform.rect.width, 10.0f, Time.deltaTime * 2.0f),
				Mathf.Lerp (medTransform.rect.height, 10.0f, Time.deltaTime * 2.0f));
		}
	}

	//Setter per flash schermo
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

	//Setter di mira
	public void aimingState (bool state) {
		aiming = state;
	}

	//Attiva/disattiva HUD armi
	public void hudShotsEnabler (bool state) {
		shotsUI.SetActive (state);
	}

	//Attiva/disattiva minimappa
	public void minimapEnabler (bool state) {
		minimapSet.SetActive (state);
	}

	//Attiva/disttiva mirino
	public void reticleEnabler (bool state) {
		reticle.SetActive (state);
	}

	//Attiva/disattiva box centrale di testo
	public void centralBoxEnabler (bool state) {
		centralMessageBox.enabled = state;
	}

	//Setter testo box centrale
	public void centralBoxText (string testo) {
		centralMessageBox.text = testo;
	}

	//Attiva/disattiva box laterale di testo
	public void sideBoxEnabler (bool state) {
		sideMessageBox.enabled = state;
	}

	//Setter testo box laterale
	public void sideBoxText (string testo) {
		sideMessageBox.text = testo;
	}

	//Attiva/disattiva barra della vita del boss
	public void bossBarEnabler (bool state) {
		bossBar.SetActive (state);
	}

	//Setter vita barra del boss
	public void bossBarSetter (int maxHealth, int currentHealth) {
		if (currentHealth < 0) {
			currentHealth = 0;
		}

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		bossHealth.minValue = 0;
		bossHealth.maxValue = maxHealth;

		bossHealth.value = currentHealth;
	}

	//Setter nome del boss
	public void bossNameSetter (string name) {
		BossName.text = name;
	}

	//Attivazione death screen
	public void deathScreenTrigger () {
		screenDamage.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

		deathBox.enabled = true;
		deathBox.fontSize = 1;
		
		death = true;
	}

	//Funzione per la schermata di morte
	private void deathWork () {
		hudShotsEnabler (false);
		minimapEnabler (false);
		reticleEnabler (false);

		if (Mathf.Abs (screenDamage.color.a - deathScreenColor.a) > 2.0f) {
			screenDamage.color = Color.Lerp (screenDamage.color, deathScreenColor, Time.deltaTime);
		} else {
			screenDamage.color = deathScreenColor;
		}

		if (Mathf.Abs (deathBox.fontSize - 60) > 2) {
			deathBox.fontSize = (int) Mathf.Lerp (deathBox.fontSize, 120, Time.deltaTime);
		} else {
			deathBox.fontSize = 60;
		}
	}

	//Schermata durante i passaggi di livello
	public void onLoadHUD () {
		loadingScreen.SetActive(true);

		reticleEnabler (false);
		hudShotsEnabler (false);

		centralBoxEnabler (false);
		sideBoxEnabler (false);

		minimapEnabler (false);
		bossBarEnabler (false);
	}

	//Reset dell'HUD dopo il cambio di livello
	public void resumeHUD (int health, int maxHealth, int medkit, int arma, int ammo, int invAmmo) {
		radialHealthSet (health, maxHealth);
		medKitSet (medkit);

		if ((arma == 1) || (arma == 2)) {
			reticleEnabler (true);
			hudShotsEnabler (true);
			reloadWeapon (ammo, invAmmo);
		} else {
			reticleEnabler (false);
			hudShotsEnabler (false);
		}

		centralBoxEnabler (false);
		sideBoxEnabler (false);

		minimapEnabler (false);
		bossBarEnabler (false);
	}
}
