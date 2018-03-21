using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	//Componeneti
	private Animator animator;
	private GameObject player;
	private NavMeshAgent agent;

	//Variabili di supporto
	private Vector3 startPosition;
	private float currentDistance;
	private float lastDistance;
	private bool backToStart;

	//Elementi da settare
	public float spotDistance = 20.0f;
	public float attackDistance = 3.0f;

	void Start () {
		animator = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();

		player = GameObject.FindGameObjectWithTag ("Player");

		currentDistance = 99.9f;
		backToStart = false;
		startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		lastDistance = currentDistance;
		currentDistance = Vector3.Distance (this.transform.position, player.transform.position);

		//Azione di posizionamento
		if (currentDistance > spotDistance) {
			if ((lastDistance <= spotDistance) || backToStart) {
				//Sistema di ritorno
				agent.enabled = true;
				animator.SetFloat ("Speed", agent.speed);
				agent.SetDestination(startPosition);

				//Trigger animazione "bersaglio perso"
				if (!backToStart) {
					animator.SetTrigger ("Lost");
				}

				//Altro sistema di ingresso:
				//Il primo update che entra verifica la condizione (lastDistance <= spotDistance)
				//Gli altri entrano finché non torna alla posizione iniziale
				if (this.transform.position == startPosition) {
					backToStart = false;
				} else {
					backToStart = true;
				}

			} else {
				//Idle
				agent.enabled = false;
				animator.SetFloat ("Speed", 0.0f);
			}

		} else if ((currentDistance <= spotDistance) && (currentDistance > attackDistance)) {
			//Insegumento del player
			agent.enabled = true;
			animator.SetFloat ("Speed", agent.speed);
			animator.SetTrigger ("Spotted");
			agent.SetDestination (player.transform.position);

		} else if (currentDistance <= attackDistance) {
			//Attacco
			agent.enabled = false;
			animator.SetFloat ("Speed", 0.0f);
			animator.SetTrigger ("Attack");
		}
	}
}
