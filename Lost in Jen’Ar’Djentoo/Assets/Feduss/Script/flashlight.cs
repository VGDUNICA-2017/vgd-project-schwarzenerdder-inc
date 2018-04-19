using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {

    public Light torcia;

	// Use this for initialization
	void Start () {
        torcia.intensity = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Torcia") && GetComponent<SwitchWeapon>().getTorch) {
            if (torcia.intensity == 1)
            {
                torcia.intensity = 0;
            }
            else
            {
                torcia.intensity = 1;
            }
        }

	}
}
