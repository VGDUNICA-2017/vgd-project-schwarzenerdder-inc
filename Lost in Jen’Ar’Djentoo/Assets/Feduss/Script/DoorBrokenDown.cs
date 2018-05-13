using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBrokenDown : MonoBehaviour {

    private Rigidbody rb;
    public float force;
    public bool flag = false;

    private AudioSource audio_source;
    public AudioClip doorKick;
    public AudioClip doorBrokenDown;

	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        audio_source = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

    }

        

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            //Da sistemare (deve riprodurre il suono del calcio per 2/3 volte e poi buttare giù la porta (con un altro suono)
            audio_source.clip = doorKick;
            audio_source.Play();

            audio_source.clip = doorBrokenDown;
            audio_source.Play();
            rb.AddForce(Vector3.right * force, ForceMode.Impulse);
            Destroy(gameObject, 5);
        }
    }


}

