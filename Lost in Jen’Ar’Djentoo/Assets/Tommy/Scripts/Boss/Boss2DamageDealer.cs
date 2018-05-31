using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2DamageDealer : MonoBehaviour {

	private Animator anim;
	private bool damageDealt;

	// Use this for initialization
	void Start () {
		anim = this.GetComponentInParent<Animator>();
		damageDealt = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Punch")) {
			damageDealt = false;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Punch")) {
				if (!damageDealt) {
					print("bool");
					other.GetComponent<InventorySystem>().takeDamage(40);
					other.GetComponent<Animator>().SetTrigger("Hit");
					damageDealt = true;
				}
			}
		}
	}
}
