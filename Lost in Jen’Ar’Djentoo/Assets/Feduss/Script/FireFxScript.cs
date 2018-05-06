using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFxScript : MonoBehaviour {

    private Vector3 rot;

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
		
        rot= GameObject.FindGameObjectWithTag("Hands").transform.localEulerAngles;
        rot.y += 90f;
        transform.localEulerAngles = rot;


    }
}
