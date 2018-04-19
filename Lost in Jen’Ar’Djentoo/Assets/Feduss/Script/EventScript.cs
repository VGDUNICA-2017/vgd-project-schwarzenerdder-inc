using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour {

    private GameObject shutter1;

	// Use this for initialization
	void Start () {
        shutter1 = GameObject.FindGameObjectWithTag("Serranda");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.name.Equals("shutter1"))
        {
            shutter1.GetComponent<Animator>().SetTrigger("Close");
        }
    }
}
