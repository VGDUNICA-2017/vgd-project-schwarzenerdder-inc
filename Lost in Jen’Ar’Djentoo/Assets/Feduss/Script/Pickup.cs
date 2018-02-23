using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    public Text testo;
    public GameObject fin;
    private Animator animator;
    public bool flag;

    PistolScript pistol;
    PlayerMagazineHUD a;

    // Use this for initialization
    void Start() {
        testo.enabled = false;
        flag = false;
        animator = fin.GetComponent<Animator>();

        //Disattivo la parte dell'hud delle munizioni
        a=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMagazineHUD>();
        a.enabled = false;

        //Mi servirà per riprodurre il suono di pickup della pistola
        pistol = GameObject.Find("P226Stock").GetComponent<PistolScript>();
        

    }

        // Update is called once per frame
        void Update () {

        
	}

    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            testo.text = "Premi \"F\" per raccogliere "+gameObject.name;
            testo.enabled = true;
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Pickup") )
        {
            if (gameObject.name.Equals("la Torcia"))
            {
                EquipTorch();
                gameObject.SetActive(false);
                testo.enabled = false;
                
            }

            if(gameObject.name.Equals("P226"))
            {
                EquipPistol();
                gameObject.SetActive(false);
                testo.enabled = false;
               
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

    public void EquipTorch()
    {
        animator.SetBool("Torch", true);
        animator.SetBool("WeaponLess", false);
    }

    public void EquipPistol()
    {
        animator.SetBool("Torch", false);
        animator.SetBool("Pistol", true);

        //Riproduto il suono di equip della pistola e riattivo la parte dell'hud delle munizioni
        pistol.PlayEquipPistolSound();
        a.enabled = false;
    }
}
