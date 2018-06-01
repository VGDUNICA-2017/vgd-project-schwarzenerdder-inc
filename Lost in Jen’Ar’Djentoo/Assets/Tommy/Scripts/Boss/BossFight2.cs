using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFight2 : MonoBehaviour {

	private HUDSystem hud;
	public bool fightStarted = false; //variabile che indica se la bossfight è iniziata
	private bool onetime = true; //variabile per assicurarsi che la bossfight parta una volta sola

	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player") && onetime) {//se il player esce per la prima volta dalla zona
			(GameObject.Find("Jentoo")).GetComponent<NavMeshAgent>().enabled = true;
			fightStarted = true; //inizia la boss fight
			hud.bossBarEnabler(true);
			hud.bossNameSetter("Jentoo, Lord of Jen'Ar'Djentoo");
			hud.bossBarSetter(1000, 1000);//imposta l'hud del boss
			onetime = false; //disattiva onetime
			Destroy(gameObject.GetComponent<BoxCollider>(), 5f);
		}


	}
}
