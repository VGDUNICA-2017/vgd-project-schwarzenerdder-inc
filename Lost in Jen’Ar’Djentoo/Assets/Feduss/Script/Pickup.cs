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

    // Use this for initialization
    void Start() {
		hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        //testo = GameObject.Find("MessageBox").GetComponent<Text>();
        //take = GameObject.Find("MessageBoxTake").GetComponent<Text>();
        fin = GameObject.FindGameObjectWithTag("Player");
		hud.centralBoxEnabler (false);
        //testo.enabled = false;
		hud.sideBoxEnabler(false);
        //take.enabled = false;
        flag = false;
        animator = fin.GetComponent<Animator>();
        inventario = fin.GetComponent<InventorySystem>();

    }

    private void Update()
    {
        //Da completare con le altre armi
        if (gameObject.CompareTag("Ammo_9mm")) munizioni_ammobox = 8;

        //Da completare con le altre armi
        if (animator.GetBool("Pistol")) arma_attuale = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !other.gameObject.name.Equals("la Torcia (Impugnata)"))
        {
			hud.centralBoxText ("Premi \"E\" per raccogliere " + gameObject.name);
            //testo.text = "Premi \"E\" per raccogliere "+gameObject.name;
			hud.centralBoxEnabler (true);
			//testo.enabled = true;
        }

    }

    public void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("Open Door") && other.gameObject.CompareTag("Player") && !other.gameObject.name.Equals("la Torcia (Impugnata)")) {
            if (gameObject.name.Equals("la Torcia")) {
                EquipTorch();
                //animator.SetTrigger("isTaken");
				hud.centralBoxEnabler(false);
                //testo.enabled = false;
				inventario.setTorcia (true);
                fin.GetComponent<SwitchWeapon>().getTorch = true;
                Destroy(GameObject.Find("MuroInvisibile1"));

				hud.sideBoxEnabler (true);
				//take.enabled = true;
				hud.sideBoxText("Hai raccolto la torcia");
				//take.text = "Hai raccolto la torcia";
                StartCoroutine(DisableAfterSomeSeconds());
            }

            if (gameObject.name.Equals("l'ascia"))
            {
                EquipAxe();
				hud.centralBoxEnabler(false);
				//testo.enabled = false;
                //inventario.setAxe(true);
                fin.GetComponent<SwitchWeapon>().getAxe = true;
				hud.sideBoxEnabler (true);
				//take.enabled = true;
				hud.sideBoxText("Hai raccolto l'ascia");
                //take.text = "Hai raccolto l'ascia";
                StartCoroutine(DisableAfterSomeSeconds());
            }

            if(gameObject.name.Equals("P226")) {
                //EquipPistol();
                //animator.SetTrigger("isTaken");
				hud.centralBoxEnabler(false);
                //testo.enabled = false;
				inventario.startAmmo (0);
                fin.GetComponent<SwitchWeapon>().getPistol = true;

				hud.sideBoxEnabler (true);
				//take.enabled = true;
				hud.sideBoxText("Hai raccolto la P226");
                //take.text = "Hai raccolto la P226";
                StartCoroutine(DisableAfterSomeSeconds());
            }
  

            if (gameObject.CompareTag("Ammo_9mm"))
            {

				hud.sideBoxEnabler (true);
				//take.enabled = true;
				hud.sideBoxText("Hai raccolto " + munizioni_ammobox + " colpi da 9mm");
                //take.text = "Hai raccolto " + munizioni_ammobox + " colpi da 9mm";
                StartCoroutine(DisableAfterSomeSeconds());
                gameObject.GetComponent<Renderer>().enabled = false;

                munizioni_ammobox =inventario.ammoPickup(munizioni_ammobox, 0, arma_attuale);

                

                if (munizioni_ammobox == 0){
					hud.centralBoxEnabler (false);
                    //testo.enabled = false;
                    munizioni_ammobox = 8;

                    Destroy(gameObject);
                }

            }

            if (gameObject.CompareTag("FirstAid"))
            {
                if (inventario.medkitPickup())
                {
					hud.centralBoxEnabler (false);
					//testo.enabled = false;

					hud.sideBoxEnabler (true);
					//take.enabled = true;
					hud.sideBoxText("Hai raccolto un kit medico");
                    //take.text = "Hai raccolto un kit medico";
                    StartCoroutine(DisableAfterSomeSeconds());
                }
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
			hud.centralBoxEnabler (false);
			//testo.enabled = false;
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
        animator.SetBool("Pistol", true);
    }

    IEnumerator DisableAfterSomeSeconds()
    {
        yield return new WaitForSeconds(2f);
       
		hud.centralBoxEnabler (false);
        //take.enabled = false;
		hud.sideBoxEnabler (false);
        //testo.enabled = false;
        Destroy(gameObject);
    }
}
