using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {

    public Light torcia;

	// Use this for initialization
	void Start () {
        torcia.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

            if (Input.GetKeyDown("f"))
            {
                if (torcia.enabled) torcia.enabled = false;
                else torcia.enabled = true;
            }

	}
}
