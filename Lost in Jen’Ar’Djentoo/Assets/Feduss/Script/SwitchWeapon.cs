using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour {

    private Animator animator;

    //flag per il possesso delle armi
    public bool getTorch = false;
    public bool getAxe = false;
    public bool getPistol = false;
    public bool getShotgun = false;
    public bool getSmg = false;

    private GameObject torcia_imp;
    private GameObject ascia_imp;
    private GameObject pistola_imp;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        pistola_imp = GameObject.Find("P226 (Impugnata)");
        torcia_imp = GameObject.Find("la Torcia (Impugnata)");
        ascia_imp = GameObject.Find("l'ascia (Impugnata)");
    }
	
	// Update is called once per frame
	void Update () {

        SwitchWeapons();

        SetActive();

	}

    //Da completare man mano che inseriremo le altre armi
    private void SwitchWeapons()
    {

        //Debug
        if (animator.GetBool("Torch")) getTorch = true;
        if (animator.GetBool("Pistol")) getPistol = true;
        if (animator.GetBool("Axe")) getAxe = true;
        // if (animator.GetBool("Smg")) getSmg = true;
        // if (animator.GetBool("Shotgun")) getShotgun = true;

        //Torcia
        if (Input.GetKeyDown("1") && getTorch)
        {
            if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

            animator.SetBool("Torch", true);
        }

        //Ascia
        if (Input.GetKeyDown("2") && getAxe)
        {
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

            animator.SetBool("Axe", true);
        }

        //Pistola
        if (Input.GetKeyDown("3") && getPistol)
        {
            if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);
            animator.SetBool("Pistol", true);
        }

        //Shotgun
        if (Input.GetKeyDown("5") && getShotgun)
        {
            if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);
            //animator.SetBool("Shotgun", true);
        }

        //Smg
        if (Input.GetKeyDown("4") && getSmg)
        {
            if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //animator.SetBool("Smg", true);
        }


    }

    //ATTIVAZIONE ARMI O ELEMENTI DELL'HUD (da completare con le altre armi)
    public void SetActive()
    {

        //TORCIA
        if (!animator.GetBool("Torch") && torcia_imp != null) torcia_imp.SetActive(false);
        else if (torcia_imp != null) torcia_imp.SetActive(true);

        //ASCIA
        //TORCIA
        if (!animator.GetBool("Axe") && ascia_imp != null) ascia_imp.SetActive(false);
        else if (ascia_imp != null) ascia_imp.SetActive(true);

        //PISTOLA
        if (!animator.GetBool("Pistol") && pistola_imp != null) pistola_imp.SetActive(false);
        else if (pistola_imp != null) pistola_imp.SetActive(true);

    }
}
