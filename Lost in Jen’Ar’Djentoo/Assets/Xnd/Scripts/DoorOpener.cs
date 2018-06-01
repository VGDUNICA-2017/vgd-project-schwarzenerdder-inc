using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {
	public GameObject door;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		//when enemy dies, unlock target door
		if (GetComponent<EnemyController>().health <= 0) {
			door.GetComponent<OpenCloseDoor> ().unlocked = true;
			//print ("DoorOpener if funziona");
		}
			
	}
}
