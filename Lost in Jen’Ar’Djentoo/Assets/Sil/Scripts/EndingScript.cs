using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScript : MonoBehaviour {
	private bool end;

	private GameObject hud;
	private GameObject player;

	//Use this for initialization
	void Start () {
		end = false;

		hud = GameObject.FindGameObjectWithTag ("HUD");
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (end) {
			//Disattivo ogni singolo elemento dell'hud, tranne l'ending screen
			foreach (Transform child in hud.transform) {
				if (!child.gameObject.name.Equals ("EndingScreen")) {
					child.gameObject.SetActive (false);
				}
			}

			//Disattivo alcune script del player, e la pausa dall'hud
			player.GetComponent<Moving>().enabled = false;
			player.GetComponent<SwitchWeapon>().enabled = false;
			player.GetComponent<Load>().enabled = false;
			hud.GetComponent<PauseMenu>().enabled = false;

			//Disattivo i nemici della scena
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
				enemy.SetActive (false);
			}

			end = false;

			hud.GetComponent<HUDSystem> ().endingTrigger ();
		}
	}

	public void ending () {
		end = true;
	}
}
