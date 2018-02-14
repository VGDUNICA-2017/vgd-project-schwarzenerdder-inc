using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variabili per l'animazione
    private Animator animator;
    //private AnimatorStateInfo currentState;
    //static int isWalking = Animator.StringToHash("Base Layer.isWalking");
    //static int isRunning = Animator.StringToHash("Base Layer.isRunning");

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

    public float altezza_salto;
    private bool isJumping;
    private bool isOnGround;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        isOnGround = true;
        isJumping = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Ricavo gli angoli di rotazione X e Y in base allo spostamento del mouse
        Spostamento_X += Velocità_X * Input.GetAxis("Mouse X");
        Spostamento_Y -= Velocità_Y * Input.GetAxis("Mouse Y");

        //Aggiorno la rotazione del giocatore in base allo spostamento orizzontale del mouse
        transform.eulerAngles = new Vector3(0.0f, Spostamento_X, 0.0f);

        //Aggiorno la rotazione della camera in base allo spostamento vert. ed oriz. del mouse
       /* if (Spostamento_Y >= -75.0f && Spostamento_Y <= 55.0f) camera.transform.eulerAngles = new Vector3(Spostamento_Y, Spostamento_X, 0.0f);
        else if (Spostamento_Y >= 55.0f) camera.transform.eulerAngles = new Vector3(55.0f, Spostamento_X, 0.0f);
        else if (Spostamento_Y >= -75.0f) camera.transform.eulerAngles = new Vector3(-75.0f, Spostamento_X, 0.0f);*/




    }

    private void FixedUpdate() //ad ogni frame fisico
    {
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

        //Se corre e salta
        /*if (Input.GetButton("Jump") && Input.GetKey(KeyCode.LeftShift)) {
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", true);
        }*/

        //Se salta e quindi non è a terra (non è un'animazione)

        if (isJumping && !isOnGround)
        {
            isJumping = false;
            isOnGround = true;
        }

        //Se si accovaccia
        if (Input.GetButton("Crouch"))
        {
            animator.SetBool("isCrouching", true);
        }

        if (isJumping) PlayJumpSound();




        //Debug.Log("isWalking: "+animator.GetFloat("isWalking")+"; isRunning: "+ animator.GetBool("isRunning")+"; WeaponLess: "+ animator.GetBool("WeaponLess")+"; isTurning: " + animator.GetFloat("isTurning"),  this);
    }


    public void LateUpdate(){

        float salto = Mathf.Sqrt(altezza_salto * -2f * Physics.gravity.y);

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Salto: "+ salto);
            transform.position.Set(0.0f, transform.position.y+salto, 0.0f);
            isJumping = true;
            isOnGround = false;
        }

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
}
