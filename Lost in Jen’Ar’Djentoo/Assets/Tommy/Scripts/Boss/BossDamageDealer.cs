using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageDealer : MonoBehaviour {

    private Animator anim; //animator del boss
    private bool damageDealt; //variabile che indica 

	// Use this for initialization
	void Start () {
		anim = this.GetComponentInParent<Animator>(); //questa script appartiene al braccio del boss, quindi cerca l'animator del parent
        damageDealt = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack")) { //se il boss non sta attaccando
            damageDealt = false;
        }
	}

    public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) { //se il collider collide con il player
            print("we");
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack")) {//se il boss sta attaccando
				if (!damageDealt) {//se non ha già inflitto danno
                    //print("bool");
                    other.GetComponent<InventorySystem>().takeDamage(30);//infliggi danno al player
                    other.GetComponent<Animator>().SetTrigger("Hit");//attiva il trigger del player
                    damageDealt = true;//indica che il boss ha inflitto danno
                }
            }
        }
	}

	//fonte del codice: https://unity3d.com/learn/tutorials/s/survival-shooter-tutorial
}
