using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    public Text testo;
    public GameObject fin;
    private Animator animator;
    public bool flag;
    private HUDSystem hudsystem;
    private InventorySystem inventario;

    // Use this for initialization
    void Start() {
        testo = GameObject.Find("MessageBox").GetComponent<Text>();
        testo.enabled = false;
        flag = false;
        animator = fin.GetComponent<Animator>();
        hudsystem = fin.GetComponent<HUDSystem>();
        inventario = fin.GetComponent<InventorySystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            testo.text = "Premi \"E\" per raccogliere "+gameObject.name;
            testo.enabled = true;
        }
    }

    public void OnTriggerStay(Collider other) {
        if (Input.GetButton("Open Door")) {
            if (gameObject.name.Equals("la Torcia")) {
                EquipTorch();
                testo.enabled = false;
				inventario.setTorcia (true);
                Destroy(gameObject);
            }

            if(gameObject.name.Equals("P226")) {
                EquipPistol();
                testo.enabled = false;
				inventario.startAmmo (0);
                Destroy(gameObject);
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

    public void EquipPistol() {
        animator.SetBool("Torch", false);
        animator.SetBool("Pistol", true);
    }
}
