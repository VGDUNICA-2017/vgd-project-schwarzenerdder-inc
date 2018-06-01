using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintTasto : MonoBehaviour {

    /// <summary>
    /// author: feduss
    /// </summary>
    private HUDSystem hud;
    private KeyScript access_cutter;
    private KeyScript access_key;

    public void Start()
    {
        access_cutter = GameObject.FindGameObjectWithTag("Cutter").GetComponent<KeyScript>();
        access_key = GameObject.FindGameObjectWithTag("FinalKey").GetComponent<KeyScript>();
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem> ();

        if (gameObject.name.Equals("MusicaFondo")) {
            GetComponent<AudioSource>().Play();
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Serranda"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"Left Ctrl\" per abbassarti");
				hud.centralBoxEnabler(true);
            }
        }

        if (gameObject.name.Equals("chain"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				if (!access_cutter.key) {
					hud.centralBoxText ("Ti servono delle cesoie per spezzare il lucchetto");
				} else {
					hud.centralBoxText ("Premi \"E\" per spezzare il lucchetto");
				}
				hud.centralBoxEnabler (true);
            }
        }

        if (gameObject.CompareTag("FinalDoor"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!access_key.key)
                {
                    hud.centralBoxText("Ti serve una chiave per procedere");
                }
                else
                {
                    hud.centralBoxText("Premi \"E\" per usare la chiave");
                }
                hud.centralBoxEnabler(true);
            }
        }

        if (gameObject.CompareTag("Cutter"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"E\" per raccogliere le cesoie");
				hud.centralBoxEnabler (true);
            }
        }

        if (gameObject.CompareTag("FinalKey"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"E\" per raccogliere le chiavi");
				hud.centralBoxEnabler (true);
            }
        }

        if (gameObject.name.Equals("Corsa"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"Left Shit\" per correre");
				hud.centralBoxEnabler (true);
            }
        }

        if (gameObject.name.Equals("Camminata"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"W\" per avanzare");
				hud.centralBoxEnabler (true);
            }
        }

        if (gameObject.name.Equals("TorciaHint"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
				hud.centralBoxText ("Premi \"F\" per accendere e spegnere la torcia");
				hud.centralBoxEnabler (true);
                
            }
        }

        if (gameObject.name.Equals("KitmedicoHint"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere " + gameObject.name + "e premi \"K\" per usarlo");
			hud.centralBoxEnabler (true);
        }

        if (gameObject.CompareTag("Axe"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere l'ascia");
			hud.centralBoxEnabler (true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hud.centralBoxEnabler(false);
        }
    }
}
