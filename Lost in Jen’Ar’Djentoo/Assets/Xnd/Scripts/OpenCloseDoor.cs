using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour {

	private Animator mov = null; // declarating the animator
	[SerializeField] private AudioClip m_OpenDoor;
	[SerializeField] private AudioClip m_CloseDoor;
	private AudioSource m_AudioSource;
	public bool unlocked = false;

	void Start(){
		mov = GetComponent<Animator> (); // initializing animator
		m_AudioSource = GetComponent<AudioSource>();
	}
 
	// when player enters the trigger, the door opens(setting the bool allows to change state and play animation)
		void OnTriggerEnter(Collider other){
			if ((other.CompareTag ("Player")) && (unlocked == true)) {
				m_AudioSource.clip = m_OpenDoor;
				m_AudioSource.Play ();
				mov.SetBool ("open", true); 
			}

		}

		//opposite of entering the trigger
		void OnTriggerExit(Collider other){
			if ((other.CompareTag ("Player")) && (unlocked == true)) {
				m_AudioSource.clip = m_CloseDoor;
				m_AudioSource.Play ();
				mov.SetBool ("open", false);
			}

		}
		
}
