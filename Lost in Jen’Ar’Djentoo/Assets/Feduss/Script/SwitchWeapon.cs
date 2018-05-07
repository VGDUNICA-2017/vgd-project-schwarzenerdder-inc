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
    
    private GameObject ascia_imp;
    private GameObject pistola_imp;
    private GameObject smg_imp;

    private GameObject texture_braccio1;
    private GameObject texture_braccio2;



    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        pistola_imp = GameObject.Find("P226 (Impugnata)");
        ascia_imp = GameObject.Find("l'ascia (Impugnata)");
        smg_imp = GameObject.Find("MP5 (Impugnato)");

        texture_braccio1=GameObject.FindGameObjectWithTag("HR1");
        texture_braccio2=GameObject.FindGameObjectWithTag("HR2");

    }
	
	// Update is called once per frame
	void Update () {

        SwitchWeapons();

        SetActive();

        //Quando è senza armi equipaggiate, disattivo le texture delle braccia
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("NoWeapon")){
            texture_braccio1.SetActive(false);
            texture_braccio2.SetActive(false);
        }
        else
        {
            texture_braccio1.SetActive(true);
            texture_braccio2.SetActive(true);
        }

	}

    //Da completare man mano che inseriremo le altre armi
    private void SwitchWeapons()
    {
        if (animator.GetBool("Pistol")) getPistol = true;
        if (animator.GetBool("Axe")) getAxe = true;
        if (animator.GetBool("Smg")) getSmg = true;
        // if (animator.GetBool("Shotgun")) getShotgun = true;

        if (!animator.IsInTransition(0))
        {

            //Ascia
            if (Input.GetKeyDown("1") && getAxe)
            {
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
                //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
                if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

                animator.SetBool("Axe", true);
            }

            //Pistola
            if (Input.GetKeyDown("2") && getPistol)
            {
                if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
                if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

                animator.SetBool("Pistol", true);
            }

            //Smg
            if (Input.GetKeyDown("3") && getSmg)
            {
                if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
                //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
                animator.SetBool("Smg", true);
            }

            //Shotgun
            if (Input.GetKeyDown("4") && getShotgun)
            {
                if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
                if (animator.GetBool("Smg")) animator.SetBool("Smg", false);
                //animator.SetBool("Shotgun", true);
            }

            
        }


    }

    //ATTIVAZIONE ARMI O ELEMENTI DELL'HUD (da completare con le altre armi)
    public void SetActive()
    {

        /*//TORCIA
        if (!animator.GetBool("Torch") && torcia_imp != null) torcia_imp.SetActive(false);
        else if (torcia_imp != null) torcia_imp.SetActive(true);*/

        //ASCIA
        if (!animator.GetBool("Axe") && ascia_imp != null) ascia_imp.SetActive(false);
        else if (ascia_imp != null) ascia_imp.SetActive(true);

        //PISTOLA
        if (!animator.GetBool("Pistol") && pistola_imp != null) pistola_imp.SetActive(false);
        else if (pistola_imp != null) pistola_imp.SetActive(true);

        //SMG
        if (!animator.GetBool("Smg") && smg_imp != null) smg_imp.SetActive(false);
        else if (smg_imp != null) smg_imp.SetActive(true);

    }
}
