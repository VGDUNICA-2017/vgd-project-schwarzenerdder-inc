using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintTasto : MonoBehaviour {

    public Text testo;

    private KeyScript access;

    public void Start()
    {
        access = GameObject.FindGameObjectWithTag("Cutter").GetComponent<KeyScript>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Serranda"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                testo.text = "Premi \"Left Ctrl\" per abbassarti";
                testo.enabled = true;
            }
        }

        if (gameObject.name.Equals("chain"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!access.key) testo.text = "Ti servono delle cesoie per spezzare il lucchetto";
                else testo.text = "Premi \"E\" per spezzare il lucchetto";
                testo.enabled = true;
            }
        }

        if (gameObject.CompareTag("Cutter"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                testo.text = "Premi \"E\" per raccogliere le cesoie";
                testo.enabled = true;
            }
        }




    }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                testo.enabled = false;
            }

        }
}
