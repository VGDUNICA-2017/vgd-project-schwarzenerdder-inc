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
    public float pos_sx=-13;
    public float pos_dx=6f;

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

    //Fov della camera (utile per quando si mira)
    int start_fov = 90;
    int end_fov = 45;

    //Rotazione della pistola (utile per quando si mira)
    Vector3 pistol_start_angles = new Vector3(95.15399f, -101.196f, -106.771f);
    Vector3 pistol_end_angles= new Vector3(99.077f, -83.23401f, -95.02301f);

    private HUDSystem hudsystem;

    private Transform aimSpot;

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

        aimSpot = GameObject.Find("Reticle").GetComponent<Transform>();
        


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
        if (animator.GetBool("Pistol")) RotazioneVerticale(GameObject.FindGameObjectWithTag("Braccio_sx").transform); 
        

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

    //ROTAZIONE VERTICALE DELLA TESTA E DEL BRACCIO SX
    public void RotazioneVerticale(Transform parte_corpo)
    {
        Spostamento_Y = -(Velocità_Y * Input.GetAxis("Mouse Y"));  

        posX += Spostamento_Y;

        if (posX < -45.0f) posX = -45.0f;
        if (posX > 60.0f) posX = 60.0f;

        //Se sta ricaricando, blocco la rotazione verticale, altrimenti no 
        if ((animator.GetCurrentAnimatorStateInfo(1).IsName("Reload")) && !parte_corpo.gameObject.CompareTag("Testa"))
        {
            parte_corpo.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            //GameObject.Find("P6Mag").transform.SetParent(GameObject.Find("swat:RightHand").transform);
        }
        else
        {
            parte_corpo.eulerAngles = (new Vector3(posX, Spostamento_X, 0.0f));
           // GameObject.Find("P6Mag").transform.parent = GameObject.Find("P226 (Impugnata)").transform;
        }

        if (Input.GetButton("Aim") && !animator.GetBool("Torch"))
        {
            GameObject.FindGameObjectWithTag("Braccio_sx").transform.eulerAngles = new Vector3(posX, Spostamento_X+pos_sx, 0.0f);
            GameObject.FindGameObjectWithTag("Braccio_dx").transform.eulerAngles = new Vector3(posX, Spostamento_X-pos_dx, 0.0f);
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
 

        //Se preme il tasto dx del mouse e se non ho la torcia
        if (Input.GetButton("Aim") && !animator.GetBool("Torch"))
        {
            animator.SetBool("isAiming", true);

            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime*5);
            //pistola_imp.transform.localRotation = Quaternion.Euler(pistol_end_angles);
            //pistola_imp.transform.LookAt(aimSpot);
            //pistola_imp.transform.localRotation.SetLookRotation(aimSpot.rotation.eulerAngles);
        }
        else
        {
            animator.SetBool("isAiming", false);
            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime*5);
            //pistola_imp.transform.localRotation = Quaternion.Euler(pistol_start_angles);
            //pistola_imp.transform.LookAt(aimSpot);
            //pistola_imp.transform.localRotation.SetLookRotation(aimSpot.rotation.eulerAngles);
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
