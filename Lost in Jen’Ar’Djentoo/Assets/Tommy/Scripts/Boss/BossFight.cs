using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossFight : MonoBehaviour {

    private HUDSystem hud;
    public bool fightStarted = false; //variabile che indica se la bossfight è iniziata
    private bool onetime = true; //variabile per assicurarsi che la bossfight parta una volta sola
    public string boss_name;

	// Use this for initialization
	void Start () {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") && onetime) { //se il player esce per la prima volta dalla zona
            (GameObject.Find(boss_name)).GetComponent<NavMeshAgent>().enabled = true; //attiva il navmesh del boss

            //Nella boss fight del livello 2, chiudo la porta alle spalle del personaggio
            if (SceneManager.GetActiveScene().name.Equals("Level_2")) {
                GameObject.FindGameObjectWithTag("FinalDoor").GetComponentInChildren<OpenCloseDoor>().unlocked = false;
                GameObject.FindGameObjectWithTag("FinalDoor").GetComponentInChildren<Animator>().SetBool("open", false);
            }
            fightStarted = true; //inizia la boss fight
            hud.bossBarEnabler(true);
            hud.bossNameSetter(boss_name);
            hud.bossBarSetter(500, 500); //imposta l'hud del boss
            onetime = false; //disattiva onetime
            Destroy(gameObject.GetComponent<BoxCollider>(), 5f);
        }
            

    }
}
