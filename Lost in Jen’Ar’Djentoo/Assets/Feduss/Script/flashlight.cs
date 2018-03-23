using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {

    private Light torcia;

	// Use this for initialization
	void Start () {
        torcia = GameObject.Find("Luce").GetComponent<Light>();
        torcia.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

            if (Input.GetButtonDown("Torcia"))
            {
                if (torcia.enabled) torcia.enabled = false;
                else torcia.enabled = true;
            }

	}
}
