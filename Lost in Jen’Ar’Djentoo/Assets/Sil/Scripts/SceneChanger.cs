using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {
	/// <summary>
	/// author: silvio
	/// </summary>

	private bool loadCheck;
	private bool inside;
	private bool oneTime;
	[SerializeField]private PlayerData pdata;
	private HUDSystem hud;
	public string sceneName; //Nome della scena obiettivo
    public Text percentageText;
    public GameObject loadingText;
    public Slider slider;

    // Use this for initialization
    void Start () {
		loadCheck = false;
		inside = false;
		oneTime = false;
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		//Quando sono presenti dei dati salvati, l'oggetto diventa un DontDestroyOnLoad
		if (loadCheck) {
			DontDestroyOnLoad (gameObject);
		}

		//Avvio della nuova scena
		//(Salvataggio effettuato) && (Nella zona finale) && (Comando)
		if (loadCheck && inside && Input.GetButton ("Open Door")) {
			hud.onLoadHUD ();
			StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Load>().LoadAsync(sceneName, percentageText, loadingText, slider));
			oneTime = true;
			loadCheck = false;
		}

		//Nella nuova scena, carica i dati del player
		if (SceneManager.GetActiveScene ().name.Equals (sceneName) && oneTime) {
			oneTime = false;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<InventorySystem> ().LoadPlayer (pdata);
			print ("Loaded");
			Destroy (gameObject);
		}
	}
		
	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player") && !oneTime) {
			loadCheck = true;
			pdata = other.GetComponent<InventorySystem> ().SavePlayer ();

			//"Manomissione" del salvataggio
			//Settando il flag a false, il transform del player non verrà modificato
			pdata.fromFile = false;
			print ("Player saved!");

			//Il menù di pausa viene disattivato nella zona di salvataggio
			GameObject.FindGameObjectWithTag ("HUD").GetComponent<PauseMenu> ().enabled = false;

			hud.centralBoxEnabler (true);
			hud.centralBoxText ("Premi E per apri... ehm, passare alla prossima zona; non potrai tornare indietro...");
			inside = true;
		}
	}

	public void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Player") && !oneTime) {
			hud.centralBoxEnabler (true);
		}
	}

	public void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag ("Player") && !oneTime) {
			hud.centralBoxEnabler (false);

			//Riabilitazione del menù di pausa
			GameObject.FindGameObjectWithTag ("HUD").GetComponent<PauseMenu> ().enabled = true;
			inside = false;
		}
	}
}
