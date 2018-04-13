using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    private Animator animator; // animator del player
    private HUDSystem hudsystem; //script del player
    public bool autoaim = false; //debug per mirare senza premere il tasto dx del mouse
    private PlaySound playsound; //script del player
    private GameObject pistola_imp; //arma impugnata dal player

    //Transform della pistola (utile per quando si mira)
    Vector3 pistol_start_angles = new Vector3(78.55801f, -77.943f, -77.22501f);
    //Vector3 pistol_end_angles = new Vector3(66.12701f, -10.069f, -6.337f);
    Vector3 pistol_end_angles = new Vector3(70.579f, -49.945f, -36.22f);

    Vector3 pistol_start_pos = new Vector3(0.2576f, -0.0756f, 0.1487f);
    Vector3 pistol_end_pos = new Vector3(0.275f, -0.048f, 0.183f);

    // Use this for initialization
    void Start () {
		
        animator=GetComponent<Animator>();
        hudsystem = GetComponent<HUDSystem>();
        playsound = GetComponent<PlaySound>();
        pistola_imp = GameObject.Find("P226 (Impugnata)");


	}
	
	// Update is called once per frame
	void Update () {

        PlayAnimations();

    }

    private void PlayAnimations()
    {
        //Gestione della camminata dritta e di lato
        animator.SetFloat("isWalking", Input.GetAxis("Vertical"));
        animator.SetFloat("isTurning", Input.GetAxis("Horizontal"));

        //Se ha raccolto la torcia, non è più senza armi, altrimenti lo è
        if (animator.GetBool("Torch")) animator.SetBool("WeaponLess", false);

        //Se avanza, riproduce il suo dei passi
        if ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            hudsystem.movingState(true);
            if (!animator.GetBool("isRunning")) playsound.PlayFootStepAudio(1);
        }
        else
        {
            hudsystem.movingState(false);
        }

        //Se corre
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            animator.SetBool("isRunning", true);
            playsound.PlayFootStepAudio(2);
        }
        else
        {
            //Di default non sta correndo
            animator.SetBool("isRunning", false);

        }


        //Se si accovaccia, aggiorno il collider...e viceversa
        if (Input.GetButton("Crouch"))
        {
            animator.SetBool("isCrouching", true);
            GetComponent<CharacterController>().height = 3.3f;
            GetComponent<CharacterController>().center = new Vector3(0f, 1.1f, 0f);


        }
        else
        {
            //Di default non è accovacciato
            animator.SetBool("isCrouching", false);
            GetComponent<CharacterController>().height = 3.8f;
            GetComponent<CharacterController>().center = new Vector3(0f, 1.59f, 0f);


        }

        //MIRA
        //Se preme il tasto dx del mouse, se non ho la torcia e se non sto correndo
        if (autoaim || Input.GetButton("Aim") && !animator.GetBool("WeaponLess") &&!animator.GetBool("Torch") && !animator.GetBool("isCrouching") && !animator.GetBool("isRunning") && !animator.GetBool("isReloading"))
        {
            animator.SetBool("isAiming", true);

            hudsystem.hudReticle(true); //attivo il reticolo di mira

            pistola_imp.transform.localPosition = pistol_end_pos;
            pistola_imp.transform.localEulerAngles = pistol_end_angles;

            

        }
        else
        {
            animator.SetBool("isAiming", false);
            pistola_imp.transform.localPosition = pistol_start_pos;
            pistola_imp.transform.localEulerAngles = pistol_start_angles;

            if (animator.GetBool("Axe")) hudsystem.hudReticle(true);
            else hudsystem.hudReticle(false);

            

        }

        //salto
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
            playsound.PlayJumpSound();
        }

    }
}
