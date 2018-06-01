using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseDoor : MonoBehaviour {

	private Animator mov = null; // declarating the animator
	[SerializeField] private AudioClip m_OpenDoor;
	[SerializeField] private AudioClip m_CloseDoor;
	private AudioSource m_AudioSource;
	public bool unlocked = false;
    public GameObject nemico;

	void Start(){
		mov = GetComponent<Animator> (); // initializing animator
		m_AudioSource = GetComponent<AudioSource>();
	}

    //Le porte si aprono se il nemico ad esso associato è null (cioè se sono distrutti, poichè uccisi, durante il caricamento) o se sono senza vita
    public void Update() {
        if(nemico == null || (nemico!=null && nemico.GetComponent<EnemyController>().health<=0)) {
            unlocked = true;
        }
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
