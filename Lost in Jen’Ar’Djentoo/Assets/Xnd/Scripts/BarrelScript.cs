using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour {

	public float pushX;
	public float pushY;
	public float pushZ;
	public Rigidbody rb;
	private bool firstTimeEntering = true;
	public int damage;
	public GameObject explosion;
	public AudioClip explosound;
	private AudioSource source;
	private float initPosX;
	private float initPosZ;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		source = GetComponent<AudioSource>();
		initPosX = rb.transform.position.x;
		initPosZ = rb.transform.position.z;
	}
	
	//the barrel moves when player enters the trigger
	void OnTriggerEnter(Collider other){
		if (firstTimeEntering && other.gameObject.tag == "Player") {
			rb.AddForce (pushX,pushY, pushZ, ForceMode.Impulse);
			firstTimeEntering = false;
		}
	}

	//explosive barrel damages player's life
	void OnCollisionEnter(Collision collision){

		if (collision.collider.CompareTag ("Player")) {
			collision.gameObject.GetComponent<InventorySystem> ().takeDamage (damage);
			Instantiate (explosion, transform.position, transform.rotation);
			source.clip = explosound;
			source.Play ();
			
		}else if(rb.transform.position.x < initPosX || rb.transform.position.x < initPosZ){
			Instantiate (explosion, transform.position, transform.rotation);
			source.clip = explosound;
			source.Play ();
		}
	}

    private void OnCollisionStay(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            Destroy(gameObject, 2f);
        }
    }

}
