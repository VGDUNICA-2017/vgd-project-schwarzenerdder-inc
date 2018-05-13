using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour {

    private GameObject shutter1;
    private GameObject boss_door1;
    private GameObject boss1;
    public bool onetime = true;
    private bool boss_death = false;

	// Use this for initialization
	void Start () {
        shutter1 = GameObject.FindGameObjectWithTag("Serranda");
        boss_door1 = GameObject.Find("door_2");
        boss1 = GameObject.FindGameObjectWithTag("MiniBoss");
        if(boss1!=null) boss1.SetActive(false);



    }
	
	// Update is called once per frame
	void Update () {

        //Se ho sconfitto il miniboss, apro la porta di uscita dalla zona
        if (boss1 != null && boss1.GetComponent<Boss1Controller>().health == 0)
        {
            boss_door1.GetComponent<Animator>().SetTrigger("Boss dies");
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        //Chiusura serranda
        if (other.CompareTag("Player") && gameObject.name.Equals("shutter1"))
        {
            shutter1.GetComponent<Animator>().SetTrigger("Close");
        }

        //Chiudo la porta quando mi ci avvicino ad essa (è la porta di uscita dalla mini boss fight)
        if (other.CompareTag("Player") && gameObject.name.Equals("boss_door") && onetime)
        {
            boss_door1.GetComponent<Animator>().SetTrigger("Close");
            onetime = false;
            boss1.SetActive(true);
        }

    }
}
