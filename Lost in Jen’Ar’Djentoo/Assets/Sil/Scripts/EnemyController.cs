using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
	private GameObject player;
	private Animator animator;
	private NavMeshAgent agent;

	public float spotDistance = 20.0f;
	public float attackDistance = 5.0f;
	private float lastDistance;
	private bool atStart;
	private Vector3 startPosition = new Vector3 ();

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		animator = this.GetComponent<Animator> ();
		startPosition = this.transform.position;
		agent = this.GetComponent<NavMeshAgent> ();

		animator.SetBool ("Idle", true);
		animator.SetBool ("Spotted", false);
		animator.SetBool ("Attack", false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currentPosition = this.transform.position;

		//Aggiornameto costante della posizione del personaggio
		if (currentPosition == startPosition) {
			atStart = true;
		} else {
			atStart = false;
		}
	}

	public void FixedUpdate () {
		float distance = Vector3.Distance (this.transform.position, player.transform.position);

		//TODO Rivedere le distanze
		if (distance >= spotDistance) { //Oltre la soglia di visione
			//Se nel frame precedente era sotto la soglia di visione o se non è nella posizione iniziale
			if ((lastDistance < spotDistance) || (!atStart)) { 
				backToStart ();
			} else {
				idleState ();
			}

		} else if ((distance < spotDistance) && (distance >= attackDistance)) { //Tra le soglie di visione e attacco
			followPlayer ();

		} else if (distance < attackDistance) { //Sotto la soglia di attacco
			attack ();
		}

		lastDistance = distance;
	}

	//Funzione di idle
	public void idleState () {
		animator.SetBool ("Idle", true);
		animator.SetBool ("Spotted", false);
		animator.SetBool ("Attack", false);

		agent.enabled = false;
		//TODO
	}

	//Funzione di inseguimento
	public void followPlayer () {
		animator.SetBool ("Idle", false);
		animator.SetBool ("Spotted", true);
		animator.SetBool ("Attack", false);

		agent.enabled = false;
		//agent.SetDestination (player.transform.position);
		//TODO
	}

	//Funzione di ritorno al punto iniziale
	public void backToStart () {
		agent.enabled = true;
		agent.SetDestination (startPosition);
		//TODO
	}

	//Funzione di attacco
	public void attack () {
		animator.SetBool ("Idle", false);
		animator.SetBool ("Spotted", true);
		animator.SetBool ("Attack", true);

		agent.enabled = false;
		//TODO
	}
}
