using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    private GameObject lamp;
    private GameObject light;
	// Use this for initialization
	void Start () {

        lamp = GameObject.FindGameObjectWithTag("BrokenLamp");
        light = GameObject.FindGameObjectWithTag("BrokenLight");
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(BrokenLight());
    }

    IEnumerator BrokenLight()
    {

        light.GetComponent<Light>().intensity = 0;
        lamp.SetActive(false);

        yield return new WaitForSeconds(1f);

        light.GetComponent<Light>().intensity = 10;
        lamp.SetActive(true);

    }
}
