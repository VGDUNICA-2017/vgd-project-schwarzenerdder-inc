using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{


    public bool key = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        //Se collide col giocatore, se preme "E" e se le cesoie esistono nella scena, allora ho raccolto la chiave, disattivo il testo assocciato alle cesoie
        // e disattivo le cesoie
        if (other.CompareTag("Player") && Input.GetButton("Open Door") && gameObject.CompareTag("Cutter"))
        {
            Debug.Log("sono in un log");
            key = true;
            GameObject.Find("chain").GetComponent<KeyScript>().key = true;
            GameObject.FindGameObjectWithTag("Cutter").GetComponent<HintTasto>().testo.enabled = false;
            gameObject.SetActive(false);


        }

        //Se spezzo la catena quando ho le cesoie, le aggiungo alcuni componenti per rendere l'effetto più realistico
        if (gameObject.name.Equals("chain"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (key && Input.GetButton("Open Door"))
                {
                    gameObject.AddComponent<Rigidbody>();
                    gameObject.AddComponent<BoxCollider>();
                    GameObject.Find("chain").GetComponent<HintTasto>().testo.enabled = false;

                    //Sblocco la recinzione
                    JointLimits limits = GameObject.FindGameObjectWithTag("Recinzione").GetComponent<HingeJoint>().limits;
                    limits.max = 90;                  
                
                    GameObject.FindGameObjectWithTag("Recinzione").GetComponent<HingeJoint>().limits = limits;
                    Debug.Log("Limite max rotazione recinzione: " + GameObject.FindGameObjectWithTag("Recinzione").GetComponent<HingeJoint>().limits.max);

                    Destroy(GameObject.Find("chain"), 2f);

                }
            }
        }
    }
}
