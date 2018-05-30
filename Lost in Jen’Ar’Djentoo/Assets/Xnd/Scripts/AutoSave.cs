using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour {

	public int health;
	public Vector3 position;
	public int[] ammoLeft = new int[] {0, 0, 0};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Collider>().CompareTag ("Player")) {
			health = other.gameObject.GetComponent<InventorySystem>().currentHealth;
			//sono stanco, da aggiustare
			//ammoLeft = other.gameObject.GetComponent<InventorySystem> ().ammoInvLeft;
			position = other.gameObject.transform.position;
				
		}
	}
}
