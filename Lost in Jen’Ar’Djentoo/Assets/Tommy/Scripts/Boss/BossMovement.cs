using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour {

    Transform player;
    NavMeshAgent nav;
    Animator anim;
    public float attackDistance = 5.0f; //raggio di attacco del boss
    private float currentDistance; //distanza dal player

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = false;
        anim = GetComponent<Animator>();
    }

    private void Update() {
        //print(nav.enabled);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack") //se il boss non sta attaccando
			&& GameObject.Find("TriggerFight").GetComponent<BossFight>().fightStarted == true) { //e la boss fight è iniziata
            nav.enabled = true; //attiva il navmesh
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack")) { //se il boss sta attaccando
            nav.enabled = false; //disattiva il navmesh
        }
        if (nav.enabled == true) { //se il navmesh è attivo
			nav.SetDestination(player.position);
            currentDistance = Vector3.Distance(this.transform.position, player.position); //controlla la distanza tra boss e player
            //print("distance: " + currentDistance);
			if (currentDistance > attackDistance) Chase(); //se non è nel raggio di attacco insegui
            else Attack(); //sennò attacca
        }
        
	}

    void Chase() {
        anim.SetBool("IsWalking", true); //attiva l'animazione di corsa
    }

    void Attack() {
        anim.SetBool("IsWalking", false); //disattiva l'animazione di corsa
        nav.enabled = false;
        anim.SetTrigger("AttackTrigger"); //attiva l'animazione di attacco
    }

	//fonte del codice: https://unity3d.com/learn/tutorials/s/survival-shooter-tutorial
}
