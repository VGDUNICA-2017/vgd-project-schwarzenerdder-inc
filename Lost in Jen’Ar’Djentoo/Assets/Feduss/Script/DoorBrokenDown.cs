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


        if (flag)
        {
            flag = false;

            int i = 0;
            while (i < 3)
            {
                if (!audio_source.isPlaying)
                {
                    audio_source.clip = doorKick;
                    audio_source.Play();
                    i++;
                    Debug.Log(i);
                }
            }
            if (i == 3)
            {
                rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
                audio_source.clip = doorBrokenDown;
                audio_source.Play();
            }

            
        }


    }

}

