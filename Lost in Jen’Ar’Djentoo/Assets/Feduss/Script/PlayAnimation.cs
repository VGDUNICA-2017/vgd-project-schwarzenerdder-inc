using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    /// <summary>
    /// author: feduss
    /// </summary>
    /// 
    private Animator animator; // animator del player
    private HUDSystem hudsystem; //script del player
    private PlaySound playsound; //script del player

    public static bool snow=true; //true=a contatto con il terreno innevato
    

    // Use this for initialization
    void Start () {
		
        animator=GetComponent<Animator>();
        hudsystem = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        playsound = GetComponent<PlaySound>();
    }


    // Update is called once per frame
    void Update () {

        PlayAnimations();

    }

    private void PlayAnimations()
    {
        //Gestione della camminata dritta e di lato
        animator.SetFloat("Speed", Input.GetAxis("Vertical") + Input.GetAxis("Horizontal"));

        //Se avanza, riproduce il suo dei passi
        if ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            hudsystem.movingState(true); //Setta il reticolo a seconda del movimento o meno
            if (!animator.GetBool("Run") && snow) playsound.PlayFootStepAudioSnow(1);
            else if (!animator.GetBool("Run") && !snow) playsound.PlayFootStepAudioIndoor(1);
        }
        else
        {
            hudsystem.movingState(false);
        }

        //Se corre
        if (Input.GetKey(KeyCode.LeftShift) && ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0)))
        {
            animator.SetBool("Run", true);
            if (snow) playsound.PlayFootStepAudioSnow(2);
            else if (!snow) playsound.PlayFootStepAudioIndoor(2);
        }
        else
        {
            //Di default non sta correndo
            animator.SetBool("Run", false);

        }

        

        //MIRA
        //Se preme il tasto dx del mouse, se ha la pistola o il mitra
        if (Input.GetButton("Aim") && (animator.GetBool("Pistol") || animator.GetBool("Smg")) && !animator.GetBool("Run") && !animator.GetBool("Reload"))
        {
            animator.SetBool("Aim", true);
        }
        else
        {
            animator.SetBool("Aim", false);
        }

        //Attivo o disattivo il reticolo di mira in base all'arma
        if (animator.GetBool("Pistol") || animator.GetBool("Smg")) hudsystem.reticleEnabler(true);
        else hudsystem.reticleEnabler(false);


    }
}
