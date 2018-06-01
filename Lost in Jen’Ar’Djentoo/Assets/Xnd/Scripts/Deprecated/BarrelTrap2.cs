using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrap2 : MonoBehaviour {

	public float push;
	public Rigidbody rb;
	private bool firstTimeEntering = true;
	public int damage = 10;
	public GameObject explosion;
	[SerializeField] private AudioClip m_explosion;
	private AudioSource m_AudioSource;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		m_AudioSource = GetComponent<AudioSource>();
	}

	//the barrel moves when player enters the trigger
	void OnTriggerEnter(Collider other){
		if (firstTimeEntering && other.gameObject.tag == "Player") {
			rb.AddForce (0.0f, 0.0f, push, ForceMode.Impulse);
			firstTimeEntering = false;
		}

	}

	//radioactive barrel damages player's life
	void OnCollisionEnter(Collision other){

		if (other.collider.CompareTag ("Player")) {
			other.gameObject.GetComponent<InventorySystem> ().takeDamage (damage);
			Instantiate (explosion, transform.position, transform.rotation);
			m_AudioSource.clip = m_explosion;
			m_AudioSource.Play();
			Destroy (gameObject);
		} else if (transform.position.z != 92.18f ) {
			Instantiate (explosion, transform.position, transform.rotation);
			m_AudioSource.clip = m_explosion;
			m_AudioSource.Play ();
			Destroy (gameObject);
		}
	}

}
