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
	private Vector3 lastPlayerPosition;
	private bool tracking;
	private bool backToStart;
	private bool canRoar;
	private bool randomAttack;
	private bool isPlayerVisible;
	private bool lastVisible;
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
		playsound = this.GetComponent<PlayEnemySound> ();

		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		playerSpotPoint = GameObject.Find ("SpotPoint").transform;

		distance = spotDistance + 1.0f;
		backToStart = false;
		startPosition = this.transform.position;
		startDistance = 0.0f;

		isPlayerVisible = false;
		tracking = false;
		canRoar = true;
		randomAttack = true;
        debug = false;

		health = MaxHealth;
		if (health <= 1) {
			deathCall = true;
		} else {
			deathCall = false;
		}
    }

	void Update () {
		//Check sulla vita del nemico
		if (health > 0) {
			//Se il nemico è vivo, calcolo i nuovi dati di posizione
			distance = Vector3.Distance (this.transform.position, playerTransform.position);

			//Codice dell'angolo copiato online. Dura a spiegarlo
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
                	"; Ray: " + rayHit +
					"; Compare: " + (hittedElement.collider.gameObject.CompareTag ("Player") ||
							hittedElement.collider.gameObject.name.Equals("l'ascia (Impugnata)") ||
							hittedElement.collider.gameObject.CompareTag("Hands")) +
                	"\nHitted: " + hittedElement.collider);
                } else {
                	print ("Distance: " + (distance <= spotDistance) +
                	"; Angle: " + (angle <= spotAngle) +
                	"; Ray: " + rayHit);
                }
            }

			//Flag di posizione
			lastVisible = isPlayerVisible;
			isPlayerVisible = (distance <= spotDistance) && 								//Se il player è entro la distanza di visione,
					(angle <= spotAngle) && 												// entro l'angolo di visione,
					rayHit && 																// il ray ha colpito qualcosa
					(hittedElement.collider.gameObject.CompareTag ("Player") ||				// e quel qualcosa è il player
					hittedElement.collider.gameObject.CompareTag ("Hands") ||				//o un suo componente
					hittedElement.collider.gameObject.name.Equals ("l'ascia (Impugnata)"));

			//Definizione azione
			if (isPlayerVisible) {
				
				//Il player è nel cono di visione
				if (distance >= attackDistance) {//Se il player è oltre la soglia di attacco
					movingAction ();
//					print ("M");
				} else {
					attackAction ();
//					print ("A");
				}
			} else {

				//Il player non è al momento in vista
				if (lastVisible || tracking) {
					trackingAction ();
//					print ("T");
				} else {
					canRoar = true;
					startDistance = Vector3.Distance (this.transform.position, startPosition);
					if (startDistance > 0.8f) {
						returnAction ();
//						print ("R");
					} else {
						idleAction ();
//						print ("I");
					}
				}
			}
		} else {
			//Se il nemico non è vivo
			deathAction ();
		}
	}
		
	public void FixedUpdate () {
		//Reset del flag per scegliere l'animazione di attacco
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Attacking")) {
			randomAttack = true;
		}
	}

	//Funzione di idle
	public void idleAction () {
		agent.enabled = false;
		animator.SetFloat ("Speed", 0.0f);
		backToStart = false;
	}

	//Funzione di ritorno
	public void returnAction () {
		//Bersaglio perso
		if (!backToStart) {
			animator.SetTrigger ("Lost");
			backToStart = true;
			animator.SetBool ("Attack", false);
		}

		/* Controllo sullo stato dell'animator
		 * Durante l'animazione "PlayerLost", il nemico non si deve muovere
		 */
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PlayerLost")) {
			agent.enabled = false;
		} else {
			//Sistema di ritorno
			if (startDistance >= 3.0f) {
				agent.enabled = true;
				agent.SetDestination (startPosition);
				animator.SetFloat ("Speed", 1.0f);

			} else if (startDistance < 3.0f && startDistance >= 0.5f) {
				agent.enabled = true;
				agent.SetDestination (startPosition);
				animator.SetFloat ("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), 0.0f, Time.deltaTime));

			} else if (startDistance < 0.5f) {
				agent.enabled = false;
				animator.SetFloat ("Speed", 0.0f);
			}
		}
	}

	//Funzione di movimento
	public void movingAction () {
		//Ruggito
		if (canRoar) {
			animator.SetBool ("Spotted", true);
            playsound.PlayRoarSound();
			canRoar = false;
		} else {
			animator.SetBool ("Spotted", false);
		}

		/* Se il nemico si trova nello stato di moving, allora l'agent viene attivato
		 * In qualunque altro stato, è inattivo
		 * Questo previene lo "sliding" durante altri stati
		 */
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Moving")) {
			animator.SetFloat ("Speed", 1.0f);
			animator.SetBool ("Attack", false);
			agent.enabled = true;
			lastPlayerPosition = playerTransform.position;
			agent.SetDestination (lastPlayerPosition);

		} else {
			agent.enabled = false;
		}
	}

	//Funzione di inseguimento
	public void trackingAction () {
		float trackDistance = Vector3.Distance (this.transform.position, lastPlayerPosition);

		if (trackDistance >= 3.0f) {
			tracking = true;
			agent.enabled = true;
			agent.SetDestination (lastPlayerPosition);
			animator.SetFloat ("Speed", 1.0f);

		} else if (trackDistance < 3.0f && trackDistance >= 0.5f) {
			tracking = false;
			agent.enabled = true;
			agent.SetDestination (lastPlayerPosition);
			animator.SetFloat ("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), 0.0f, Time.deltaTime));

		} else if (trackDistance < 0.5f) {
			tracking = false;
			agent.enabled = false;
			animator.SetFloat ("Speed", 0.0f);
		}

		animator.SetBool ("Spotted", false);
		animator.SetBool ("Attack", false);
	}

	//Funzione di attacco
	public void attackAction() {
		//Un solo frame di random.range per attacco
		if (randomAttack && animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
			animator.SetFloat ("Range", (float)Random.Range (-1, 1));
			randomAttack = false;
		}

		this.transform.LookAt(playerTransform.position);
		agent.enabled = false;
		animator.SetFloat ("Speed", 0.0f);
		animator.SetBool ("Attack", true);
        playsound.PlayEnemyAttackSound();
	}

	//Funzione di morte
	public void deathAction() {
		//Se il nemico non è ancora in fase di morte, attiva tale animazione
		if (deathCall) {
            playsound.PlayEnemyDeath();
			animator.SetTrigger ("Death");
			animator.SetFloat ("Speed", 0.0f);
			agent.enabled = false;
			deathCall = false;
		}

		//Dopo 3 cicli nello stato Dying, il nemico viene cancellato
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dying") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 3.0f) {
            Destroy(gameObject);
        }
    }

	//Funzione per subire danno
	public void takeDamage(int damage) {
		this.health -= damage;

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
