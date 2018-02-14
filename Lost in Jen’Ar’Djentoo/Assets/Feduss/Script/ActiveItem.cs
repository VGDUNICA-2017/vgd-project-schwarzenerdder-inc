using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem : MonoBehaviour {

    private GameObject torcia;


    // Use this for initialization
    void Start () {

        torcia = GameObject.FindGameObjectWithTag("Torcia");
        torcia.SetActive(false);
        
	}

    // Update is called once per frame
    void FixedUpdate () {

        if (GetComponent<Animator>().GetBool("Torch"))
        {
            torcia.SetActive(true);
            
        }

	}
}
