using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour {

	private Animator enemyAnimator;
	private bool damageDealt;

	void Start () {
		enemyAnimator = this.GetComponentInParent<Animator> ();
		damageDealt = false;
	}

	void FixedUpdate () {
		if (!enemyAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Attacking")) {
			damageDealt = false;
		}
	}

	public void OnTriggerEnter (Collider other) {
		
		if (other.gameObject.CompareTag("Player")) {
			if (enemyAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Attacking")) {
				if (!damageDealt) {
					other.GetComponent<InventorySystem> ().takeDamage (15);
					damageDealt = true;
				}
			}
		}
	}
}
