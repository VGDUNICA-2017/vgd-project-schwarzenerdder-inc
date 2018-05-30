using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour {
	/// <summary>
	/// author: silvio
	/// </summary>

	private Animator enemyAnimator;
	public bool damageDealt;
	public int damage;

	void Start () {
		enemyAnimator = this.GetComponentInParent<Animator>();
		damageDealt = false;
	}

	void FixedUpdate () {
		if (!enemyAnimator.GetCurrentAnimatorStateInfo (0).IsName("Attacking")) {
			damageDealt = false;
		}
	}

	public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (enemyAnimator.GetCurrentAnimatorStateInfo (0).IsName("Attacking")) {
				if (!damageDealt) {
					other.GetComponent<InventorySystem>().takeDamage(damage);
					damageDealt = true;
				}
			}
		}
	}
}
