using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_v2 : MonoBehaviour {
	//Sistema movimento mouse e camera
	public float vel_X = 2.0f;
	public float vel_Y = 1.0f;
	private float mov_X = 0.0f;
	private float mov_Y = 0.0f;
	private const float Y_highLimit = -20.0f;
	private const float Y_lowLimit = 25.0f;

	//Potenza del salto
	public float jumpSpeed;

	//Clip audio
	[SerializeField] private AudioClip[] musFootStepSound;
	[SerializeField] private AudioClip musJumpSound;
	[SerializeField] private AudioClip musLandSound;

	//Variabili interne e riferimenti
	private Rigidbody rb;
	private CapsuleCollider collider;
	private AudioSource audio;
	private Animator animator;

	private bool isOnGround = true;

	private Vector3 normalCollider;
	private Vector3 crouchCollider = new Vector3 (0.0f, 0.95f, 0.0f);
	private float normalHeightCollider;
	private float crouchHeightCollider = 2.55f;

	//Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		collider = GetComponent<CapsuleCollider> ();
		audio = GetComponent<AudioSource> ();
		animator = GetComponent<Animator> ();

		AnimDefault ();

		normalCollider = collider.center;
		normalHeightCollider = collider.height;
	}
	
	// Update is called once per frame
	void Update () {
		MouseReg ();
	}

	//Update is called once per phisical frame
	void FixedUpdate () {
		//Movimento
		animator.SetFloat ("Walking", Input.GetAxis ("Vertical"));
		animator.SetFloat ("Turning", Input.GetAxis ("Horizontal"));

		//Suono dei passi
		if((Input.GetAxis("Vertical") != 0.0f) || (Input.GetAxis("Horizontal") != 0.0f)) {
			PlayFootStep();
		}

		//Raccolta di un oggetto
		if (Input.GetButton ("Pickup")) {
			animator.SetTrigger ("Picking");
		}

		//Accucciamento
		if (Input.GetKey (KeyCode.LeftControl)) {
			animator.SetBool ("Crouch", true);
			collider.center = crouchCollider;
			collider.height = crouchHeightCollider;
		} else {
			animator.SetBool ("Crouch", false);
			collider.center = normalCollider;
			collider.height = normalHeightCollider;
		}

		//Corsa
		if (Input.GetKey (KeyCode.LeftShift)) {
			animator.SetBool ("Run", true);
		} else {
			animator.SetBool ("Run", false);
		}

		//Salto
		if(isOnGround) {
			float jump = Input.GetAxis ("Jump");
			Vector3 jumpVector = new Vector3 (0.0f, jump * jumpSpeed, 0.0f);
			rb.AddForce(jumpVector, ForceMode.Impulse);
		}
	}

	public void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Terreno")) {
			isOnGround = true;
			PlayLand ();
		}
	}

	public void OnCollisionExit (Collision other) {
		if (other.gameObject.CompareTag ("Terreno")) {
			isOnGround = false;
			PlayJump ();
		}
	}

	//Funzione di regolazione camera col mouse
	private void MouseReg () {
		if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Picking"))) {
			//Ricavo gli angoli di rotazione X e Y in base allo spostamento del mouse
			mov_X += vel_X * Input.GetAxis ("Mouse X");
			mov_Y -= vel_Y * Input.GetAxis ("Mouse Y");

			//Verifico i limiti di spo
			if (mov_Y < Y_highLimit) {
				mov_Y = Y_highLimit;
			}
			if (mov_Y > Y_lowLimit) {
				mov_Y = Y_lowLimit;
			}

			//Aggiorno la rotazione della camera in base allo spostamento vert. ed oriz. del mouse
			transform.eulerAngles = new Vector3 (mov_Y, mov_X, 0.0f);
		}
	}

	//Valori di default dell'animator
	private void AnimDefault () {
		animator.SetFloat ("Walking", 0.0f);
		animator.SetFloat ("Turning", 0.0f);

		animator.SetBool ("Torch", false);
		animator.SetBool ("Run", false);
		animator.SetBool ("Crouch", false);
	}

	private void PlayFootStep () {
		//Sceglie un suono casuale dall'array, escludendo l'elemento 0
		int n = Random.Range(1, musFootStepSound.Length);
		audio.clip = musFootStepSound[n];
		audio.PlayOneShot(audio.clip);

		//Sposta l'ultimo suono riprodotto nell'indice 0
		musFootStepSound[n] = musFootStepSound[0];
		musFootStepSound[0] = audio.clip;
	}

	private void PlayLand () {
		audio.clip = musLandSound;
		audio.Play();
	}

	private void PlayJump () {
		audio.clip = musJumpSound;
		audio.Play();
	}
}
