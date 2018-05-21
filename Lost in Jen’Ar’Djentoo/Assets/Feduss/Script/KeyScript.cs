using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{


    public bool key = false;
	private HUDSystem hud;

    private GameObject ChainSpawn;
    private GameObject CutterSpawn;
    private Misc misc;
    private GameObject player;
    private bool onetime=true;

    // Use this for initialization
    void Start()
    {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUDSystem> ();
        player = GameObject.FindGameObjectWithTag("Player");
        ChainSpawn=GameObject.FindGameObjectWithTag("ChainEnemySpawn");
        CutterSpawn = GameObject.FindGameObjectWithTag("CutterEnemySpawn");
        misc = player.GetComponent<Misc>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        //Se collide col giocatore, se preme "E" e se le cesoie esistono nella scena, allora ho raccolto la "chiave", disattivo il testo assocciato alle cesoie
        // e disattivo le cesoie
        if (other.CompareTag("Player") && Input.GetButton("Open Door") && gameObject.CompareTag("Cutter"))
        {
            key = true;
            GameObject.Find("chain").GetComponent<KeyScript>().key = true;
			hud.centralBoxEnabler (false);
			hud.sideBoxEnabler (true);
			hud.sideBoxText("Hai raccolto le cesoie");
            CutterSpawn.SetActive(true);
            misc.supportFunction(gameObject);

        }

        if (other.CompareTag("Player") && Input.GetButton("Open Door") && gameObject.CompareTag("FinalKey"))
        {
            key = true;
            GameObject.FindGameObjectWithTag("FinalDoor").GetComponent<KeyScript>().key = true;
			hud.centralBoxEnabler (false);
			hud.sideBoxEnabler (true);
			hud.sideBoxText("Hai raccolto le chiavi");
            misc.supportFunction(gameObject);

        }

        //Se spezzo la catena quando ho le cesoie e apro la recinzione
        if (gameObject.name.Equals("chain"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (key && Input.GetButtonDown("Open Door") && onetime)
                {
                    gameObject.AddComponent<Rigidbody>();
                    gameObject.AddComponent<BoxCollider>();
					hud.centralBoxEnabler (false);

                    GameObject.FindGameObjectWithTag("Recinzione").GetComponent<Animator>().SetBool("Open", true);
                    ChainSpawn.SetActive(true);
                    onetime = false;
                    Destroy(GameObject.Find("chain"), 2f);
                    

                }
            }
        }

        if (gameObject.CompareTag("FinalDoor"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (key && Input.GetButtonDown("Open Door") && onetime)
                {
					hud.centralBoxEnabler (false);
                    gameObject.AddComponent<Rigidbody>();
                    gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 15f, ForceMode.Impulse);
                    onetime = false;

                    foreach (BoxCollider collider in gameObject.GetComponents<BoxCollider>())
                    {
                        if (collider.isTrigger)
                        {
                            Destroy(collider);
                        }
                    }

                    //Destroy(gameObject.GetComponent<Rigidbody>());

                }
            }
        }

        if (other.CompareTag("Player") && Input.GetButton("Open Door") && gameObject.CompareTag("FinalKey"))
        {

        }




    }
   
}
