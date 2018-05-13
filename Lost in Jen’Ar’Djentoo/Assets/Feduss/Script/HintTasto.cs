using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintTasto : MonoBehaviour {

    //public Text testo;
	private HUDSystem hud;
    private KeyScript access;

    public void Start()
    {
        access = GameObject.FindGameObjectWithTag("Cutter").GetComponent<KeyScript>();
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem> ();
        //testo = GameObject.Find("MessageBox").GetComponent<Text>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Serranda"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"Left Ctrl\" per abbassarti");
				//testo.text = "Premi \"Left Ctrl\" per abbassarti";
				hud.centralBoxEnabler(true);
				//testo.enabled = true;
            }
        }

        if (gameObject.name.Equals("chain"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				if (!access.key) {
					hud.centralBoxText ("Ti servono delle cesoie per spezzare il lucchetto");
					//testo.text = "Ti servono delle cesoie per spezzare il lucchetto";
				} else {
					hud.centralBoxText ("Premi \"E\" per spezzare il lucchetto");
					//testo.text = "Premi \"E\" per spezzare il lucchetto";
				}
				hud.centralBoxEnabler (true);
				//testo.enabled = true;
            }
        }

        if (gameObject.CompareTag("Cutter"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"E\" per raccogliere le cesoie");
                //testo.text = "Premi \"E\" per raccogliere le cesoie";
				hud.centralBoxEnabler (true);
                //testo.enabled = true;
            }
        }

        if (gameObject.name.Equals("Corsa"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"Left Shit\" per correre");
				//testo.text = "Premi \"Left Shit\" per correre";
				hud.centralBoxEnabler (true);
				//testo.enabled = true;
            }
        }

        if (gameObject.name.Equals("Camminata"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"W\" per avanzare");
                //testo.text = "Premi \"W\" per avanzare";
				hud.centralBoxEnabler (true);
				//testo.enabled = true;
            }
        }

        if (gameObject.name.Equals("TorciaHint"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"F\" per accendere e spegnere la torcia");
                //testo.text = "Premi \"F\" per accendere e spegnere la torcia";
				hud.centralBoxEnabler (true);
				//testo.enabled = true;
                GetComponent<AudioSource>().Play();
            }
        }

        if (gameObject.name.Equals("KitmedicoHint"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere " + gameObject.name + "e premi \"K\" per usarlo");
            //testo.text = "Premi \"E\" per raccogliere " + gameObject.name + "e premi \"K\" per usarlo";
			hud.centralBoxEnabler (true);
			//testo.enabled = true;
        }

        if (gameObject.CompareTag("Axe"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere l'ascia");
            //testo.text = "Premi \"E\" per raccogliere l'ascia";
			hud.centralBoxEnabler (true);
			//testo.enabled = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hud.centralBoxEnabler(false);
            //testo.enabled = false;
        }

        //Distruggo i gameobject dei consigli sui comandi quando il player esce dal loro trigger
        if (gameObject.CompareTag("Serranda") || gameObject.name.Equals("Corsa") || gameObject.name.Equals("Camminata") || gameObject.name.Equals("TorciaHint"))   
        {
            Destroy(gameObject);
        }
    }
}
