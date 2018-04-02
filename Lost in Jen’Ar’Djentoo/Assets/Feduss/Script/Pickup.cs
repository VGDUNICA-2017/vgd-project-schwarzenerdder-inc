using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    private Text testo;
    private Text take;
    public GameObject fin;
    private Animator animator;
    public bool flag;
    private HUDSystem hudsystem;
    private InventorySystem inventario;

    int arma_attuale=-1;
    int munizioni_ammobox=-1; //numero di colpi presenti in un ammobox

    // Use this for initialization
    void Start() {
        testo = GameObject.Find("MessageBox").GetComponent<Text>();
        take = GameObject.Find("MessageBoxTake").GetComponent<Text>();
        testo.enabled = false;
        take.enabled = false;
        flag = false;
        animator = fin.GetComponent<Animator>();
        hudsystem = fin.GetComponent<HUDSystem>();
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
            testo.text = "Premi \"E\" per raccogliere "+gameObject.name;
            testo.enabled = true;
        }

    }

    public void OnTriggerStay(Collider other) {
        if (Input.GetButtonDown("Open Door") && other.gameObject.CompareTag("Player") && !other.gameObject.name.Equals("la Torcia (Impugnata)")) {
            if (gameObject.name.Equals("la Torcia")) {
                EquipTorch();
                //animator.SetTrigger("isTaken"); 
                testo.enabled = false;
				inventario.setTorcia (true);
                fin.GetComponent<SwitchWeapon>().getTorch = true;
                Destroy(GameObject.Find("MuroInvisibile1"));

                take.enabled = true;
                take.text = "Hai raccolto la torcia";
                StartCoroutine(DisableAfterSomeSeconds());
            }

            if (gameObject.name.Equals("l'ascia"))
            {
                EquipAxe();
                testo.enabled = false;
                //inventario.setAxe(true);
                fin.GetComponent<SwitchWeapon>().getAxe = true;
                take.enabled = true;
                take.text = "Hai raccolto l'ascia";
                StartCoroutine(DisableAfterSomeSeconds());
            }

            if(gameObject.name.Equals("P226")) {
                //EquipPistol();
                //animator.SetTrigger("isTaken");
                testo.enabled = false;
				inventario.startAmmo (0);
                fin.GetComponent<SwitchWeapon>().getPistol = true;

                take.enabled = true;
                take.text = "Hai raccolto la P226";
                StartCoroutine(DisableAfterSomeSeconds());
            }
  

            if (gameObject.CompareTag("Ammo_9mm"))
            {

                take.enabled = true;
                take.text = "Hai raccolto " + munizioni_ammobox + " colpi da 9mm";
                StartCoroutine(DisableAfterSomeSeconds());
                gameObject.GetComponent<Renderer>().enabled = false;

                munizioni_ammobox =inventario.ammoPickup(munizioni_ammobox, 0, arma_attuale);

                

                if (munizioni_ammobox == 0){
                    testo.enabled = false;
                    munizioni_ammobox = 8;

                    Destroy(gameObject);
                }

            }

            if (gameObject.CompareTag("FirstAid"))
            {
                if (inventario.medkitPickup())
                {
                    testo.enabled = false;

                    take.enabled = true;
                    take.text = "Hai raccolto un kit medico";
                    StartCoroutine(DisableAfterSomeSeconds());
                }
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
           testo.enabled = false;
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
       
        take.enabled = false;
        testo.enabled = false;
        Destroy(gameObject);
    }
}
