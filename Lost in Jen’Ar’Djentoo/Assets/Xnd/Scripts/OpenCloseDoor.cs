using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour {

	private Animator mov = null; // declarating the animator

	void Start(){
		mov = GetComponent<Animator> (); // initializing animator
	}
 

		void OnTriggerEnter(Collider other){
			
		mov.SetBool ("open", true); 
		// when something enters the trigger, the door opens(setting the bool allows to change state and play animation)
		}

		void OnTriggerExit(Collider other){

		mov.SetBool ("open", false);
		//opposite of entering the trigger
		}

}
