using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    //private Text testo;
    //private Text take;
    private GameObject fin;
    private Animator animator;
	private HUDSystem hud;
    public bool flag;
    private InventorySystem inventario;

    int arma_attuale=-1;
    int munizioni_ammobox=-1; //numero di colpi presenti in un ammobox

    private GameObject AxeSpawn;
    private GameObject PistolSpawn;
    private GameObject ChainSpawn;
    private GameObject CutterSpawn;
    private GameObject SmgSpawn;

    private bool dontenter = false;

    // Use this for initialization
    void Start() {
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        fin = GameObject.FindGameObjectWithTag("Player");
		hud.centralBoxEnabler (false);
		hud.sideBoxEnabler(false);
        flag = false;
        animator = fin.GetComponent<Animator>();
        inventario = fin.GetComponent<InventorySystem>();

        AxeSpawn=GameObject.FindGameObjectWithTag("AxeEnemySpawn");
        PistolSpawn=GameObject.FindGameObjectWithTag("PistolEnemySpawn");
        ChainSpawn = GameObject.FindGameObjectWithTag("ChainEnemySpawn");
        CutterSpawn = GameObject.FindGameObjectWithTag("CutterEnemySpawn");
        //SmgSpawn = GameObject.FindGameObjectWithTag("SmgEnemySpawn");





    }

    private void Update()
    {
        //Da completare con le altre armi
        if (gameObject.CompareTag("Ammo_9mm")) munizioni_ammobox = 8;

        //Da completare con le altre armi
        if (animator.GetBool("Pistol")) arma_attuale = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere " + gameObject.name);
			hud.centralBoxEnabler (true);
            
        }

    }

    public void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("Open Door") && other.gameObject.CompareTag("Player")) {
            if (gameObject.name.Equals("la Torcia")) {
                EquipTorch();
				hud.centralBoxEnabler(false);
				inventario.setTorcia (true);
                fin.GetComponent<SwitchWeapon>().getTorch = true;
                Destroy(GameObject.Find("MuroInvisibile1"));
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto la torcia");

                AxeSpawn.SetActive(false);
                PistolSpawn.SetActive(false);
                ChainSpawn.SetActive(false);
                CutterSpawn.SetActive(false);

                StartCoroutine(DisableAfterSomeSeconds());
            }

            if (gameObject.name.Equals("l'ascia"))
            {
                EquipAxe();
				hud.centralBoxEnabler(false);
                inventario.setAscia(true);
                fin.GetComponent<SwitchWeapon>().getAxe = true;
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto l'ascia");
                AxeSpawn.SetActive(true);
                StartCoroutine(DisableAfterSomeSeconds());
            }

            if(gameObject.name.Equals("P226") && !dontenter) {
                EquipPistol();
				hud.centralBoxEnabler(false);
				inventario.startAmmo (0);
                fin.GetComponent<SwitchWeapon>().getPistol = true;
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto la P226");
                PistolSpawn.SetActive(true);
                dontenter = true;
                StartCoroutine(DisableAfterSomeSeconds());
                
            }

            if (gameObject.name.Equals("MP5") && !dontenter)
            {
                EquipSmg();
                hud.centralBoxEnabler(false);
                inventario.startAmmo(0);
                fin.GetComponent<SwitchWeapon>().getSmg = true;
                hud.sideBoxEnabler(true);
                hud.sideBoxText("Hai raccolto l'MP5");
                SmgSpawn.SetActive(true);
                dontenter = true;
                StartCoroutine(DisableAfterSomeSeconds());
            }


                if (gameObject.CompareTag("Ammo_9mm") && !dontenter)
            {
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto " + munizioni_ammobox + " colpi da 9mm");
                dontenter = true;
                munizioni_ammobox = inventario.ammoPickup(munizioni_ammobox, 0, arma_attuale);
                StartCoroutine(DisableAfterSomeSeconds());

                

                if (munizioni_ammobox == 0){
					hud.centralBoxEnabler (false);
                    munizioni_ammobox = 8;
                    Destroy(gameObject);
                }

            }

            if (gameObject.CompareTag("FirstAid") && !dontenter)
            {
                if (inventario.medkitPickup())
                {
					hud.centralBoxEnabler (false);
					hud.sideBoxEnabler (true);
					hud.sideBoxText("Hai raccolto un kit medico");
                    dontenter = true;
                    StartCoroutine(DisableAfterSomeSeconds());
                }
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
			hud.centralBoxEnabler (false);
        }
    }

    public void EquipTorch() {
        animator.SetBool("Torch", true);
        animator.SetBool("WeaponLess", false);
    }

    public void EquipAxe()
    {
        animator.SetBool("Torch", false);
        animator.SetBool("Axe", true);
    }

    public void EquipPistol() {
        animator.SetBool("Torch", false);
        animator.SetBool("Axe", false);
        animator.SetBool("Pistol", true);
    }

    public void EquipSmg()
    {
            animator.SetBool("Torch", false);
            animator.SetBool("Axe", false);
            animator.SetBool("Pistol", false);
            animator.SetBool("Smg", true);
        }

        IEnumerator DisableAfterSomeSeconds()
    {
        yield return new WaitForSeconds(2f);
       
		hud.centralBoxEnabler (false);
		hud.sideBoxEnabler (false);
        dontenter = false;
        Destroy(gameObject);
    }
}
