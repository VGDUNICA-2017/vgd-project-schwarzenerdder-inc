using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public GameObject fire_fx;

    // Use this for initialization
    void Start () {

        Instantiate(fire_fx, transform);



        //Aggiungo la forza dello sparo al proiettile
        transform.rotation = GameObject.Find("P226 (Impugnata)").transform.rotation;
        GetComponent<Rigidbody>().AddForce(Vector3.forward*20f, ForceMode.Impulse);

        //Inizialmente il proiettile viene istanziato come figlio dell'arma...poi la parentela viene annullata,
        //per far si che il proiettile, una volta sparato, non segua i movimenti del parent originale
        transform.SetParent(null);

        //Distrugge il colpo se non colpisce niente
        if(gameObject!=null) Destroy(gameObject, 5f);
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Colpito");
        Destroy(gameObject);
    }

    }
