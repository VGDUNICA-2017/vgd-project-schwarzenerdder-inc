using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBarrelTrap : MonoBehaviour {

	public float push;
	public Rigidbody rb;
	private bool firstTimeEntering = true;
	public int damage = 10;
	public GameObject explosion;
	public AudioClip explosound;
	private AudioSource source;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		source = GetComponent<AudioSource>();
	}

	//the barrel moves when player enters the trigger
	void OnTriggerEnter(Collider other){
		if (firstTimeEntering && other.gameObject.tag == "Player") {
			rb.AddForce (push,0.0f, 0.0f, ForceMode.Impulse);
			firstTimeEntering = false;
		}
			
	}
	//explosive barrel damages player's life
	void OnCollisionEnter(Collision other){

		if (other.collider.CompareTag ("Player")) {
			other.gameObject.GetComponent<InventorySystem> ().takeDamage (damage);
			Instantiate (explosion, transform.position, transform.rotation);
			source.clip = explosound;
			source.Play ();
			Destroy (gameObject);
		}
	}

}
