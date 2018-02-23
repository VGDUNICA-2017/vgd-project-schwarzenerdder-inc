using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variabili per l'animazione
    private Animator animator;

    //Spostamento camera e giocatore
    public float Velocità_X = 2.0f;
    public float Velocità_Y = 1.0f;

    private float Spostamento_X = 0.0f;
    private float Spostamento_Y = 0.0f;
    public Camera camera;


    public GameObject torcia;

    //Suoni passi
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

    private AudioSource m_AudioSource;

    private bool isOnGround=false;

    private Rigidbody rb;

    private CapsuleCollider collider_fin;

    private float prec_collider_center;
    private float prec_collider_height;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        collider_fin = GetComponent<CapsuleCollider>();

        prec_collider_center = collider_fin.center.y;
        prec_collider_height = collider_fin.height;

    }

    // Update is called once per frame
    void Update()
    {

        if (!(animator.GetCurrentAnimatorStateInfo(0).IsName("Picking")) || !(animator.GetCurrentAnimatorStateInfo(1).IsName("Reload")))
        {
            //Ricavo gli angoli di rotazione X e Y in base allo spostamento del mouse
            Spostamento_X += Velocità_X * Input.GetAxis("Mouse X");
            Spostamento_Y -= Velocità_Y * Input.GetAxis("Mouse Y");

            //Aggiorno la rotazione del giocatore in base allo spostamento orizzontale del mouse
            transform.eulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);
        }

        PlayAnimation();





    }


    public void LateUpdate(){

        float jump = Input.GetAxis("Jump");
        float jumpSpeed=5f;

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
           
            
            Vector3 jumpVector = new Vector3(0.0f, jump * jumpSpeed, 0.0f);
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }

    }

    private void PlayAnimation() {

        //Gestione della camminata dritta e di lato
        animator.SetFloat("isWalking", Input.GetAxis("Vertical"));
        animator.SetFloat("isTurning", Input.GetAxis("Horizontal"));

        //Se ha raccolto la torcia, non è più senza armi, altrimenti lo è
        if (animator.GetBool("Torch")) animator.SetBool("WeaponLess", false);
        else animator.SetBool("WeaponLess", true);

        //Di default non sta prendendo niente
        animator.SetBool("isTaken", false);

        //Di default non sta correndo
        animator.SetBool("isRunning", false);

        //Di default non sta saltando
        //animator.SetBool("isJumping", false);

        //Di default non è accovacciato
        animator.SetBool("isCrouching", false);

        //Se avanza, riproduce il suo dei passi
        if ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            PlayFootStepAudio();
        }

        //Se prende un oggetto, allora aggiorno isTaken
        if (Input.GetButton("Pickup"))
        {
            animator.SetBool("isTaken", true);
        }

        //Se corre
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
        }


        //Se si accovaccia
        if (Input.GetButton("Crouch"))
        {
            animator.SetBool("isCrouching", true);
            collider_fin.center = new Vector3(0f, 0.95f, 0f);
            collider_fin.height = 2.55f;
        }
        else
        {
            collider_fin.center = new Vector3(0f, prec_collider_center, 0f);
            collider_fin.height = prec_collider_height;
        }


        //Inizialmente non ho armi..quindi non ho la parte di hud sulla pistola
        PlayerMagazineHUD pmHUD= GetComponent<PlayerMagazineHUD>();
        GameObject ShotsUI =GameObject.FindGameObjectWithTag("ShotsUI");
        pmHUD.enabled = false;

        if (animator.GetBool("Pistol"))
        {
            if (GameObject.Find("la Torcia (Impugnata)")) GameObject.Find("la Torcia (Impugnata)").SetActive(false);

            ShotsUI.SetActive(true);
            pmHUD.enabled = true;


        }

        animator.SetBool("isReloading", false);
        animator.SetBool("isFiring", false);

    }

    private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        //m_NextStep = m_StepCycle + .5f;
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

   

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terreno"))
        {
            isOnGround = true;
            PlayLandingSound();
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Terreno"))
        {
            isOnGround = false;
            PlayJumpSound();
        }
    }
}
