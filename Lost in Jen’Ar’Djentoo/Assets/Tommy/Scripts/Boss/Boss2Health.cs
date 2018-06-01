using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Health : MonoBehaviour {

	public int startingBossHealth = 1000; //salute iniziale del boss
	public int currentHealth; //salute attuale del boss

	Animator anim;
	CapsuleCollider capsuleCollider;
	bool isDead = false; //indica se il boss è morto

	private HUDSystem hud;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		capsuleCollider = GetComponent<CapsuleCollider>();

		currentHealth = startingBossHealth;

		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>(); //recupera l'hud
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//funzione per il danno del boss
	public void TakeDamage (int amount) {
		if (isDead) return; //se il boss è morto non può ricevere danno
		currentHealth -= amount;
		hud.bossBarSetter(startingBossHealth, currentHealth); //viene aggiornato l'hud
		if (currentHealth <= 0) Death(); //il boss muore se raggiunge 0 o un numero negativo di salute
	}

	//funzione della morte 
	void Death() {
		GameObject.Find("Jentoo").GetComponent<NavMeshAgent>().enabled = false; //disabilita il navmesh del boss
		GameObject.Find("Jentoo").GetComponent<BossMovement>().enabled = false; //disabilita lo script di movimento del boss
		isDead = true;
		capsuleCollider.isTrigger = true;
		hud.bossBarEnabler(false); //l'hud sparisce
		anim.SetTrigger("Die"); //parte l'animazione di morte
	}

	//fonte del codice: https://unity3d.com/learn/tutorials/s/survival-shooter-tutorial
}
