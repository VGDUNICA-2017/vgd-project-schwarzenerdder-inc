using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShot : MonoBehaviour {

    public int gunDamage = 1; //danno dell'arma
    public float fireRate = .25f; //tempo tra uno sparo e l'altro..nel nostro caso, determinerà la durante dell'animazione di sparo (più è alto, più sparera velocemente)

    public float weaponRange = 50f; //gittata dell'arma
    public float hitForce = 100f; //forza impressa dal colpo durante la collisione
  
    public Transform gunEnd; //transform del gameobject vuoto, dal quale partirà il raycast

    private Camera tpsCam; //serve per sapere dove il player sta mirando

    //debug
    private LineRenderer laserLine;

    // Use this for initialization
    void Start () {

        laserLine = GetComponent<LineRenderer>();
        tpsCam = GameObject.Find("Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1"))
        {
            laserLine.enabled = true;
            

        }
        else laserLine.enabled = false;

        Vector3 rayOrigin = tpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0)); //centro della camera
        RaycastHit hit; //variabile per la memorizazzione delle info sull'oggetto colpito

        laserLine.SetPosition(0, gunEnd.position); //setto la posizione iniziale del raggio

        //cast del ray: rayOrigin: 
        //-origine del ray (il centro del camera)
        //-direzione del ray (il forward della camera
        //-info sull'oggetto colpito
        //-gittata raggio
        if (Physics.Raycast(rayOrigin, tpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition (1, hit.point); //se colpisco qualcosa, la fine del raggio nell'oggetto colpito
        }
        else
        {
            laserLine.SetPosition(1, tpsCam.transform.forward * weaponRange); //altrimenti all'infinito, sino al range definito
        }

    }
}
