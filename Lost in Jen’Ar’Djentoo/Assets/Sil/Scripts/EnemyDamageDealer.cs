using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour {

	private Animator enemyAnimator;

	void Start () {
		enemyAnimator = this.GetComponentInParent<Animator> ();
	}

	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if(enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
				other.GetComponent<InventorySystem> ().takeDamage (15);
			}
		}
	}
}
