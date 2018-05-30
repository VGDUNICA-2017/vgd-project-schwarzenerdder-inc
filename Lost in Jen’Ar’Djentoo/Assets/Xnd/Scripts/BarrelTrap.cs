using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrap : MonoBehaviour {

	public float push;
	public Rigidbody rb;
	private bool firstTimeEntering = true;
	public int damage = 10;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	//the barrel moves when player enters the trigger
	void OnTriggerEnter(Collider other){
		if (firstTimeEntering && other.gameObject.tag == "Player") {
			rb.AddForce (push,0.0f, 0.0f, ForceMode.Impulse);
			firstTimeEntering = false;
		}
			
	}
	//radioactive barrel damages player's life
	void OnCollisionEnter(Collision other){

		if (other.collider.CompareTag ("Player")) {
			other.gameObject.GetComponent<InventorySystem> ().takeDamage (damage);
		}
	}

}
