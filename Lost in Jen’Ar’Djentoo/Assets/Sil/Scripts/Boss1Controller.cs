using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour {

	//Componeneti
	private Animator animator;
	private Transform playerTransform;
	private NavMeshAgent agent;
	private PlayEnemySound playsound;

    //Variabili di controllo sulla posizione
    public bool debug;
	public float distance;

	//Variabili di supporto
	private bool randomAttack;
	private const int MaxHealth = 1000;
	public int health;
	private bool deathCall;

	//Elementi da settare
	public float attackDistance = 4.0f;

	void Start () {
		animator = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();

		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

		randomAttack = true;
        debug = false;

		health = MaxHealth;
		if (health < 1) {
			deathCall = true;
		} else {
			deathCall = false;
		}

        //playsound = GetComponent<PlayEnemySound>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		//Check sulla vita del nemico
		if (health > 0) {//Se il nemico è vivo
			//Calcolo nuovi dati di posizione
			distance = Vector3.Distance (this.transform.position, playerTransform.position);

			//Se il player è oltre la soglia di attacco
			if (distance >= attackDistance) {
				movingAction ();
//				print ("M");
			} else {
				if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Attacking")) {
					randomAttack = true;
				}
				attackAction ();
//				print ("A"); 
			}
		} else {
			//Se il nemico non è vivo
			deathAction ();
		}
	}

	public void movingAction() {
		//Se il nemico si trova nello stato di moving, allora l'agent viene attivato
		//In qualunque altro stato, è inattivo
		//Questo previene il movimento durante le altre azioni
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Moving")) {
			animator.SetFloat ("Speed", 1.0f);
			animator.SetBool ("Attack", false);
			agent.enabled = true;
			agent.SetDestination (playerTransform.position);

		} else {
			agent.enabled = false;
		}
	}

	public void attackAction() {
		//Attacco
		if (randomAttack) {
			//animator.SetFloat ("Range", -1.0f);
			animator.SetFloat ("Range", (float)Random.Range (-1, 1));
			randomAttack = false;
		}

		this.transform.LookAt(playerTransform.position);
		agent.enabled = false;
		animator.SetFloat ("Speed", 0.0f);
		animator.SetBool ("Attack", true);
        //playsound.PlayEnemyAttackSound();
	}

	public void deathAction() {
		//Se il nemico non è ancora in fase di morte, attiva tale animazione
		if (deathCall) {
            //playsound.PlayEnemyDeath();
			animator.SetTrigger ("Death");
			animator.SetFloat ("Speed", 0.0f);
			agent.enabled = false;
			deathCall = false;
		}

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3.0f) {
            Destroy(gameObject);
        }
    }

	public void takeDamage(int damage) {
		this.health -= damage;

		if (health < 0) {
			this.health = 0;
		}

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Dying")) {
			animator.SetFloat ("Range", Random.Range (-1.0f, 1.0f));
			animator.SetTrigger ("Hit");
            //playsound.PlayEnemyHitSound();

			if (health == 0) {
				deathCall = true;
			}
		}
	}
}