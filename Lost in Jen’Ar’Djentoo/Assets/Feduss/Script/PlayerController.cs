using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variabili per l'animazione
    private Animator animator;
    private AnimatorStateInfo currentState;
    static int isWalking = Animator.StringToHash("Base Layer.isWalking");
    static int isRunning = Animator.StringToHash("Base Layer.isRunning");

    //Variabili per lo spostamento del giocatore
    public float Velocità_spost = 0;
    private Rigidbody rb;

    //Spostamento camera e giocatore
    public float Velocità_X = 2.0f;
    public float Velocità_Y = 1.0f;

    private float Spostamento_X = 0.0f;
    private float Spostamento_Y = 0.0f;
    public Camera camera;


    public GameObject torcia;

    //Suoni passi
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
     //Da implementare quando avremo gestito bene il salto
    // [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
   // [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
                     private AudioSource m_AudioSource;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

        //Modifico la posizione e la rotazione della torcia per metterla in mano al giocatore (non funziona)
        /*Vector3 pos_torcia = new Vector3(-22.0f, -6.0f, -3.0f);
        Vector3 rot_torcia = new Vector3(0.0f, -90.0f, -90.0f);
        torcia.transform.position = pos_torcia;
        torcia.transform.eulerAngles = rot_torcia;*/
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
        camera.transform.eulerAngles = new Vector3(Spostamento_Y, Spostamento_X, 0.0f);

    }

    private void FixedUpdate() //ad ogni frame fisico
    {
        ///////////////
        //Spostamento del giocatore
        float movementHorizontal = Input.GetAxis("Horizontal");
        float movementVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

        rb.AddForce(movement * Velocità_spost);
        //////////////



        currentState = animator.GetCurrentAnimatorStateInfo(0); //info relative allo stato attuale

        //Se avanza
        if ((Input.GetKey("w")) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetFloat("isWalking", 1.0f);
            PlayFootStepAudio();


        }    //Se corre e avanza
        else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey("w"))
        {
            animator.SetBool("isRunning", true);
          
        }
        else
        {
            animator.SetFloat("isWalking", 0.0f);

        }
        Debug.Log("isWalking: "+animator.GetFloat("isWalking")+"; isRunning: "+ animator.GetBool("isRunning"), this);
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
}
