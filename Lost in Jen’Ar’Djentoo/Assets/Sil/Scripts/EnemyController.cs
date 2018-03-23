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
	[SerializeField] public Vector3 startPosition;
	public float currentDistance;
	public float lastDistance;
	public bool backToStart;
	private bool canRoar;
	private const int MaxHealth = 100;
	public int health;
	public bool isDead;

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

		canRoar = true;

		health = MaxHealth;
		if (health < 1) {
			isDead = true;
		} else {
			isDead = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Se il nemico è vivo
		if (health > 0) {
			lastDistance = currentDistance;
			currentDistance = Vector3.Distance (this.transform.position, player.transform.position);

			//Azione di posizionamento
			if (currentDistance > spotDistance) {
				canRoar = true;

				if ((lastDistance <= spotDistance) || backToStart) {
					//Trigger animazione "bersaglio perso"
					if (!backToStart) {
						animator.SetTrigger ("Lost");
					}

					//Controllo sullo stato dell'animator
					//Durante l'animazione "PlayerLost", il nemico non si deve muovere
					float startDistance = Vector3.Distance(this.transform.position, startPosition);
					if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PlayerLost")) {
						agent.enabled = false;
					} else {
						//Sistema di ritorno
						agent.enabled = true;
						agent.SetDestination (startPosition);

						if (startDistance < 5.0f && startDistance >= 1.5f) {
							animator.SetFloat ("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), 0.0f, Time.deltaTime));
						} else if (startDistance < 1.5f) {
							agent.enabled = false;
							animator.SetFloat ("Speed", 0.0f);
						}
					}

					//Altro sistema di ingresso:
					//Il primo update che entra verifica la condizione (lastDistance <= spotDistance)
					//Gli altri entrano finché non torna alla posizione iniziale
					if (startDistance < 1.5f) {
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
				//Ruggito
				if (canRoar) {
					animator.SetTrigger ("Spotted");
					canRoar = false;
				}

				//Se il nemico si trova nello stato di moving, allora l'agent viene attivato
				//In qualunque altro stato, è inattivo
				//Questo previene il movimento durante attacco e ruggito
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Moving")) {
					animator.SetFloat ("Speed", 1.0f);
					agent.enabled = true;
					agent.SetDestination (player.transform.position);

				} else {
					agent.enabled = false;
				}


			} else if (currentDistance <= attackDistance) {
				//Attacco
				this.transform.LookAt(player.transform.position);
				agent.enabled = false;
				animator.SetFloat ("Range", Random.Range (-1.0f, 1.0f));
				animator.SetFloat ("Speed", 0.0f);
				animator.SetTrigger ("Attack");
			}
		} else {
			//Se il nemico non è ancora in fase di morte, attiva tale animazione
			if (isDead) {
				animator.SetTrigger ("Death");
				animator.SetFloat ("Speed", 0.0f);
				agent.enabled = false;
				isDead = false;
			}
		}
	}

	public void takeDamage(int damage) {
		this.health -= damage;

		animator.SetFloat ("Range", Random.Range (-1.0f, 1.0f));
		animator.SetTrigger ("Hit");

		if (health < 1) {
			isDead = true;
		}
	}
}
