using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    private Animator animator; // animator del player
    private HUDSystem hudsystem; //script del player
    public bool autoaim = false; //debug per mirare senza premere il tasto dx del mouse
    private PlaySound playsound; //script del player
    private GameObject pistola_imp; //arma impugnata dal player

    private float crouch_pos;
    private float stand_pos;

    public float crouch_speed;
    public float crouch_offset;

    // Use this for initialization
    void Start () {
		
        animator=GetComponent<Animator>();
        hudsystem = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        playsound = GetComponent<PlaySound>();
        pistola_imp = GameObject.Find("P226 (Impugnata)");


        stand_pos = transform.position.y;
        crouch_pos = stand_pos - crouch_offset;

    }


    // Update is called once per frame
    void Update () {

        PlayAnimations();

    }

    private void PlayAnimations()
    {
        //Gestione della camminata dritta e di lato
        animator.SetFloat("Speed", Input.GetAxis("Vertical") + Input.GetAxis("Horizontal"));
        //animator.SetFloat("isTurning", Input.GetAxis("Horizontal"));

        //Se ha raccolto la torcia, non è più senza armi, altrimenti lo è
        //if (animator.GetBool("Torch")) animator.SetBool("WeaponLess", false);

        //Se avanza, riproduce il suo dei passi
        if ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            hudsystem.movingState(true);
            if (!animator.GetBool("Run")) playsound.PlayFootStepAudio(1);
        }
        else
        {
            hudsystem.movingState(false);
        }

        //Se corre
        if (Input.GetKey(KeyCode.LeftShift) && ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0)))
        {
            animator.SetBool("Run", true);
            playsound.PlayFootStepAudio(2);
        }
        else
        {
            //Di default non sta correndo
            animator.SetBool("Run", false);

        }


        //Se si accovaccia, aggiorno il collider (da problemi)
        /*if (Input.GetButton("Crouch"))
        {
            animator.SetBool("Crouch", true);

            

            //Modifico il collider quando si accovaccia
            GetComponent<CharacterController>().height = 80f;
            GetComponent<CharacterController>().center = new Vector3(0f, -29f, 7f);

            if ((transform.position.y - crouch_pos) > 0.1f)
            {
                //Accovacciamento in corso
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, crouch_pos, Time.deltaTime * crouch_speed), transform.position.z);
            }
            else
            {
                //Fine dell'accovacciamento
                transform.position = new Vector3(transform.position.x, crouch_pos, transform.position.z);

            }


        }
        else
        {
            //Di default non è accovacciato
            animator.SetBool("Crouch", false);

            if ((stand_pos - transform.position.y) > 0.1f)
            {
                //Rialzamento in corso
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, stand_pos, Time.deltaTime * crouch_speed),
         transform.position.z);
            }
            else
            {
                //Posizione normale
                transform.position = new Vector3(transform.position.x, stand_pos, transform.position.z);

                stand_pos = transform.position.y;
                crouch_pos = stand_pos - crouch_offset;
            }

            GetComponent<CharacterController>().height = 100f;
            GetComponent<CharacterController>().center = new Vector3(0f, -39, 7f);


        }
        */

        //MIRA
        //Se preme il tasto dx del mouse, se non ho la torcia e se non sto correndo
        if (Input.GetButton("Aim") && !animator.GetBool("Torch") && !animator.GetBool("Run") && !animator.GetBool("Reload"))
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
