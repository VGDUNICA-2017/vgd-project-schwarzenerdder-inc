using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	//Componeneti
	private Animator animator;
	private Transform playerTransform;
	private Transform playerSpotPoint;
	public GameObject vision;
	private NavMeshAgent agent;

    //Variabili di controllo sulla posizione
    public bool debug;
	public float distance;
	private float angle;
	private bool rayHit;

	//Variabili di supporto
	private Vector3 startPosition;
	private bool backToStart;
	private bool canRoar;
	private bool randomAttack;
	private float startDistance;
	private const int MaxHealth = 100;
	public int health;
	private bool deathCall;
	public LayerMask mask;

	//Elementi da settare
	public float spotDistance = 20.0f;
	public float attackDistance = 3.0f;
	public float spotAngle = 60.0f;

    private PlayEnemySound playsound;

	void Start () {
		animator = this.GetComponent<Animator> ();
		agent = this.GetComponent<NavMeshAgent> ();

		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		playerSpotPoint = GameObject.Find ("SpotPoint").transform;

		distance = spotDistance + 1.0f;
		backToStart = false;
		startPosition = this.transform.position;
		startDistance = 0.0f;

		canRoar = true;
		randomAttack = true;
        debug = false;

		health = MaxHealth;
		if (health < 1) {
			deathCall = true;
		} else {
			deathCall = false;
		}

        playsound = GetComponent<PlayEnemySound>();
    }
	
	// Update is called once per frame
	void Update () {
        //Check sulla vita del nemico
		if (health > 0) {//Se il nemico è vivo
			//Calcolo nuovi dati di posizione
			distance = Vector3.Distance (this.transform.position, playerTransform.position);

			//Codice dell'angolo copiato online. Dura a spiegarlo :D
			Vector3 target = playerTransform.position - this.transform.position;
			angle = Vector3.Angle (target, this.transform.forward);

			RaycastHit hittedElement;
			vision.transform.LookAt (playerSpotPoint);
			rayHit = Physics.Raycast (vision.transform.position, vision.transform.forward, out hittedElement, spotDistance, mask);
            if (debug) {
                Debug.DrawRay (vision.transform.position, vision.transform.forward * spotDistance, Color.green);
                if (rayHit) {
                	print ("Distance: " + (distance <= spotDistance) +
                	"; Angle: " + (angle <= spotAngle) +
                ";Ray: " + rayHit +
                	";Compare: " + (hittedElement.collider.gameObject.CompareTag ("Player") || hittedElement.collider.gameObject.name.Equals("la Torcia (Impugnata)")) +
                	"\nHitted: " + hittedElement.collider);
                } else {
                	print ("Distance: " + (distance <= spotDistance) +
                	"; Angle: " + (angle <= spotAngle) +
                	";Ray: " + rayHit);
                }
                //			print("Roar: "+canRoar+";Back: "+backToStart);
            }
            //Definizione azione
            if ((distance <= spotDistance) && 						//Se il player è entro la distanza di visione,
				(angle <= spotAngle) && 									// entro l'angolo di visione,
				rayHit && 													// il ray ha colpito qualcosa
				(hittedElement.collider.gameObject.CompareTag ("Player") ||	// e quel qualcosa è il player
				 hittedElement.collider.gameObject.name.Equals("la Torcia (Impugnata)") || 
                 hittedElement.collider.gameObject.name.Equals("l'ascia (Impugnata)"))) {//o un suo oggetto

				//Il player è nel cono di visione
				if (distance >= attackDistance) {//Se il player è oltre la soglia di attacco
					movingAction ();
//					print ("M");
				} else {
					attackAction ();
//					print ("A");
				}
			} else {
				
				//Il player NON è nel cono di visione
				canRoar = true;
				startDistance = Vector3.Distance(this.transform.position, startPosition);
				//print ("Distanza da casa: " + startDistance);
				if (startDistance > 0.8f) {//Se il nemico non è entro la posizione di partenza
					returnAction ();
//					print ("R");
				} else {
					idleAction ();
//					print ("I");
				}
			}
		} else {
            //Se il nemico non è vivo
            print("gg xd lol 2");
            deathAction ();
		}
	}

	public void FixedUpdate () {
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Attacking")) {
			randomAttack = true;
		}
	}

	public void idleAction () {
		agent.enabled = false;
		animator.SetFloat ("Speed", 0.0f);
		backToStart = false;
	}

	public void returnAction() {
		//Trigger animazione "bersaglio perso"
		if (!backToStart) {
			animator.SetTrigger ("Lost");
			backToStart = true;
			animator.SetBool ("Attack", false);
		}

		//Controllo sullo stato dell'animator
		//Durante l'animazione "PlayerLost", il nemico non si deve muovere
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PlayerLost")) {
			agent.enabled = false;
		} else {
			//Sistema di ritorno
			agent.enabled = true;
			agent.SetDestination (startPosition);

			if (startDistance < 3.0f && startDistance >= 0.5f) {
				animator.SetFloat ("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), 0.0f, Time.deltaTime));
			} else if (startDistance < 0.5f) {
				agent.enabled = false;
				animator.SetFloat ("Speed", 0.0f);
			}
		}
	}

	public void movingAction() {
		//Ruggito
		if (canRoar) {
			animator.SetBool ("Spotted", true);
            playsound.PlayRoarSound();
			canRoar = false;
		} else {
			animator.SetBool ("Spotted", false);
		}


		//Se il nemico si trova nello stato di moving, allora l'agent viene attivato
		//In qualunque altro stato, è inattivo
		//Questo previene il movimento durante attacco e ruggito
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
		//print("Flag: " + randomAttack + "; Anim: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"));
		if (randomAttack && animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
			//animator.SetFloat ("Range", -1.0f);
			animator.SetFloat ("Range", (float)Random.Range (-1, 1));
			randomAttack = false;
		}
		this.transform.LookAt(playerTransform.position);
		agent.enabled = false;
		animator.SetFloat ("Speed", 0.0f);
		animator.SetBool ("Attack", true);
        playsound.PlayEnemyAttackSound();
	}

	public void deathAction() {
        //Se il nemico non è ancora in fase di morte, attiva tale animazione
        print("flag: " + deathCall);
		if (deathCall) {
            playsound.PlayEnemyDeath();
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
            playsound.PlayEnemyHitSound();

			if (health <= 0) {
				deathCall = true;
			}
		}
	}
}
