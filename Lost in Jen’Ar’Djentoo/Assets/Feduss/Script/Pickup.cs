using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    //private Text testo;
    //private Text take;
    private GameObject player;
    private Animator animator;
	private HUDSystem hud;
    private InventorySystem inventario;

    private int arma_attuale=-1;
    private int munizioni_ammobox=-1; //numero di colpi presenti in un ammobox
    public int start_ammo=-1; //numero di colpi presenti inizialmente nell'ammobox


    private GameObject AxeSpawn;
    private GameObject PistolSpawn;
    private GameObject ChainSpawn;
    private GameObject CutterSpawn;
    private GameObject SmgSpawn;
    private Misc misc;
    private bool onetime = true;

    // Use this for initialization
    void Start() {
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
		hud.centralBoxEnabler (false);
		hud.sideBoxEnabler(false);
        animator = player.GetComponent<Animator>();
        inventario = player.GetComponent<InventorySystem>();

        AxeSpawn=GameObject.FindGameObjectWithTag("AxeEnemySpawn");
        PistolSpawn=GameObject.FindGameObjectWithTag("PistolEnemySpawn");
        ChainSpawn = GameObject.FindGameObjectWithTag("ChainEnemySpawn");
        CutterSpawn = GameObject.FindGameObjectWithTag("CutterEnemySpawn");
        SmgSpawn = GameObject.FindGameObjectWithTag("SmgEnemySpawn");

        munizioni_ammobox = start_ammo;

        misc = player.GetComponent<Misc>();







    }

    private void Update()
    {
        if (animator.GetBool("Pistol")) arma_attuale = 0;
        if (animator.GetBool("Smg")) arma_attuale = 2;
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
            if (gameObject.name.Equals("la Torcia") && onetime) {
                onetime = false;
                EquipTorch();
				hud.centralBoxEnabler(false);
				inventario.setTorcia (true);
                player.GetComponent<SwitchWeapon>().getTorch = true;
                Destroy(GameObject.Find("MuroInvisibile1"));
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto la torcia");

                AxeSpawn.SetActive(false);
                PistolSpawn.SetActive(false);
                ChainSpawn.SetActive(false);
                CutterSpawn.SetActive(false);
                SmgSpawn.SetActive(false);

                misc.supportFunction(gameObject);
            }

            if (gameObject.name.Equals("l'ascia") && onetime)
            {
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
                if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

                animator.SetBool("Axe", true);

                onetime = false;
				hud.centralBoxEnabler(false);
                inventario.setAscia(true);
                player.GetComponent<SwitchWeapon>().getAxe = true;
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto l'ascia (Tasto 1)");
                AxeSpawn.SetActive(true);
                misc.supportFunction(gameObject);
            }

            if(gameObject.name.Equals("P226") && onetime) {

                //Equipaggio l'arma appena raccolta
                if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

                inventario.changeWeaponHUD(0);
                animator.SetBool("Pistol", true);

                onetime = false;
				hud.centralBoxEnabler(false);
				inventario.startAmmo (0);
                player.GetComponent<SwitchWeapon>().getPistol = true;
				hud.sideBoxEnabler (true);
				hud.sideBoxText("Hai raccolto la P226 (Tasto 2)");
                PistolSpawn.SetActive(true);

                misc.supportFunction(gameObject);
                
            }

            if (gameObject.name.Equals("MP5") && onetime)
            {
                if (animator.GetBool("Axe")) animator.SetBool("Axe", false);
                if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
                if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);

                inventario.changeWeaponHUD(2);
                animator.SetBool("Smg", true);

                onetime = false;
                hud.centralBoxEnabler(false);
                inventario.startAmmo(2);
                player.GetComponent<SwitchWeapon>().getSmg = true;
                hud.sideBoxEnabler(true);
                hud.sideBoxText("Hai raccolto l'MP5 (Tasto 3)");
                SmgSpawn.SetActive(true);

                misc.supportFunction(gameObject);
            }


            if ((gameObject.CompareTag("Ammo_9mm") || gameObject.CompareTag("Ammo_smg")) && onetime)
            {
				hud.sideBoxEnabler (true);
                if (gameObject.CompareTag("Ammo_9mm"))
                {
                    hud.sideBoxText("Hai raccolto " + munizioni_ammobox + " colpi per la pistola");
                    munizioni_ammobox = inventario.ammoPickup(munizioni_ammobox, 0, arma_attuale);
                }
                if (gameObject.CompareTag("Ammo_smg"))
                {
                    hud.sideBoxText("Hai raccolto " + munizioni_ammobox + " colpi per l'mp5");
                    munizioni_ammobox = inventario.ammoPickup(munizioni_ammobox, 2, arma_attuale);
                }
                onetime = false;
                
                

                

                if (munizioni_ammobox == 0){
					hud.centralBoxEnabler (false);
                    munizioni_ammobox = start_ammo;
                    misc.supportFunction(gameObject);
                }

            }

            if (gameObject.CompareTag("FirstAid") && onetime)
            {
                if (inventario.medkitPickup())
                {
					hud.centralBoxEnabler (false);
					hud.sideBoxEnabler (true);
					hud.sideBoxText("Hai raccolto un kit medico");
                    misc.supportFunction(gameObject);
                    onetime = false;
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
    }

    
}
