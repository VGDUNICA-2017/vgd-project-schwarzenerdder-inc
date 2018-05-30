using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour {
	private bool loadCheck;
	private bool inside;
	private bool oneTime;
	[SerializeField]private PlayerData pdata;
	private HUDSystem hud;
	public string sceneName;
    public GameObject loadingScreen;
    public Text loadingProgress;
    public GameObject premiper;
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
		if (loadCheck) {
			DontDestroyOnLoad (gameObject);
		}

		if (loadCheck && inside && Input.GetButton ("Open Door")) {
			hud.onLoadHUD ();
			StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Load>().LoadAsync(sceneName, loadingProgress, premiper, slider));
			oneTime = true;
		}

		if (SceneManager.GetActiveScene ().name.Equals (sceneName) && oneTime) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<InventorySystem> ().LoadPlayer (pdata);
			oneTime = false;
			print ("Loaded");
			Destroy (gameObject);
		}
	}

	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player") && !oneTime) {
			loadCheck = true;
			pdata = other.GetComponent<InventorySystem> ().SavePlayer ();
			pdata.fromFile = false;
			print ("Player saved!");
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
			GameObject.FindGameObjectWithTag ("HUD").GetComponent<PauseMenu> ().enabled = true;
			inside = false;
		}
	}
}
