using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    public Text testo;
    public GameObject fin;
    private Animator animator;
    public bool flag;

    // Use this for initialization
    void Start() {
        testo.enabled = false;
        flag = false;
        animator = fin.GetComponent<Animator>();
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
        if (Input.GetButton("Pickup"))
        {
            //Debug.Log("Hai raccolto l'arma :D");
            gameObject.SetActive(false);
            testo.enabled = false;
            EquipTorch();

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
}
