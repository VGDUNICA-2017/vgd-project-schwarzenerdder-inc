using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Movement : MonoBehaviour {

	Transform player;
	NavMeshAgent nav;
	Animator anim;
	public float attackDistance = 5.0f;
	private float currentDistance;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	private void Update () {
		print(nav.enabled);
	}

	void FixedUpdate(){
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Punch") && GameObject.Find("TriggerFight").GetComponent<BossFight>().fightStarted == true) {
			nav.enabled = true;
		} else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Punch")) { 
			nav.enabled = false;
		}
		if (nav.enabled == true) {

			nav.SetDestination(player.position);
			currentDistance = Vector3.Distance(this.transform.position, player.position);
			//print("distance: " + currentDistance);
			if (currentDistance > attackDistance) Chase();
			else Attack();
		}
	}

	void Chase() {
		anim.SetBool("IsWalking", true);
	}

	void Attack() {
		anim.SetBool("IsWalking", false);
		nav.enabled = false;
		anim.SetTrigger("AttackTrigger");
	}
}

