using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variabili per l'animazione
    private Animator animator;

    //Spostamento giocatore
    public float Velocità_X = 2.0f; //Velocità di spostamento orizzontale
    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_X = 0.0f;
    private float Spostamento_Y = 0.0f;

    //Variabili spostamento rotazione braccia durante mira
    private float pos_sx= 11.2f; //offset per il braccio sx
    private float pos_dx= 4.14f; //offset per il braccio dx
    private float pos_dsx_up= -5.2f; //offset per l'altezza delle braccia quando miri
    public float pos_dx_down; //offset per l'altezza del braccio dx quando non miri

    //Variabile dove salvo la rotazione verticale delle braccia e della testa (vedi lateupdate)
    private float posX;
    private float posY;

    //Suoni passi
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    private AudioSource m_AudioSource;


    //Variabile per capire se il giocatore non sta saltando
    private bool isOnGround=false;


    //Corpo rigido di fin
    private Rigidbody rb;

    //Capsule collider di fin e variabili per salvare il suo collider (utile per quando si accovaccia)
    private CapsuleCollider collider_fin;
    private float prec_collider_center;
    private float prec_collider_height;

    private GameObject MainCamera;

    //Variabili di salvataggio oggetti/armi equipaggiati e elementi dell'hud (eventualmente da nascondere per qualche motivo)
    private GameObject torcia_imp;
    private GameObject pistola_imp;
    private GameObject testa;
    private GameObject braccio_sx;
    private GameObject braccio_dx;

    //Fov della camera (utile per quando si mira)
    int start_fov = 90;
    int end_fov = 35;

    //Transform della pistola (utile per quando si mira)
    Vector3 pistol_start_angles = new Vector3(93.73999f, -333.442f, -353.222f);
    Vector3 pistol_end_angles = new Vector3(112.066f, 192.071f, 184.422f);

    Vector3 pistol_end_pos = new Vector3(0.131f, -0.016f, 0.053f);
    Vector3 pistol_start_pos = new Vector3(0.1356f, -0.0514f, 0.0475f);

    //Rotazione della torcia (cambia quando ti accovacci)
    private Vector3 torcia_start_angles= new Vector3(0.005f, 45f, 75f);
    private Vector3 torcia_end_angles= new Vector3(0.005f, 45f, 50f);

    private HUDSystem hudsystem;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        collider_fin = GetComponent<CapsuleCollider>();
        //Salvo alcune info del collider di fin (utile per quando si accovaccia, e poi si rialza)
        prec_collider_center = collider_fin.center.y;
        prec_collider_height = collider_fin.height;

        //Recupero gameobjects
        torcia_imp = GameObject.Find("la Torcia (Impugnata)");
        pistola_imp = GameObject.Find("P226 (Impugnata)");
        MainCamera = GameObject.Find("Camera");
        hudsystem = GetComponent<HUDSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        //Ricavo l'angolo di rotazione orizzontale in base allo spostamento del mouse
        Spostamento_X += Velocità_X * Input.GetAxis("Mouse X");

        //Aggiorno la rotazione orizzontale del giocatore in base allo spostamento orizzontale del mouse
        transform.eulerAngles=(new Vector3(0.0f, Spostamento_X, 0.0f));

        //Eseguo le animazione durante ogni frame
        PlayAnimation();

        //Aggiorno lo stato delle armi/hud in base a quella equipaggiata 
        SetActive();

    }


    public void LateUpdate(){

        ///////////////////
        //SALTO

        float jump = Input.GetAxis("Jump");
        float jumpSpeed=5f;

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
           
            
            Vector3 jumpVector = new Vector3(0.0f, jump * jumpSpeed, 0.0f);
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }


        /////////////////
        //ROTAZIONE VERTICALE DI BRACCIA E TESTA

        RotazioneVerticale(GameObject.FindGameObjectWithTag("Testa").transform);

       if (animator.GetBool("Torch") || animator.GetBool("Pistol")) RotazioneVerticale(GameObject.FindGameObjectWithTag("Braccio_dx").transform);
       if (animator.GetBool("Pistol") && animator.GetBool("isAiming")) RotazioneVerticale(GameObject.FindGameObjectWithTag("Braccio_sx").transform); 
        

    }

    //ATTIVAZIONE ARMI O ELEMENTI DELL'HUD
    public void SetActive()
    {
        if (pistola_imp == null) Debug.Log("Pistol null");
        if (torcia_imp == null) Debug.Log("Torcia null");

        //TORCIA
        if (!animator.GetBool("Torch") && torcia_imp != null) torcia_imp.SetActive(false);
        else if(torcia_imp != null) torcia_imp.SetActive(true);

        //PISTOLA
        if (!animator.GetBool("Pistol") && pistola_imp != null) pistola_imp.SetActive(false);
        else if(pistola_imp != null) pistola_imp.SetActive(true);

    }

    //ROTAZIONE VERTICALE DELLA TESTA E DELLE BRACCIA
    public void RotazioneVerticale(Transform parte_corpo)
    {
        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));  

        posX += Spostamento_Y;

        if (posX < -45.0f) posX = -45.0f;
        if (posX > 60.0f) posX = 60.0f;

        //Offset per le braccia (quando hai la pistola)
        if (animator.GetBool("Pistol") && !parte_corpo.gameObject.CompareTag("Testa") && animator.GetCurrentAnimatorStateInfo(1).IsName("Aim"))
        {
            if (parte_corpo.gameObject.CompareTag("Braccio_dx")) parte_corpo.localEulerAngles = new Vector3(posX + pos_dsx_up, GameObject.FindGameObjectWithTag("Player").transform.rotation.y + pos_dx, 0f);
            if (parte_corpo.gameObject.CompareTag("Braccio_sx")) parte_corpo.localEulerAngles = new Vector3(posX + pos_dsx_up, GameObject.FindGameObjectWithTag("Player").transform.rotation.y + pos_sx, 0f);
        }
        else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Torch Crouch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch Pistol")) && !(parte_corpo.gameObject.CompareTag("Testa") &&
                  !(animator.GetCurrentAnimatorStateInfo(1).IsName("Aim"))))
        {
            parte_corpo.localEulerAngles = new Vector3(posX - pos_dx_down, GameObject.FindGameObjectWithTag("Player").transform.rotation.y, 0f);
            torcia_imp.transform.localEulerAngles = torcia_end_angles;
        }
        else
        {
            parte_corpo.localEulerAngles = new Vector3(posX, GameObject.FindGameObjectWithTag("Player").transform.rotation.y, 0f);
            torcia_imp.transform.localEulerAngles = torcia_start_angles;
        }

    }

    //GESTIONE ANIMAZIONI
    private void PlayAnimation() {

        //Gestione della camminata dritta e di lato
        animator.SetFloat("isWalking", Input.GetAxis("Vertical"));
        animator.SetFloat("isTurning", Input.GetAxis("Horizontal"));

        //Se ha raccolto la torcia, non è più senza armi, altrimenti lo è
        if (animator.GetBool("Torch")) animator.SetBool("WeaponLess", false);
 
        //Se avanza, riproduce il suo dei passi
        if ((Input.GetAxis("Vertical") != 0.0) || (Input.GetAxis("Horizontal") != 0.0))
        {
            hudsystem.movingState(true);
            PlayFootStepAudio();
        }
        else
        {
            hudsystem.movingState(false);
        }

        //Se prende un oggetto, allora aggiorno isTaken
        if (Input.GetButton("Pickup"))
        {
            animator.SetBool("isTaken", true);
        }
        else
        {
            //Di default non sta prendendo niente
            animator.SetBool("isTaken", false);
        }

        //Se corre
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
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
            collider_fin.center = new Vector3(0f, 0.75f, 0f);
            collider_fin.height = 1.45f;
        }
        else
        {
            collider_fin.center = new Vector3(0f, prec_collider_center, 0f);
            collider_fin.height = prec_collider_height;

            //Di default non è accovacciato
            animator.SetBool("isCrouching", false);
        }
        
        //MIRA
        //Se preme il tasto dx del mouse, se non ho la torcia e se non sto correndo
        if (Input.GetButton("Aim") && !animator.GetBool("Torch") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Run Pistol"))
        {
            animator.SetBool("isAiming", true);
            hudsystem.hudReticle(false);

            //Non setta la posizione
            pistola_imp.transform.localPosition = pistol_end_pos;
            pistola_imp.transform.localEulerAngles = pistol_end_angles;
            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime * 5);

        }
        else
        {
            animator.SetBool("isAiming", false);
            pistola_imp.transform.localPosition = pistol_start_pos;
            pistola_imp.transform.localEulerAngles = pistol_start_angles;
            if (animator.GetBool("Pistol") && !animator.GetBool("isRunning")) hudsystem.hudReticle(true);
            else hudsystem.hudReticle(false);
            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime * 5);

        }


    }

    //Metodi per gestire i suoni del giocatore

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

   
    //Metodi per capire quando il giocatore sta "con i piedi per terra"

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
