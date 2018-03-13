using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFxScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        transform.parent = GameObject.Find("P226 (Impugnata)").transform;
        Destroy(gameObject, 0.15f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
