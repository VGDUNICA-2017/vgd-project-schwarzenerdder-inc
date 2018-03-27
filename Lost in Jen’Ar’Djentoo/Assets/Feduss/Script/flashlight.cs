using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {

    public Light torcia;

	// Use this for initialization
	void Start () {
        //torcia = GameObject.Find("Luce").GetComponent<Light>();
        torcia.intensity = 10;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Torcia")) {
            if (torcia.intensity == 10)
            {
                torcia.intensity = 0;
            }
            else
            {
                torcia.intensity = 10;
            }
        }

	}
}
