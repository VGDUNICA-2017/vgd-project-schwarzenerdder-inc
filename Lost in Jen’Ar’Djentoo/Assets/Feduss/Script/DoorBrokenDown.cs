using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBrokenDown : MonoBehaviour {

    /// <summary>
    /// author: feduss
    /// </summary>
    public float force;
    public bool flag = false;
    public bool forward = false; //bool che serve per orientare la forza applicata al rigidbody del gameobject

    private AudioSource audio_source;
    public AudioClip doorKick;
    public AudioClip doorBrokenDown;

	// Use this for initialization
	void Start () {

        audio_source = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

    }

    //Funzione che reimposta l'evento durante il caricamento di una partita
    public void brokenOnLoading()
    {
        if (forward) GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
        else GetComponent<Rigidbody>().AddForce(Vector3.right * force, ForceMode.Impulse);

        foreach (BoxCollider collider in gameObject.GetComponents<BoxCollider>())
        {
            if (collider.isTrigger)
            {
                Destroy(collider);
            }
        }
    }
        

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audio_source.clip = doorKick;
            audio_source.Play();

            audio_source.clip = doorBrokenDown;
            audio_source.Play();
            gameObject.AddComponent<Rigidbody>();

            //Oriento la forza applicata
            if(forward) GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
            else GetComponent<Rigidbody>().AddForce(Vector3.right * force, ForceMode.Impulse);

            foreach (BoxCollider collider in gameObject.GetComponents<BoxCollider>())
            {
                if (collider.isTrigger)
                {
                    Destroy(collider);
                }
            }
        }
    }


}

