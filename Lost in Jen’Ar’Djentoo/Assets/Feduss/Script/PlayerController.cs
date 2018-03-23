using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variabili per l'animazione
    private Animator animator;
    private Animator enemy;

    //Spostamento giocatore
    public float Velocità_X = 2.0f; //Velocità di spostamento orizzontale
    public float Velocità_Y = 1.0f; //Velocità di spostamento verticale
    private float Spostamento_X = 0.0f;
    private float Spostamento_Y = 0.0f;

    //Variabili spostamento rotazione braccia durante mira
    public float pos_dx_noAim=-15f; //offset per la posizione orizzontale del braccio dx quando non miri

    //Variabile dove salvo la rotazione verticale delle braccia e della testa (vedi lateupdate)
    private float posX;
    private float posY;

    //Suoni passi
    [SerializeField] public AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] public AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] public AudioClip m_LandSound;           // the sound played when character touches back on ground.
    private AudioSource m_AudioSource;


    //Variabile per capire se il giocatore non sta saltando
    private bool isOnGround=false;


    //Capsule collider di fin e variabili per salvare il suo collider (utile per quando si accovaccia)
    private CapsuleCollider collider_fin;
    private float prec_collider_center;
    private float prec_collider_height;

    public GameObject MainCamera;

    //Variabili di salvataggio oggetti/armi equipaggiati e elementi dell'hud (eventualmente da nascondere per qualche motivo)
    public GameObject torcia_imp;
    public GameObject pistola_imp;

    //Fov della camera (utile per quando si mira)
    int start_fov = 60;
    int end_fov = 35;

    //Transform della pistola (utile per quando si mira)
    Vector3 pistol_start_angles = new Vector3(78.55801f, -77.943f, -77.22501f);
    Vector3 pistol_end_angles = new Vector3(71.749f, -66.369f, -52.329f);

    Vector3 pistol_start_pos = new Vector3(0.2576f, -0.0756f, 0.1487f);
    Vector3 pistol_end_pos = new Vector3(0.275f, -0.048f, 0.183f);

    //Debug
    public bool autoaim = false;

    //Rotazione della torcia (cambia quando ti accovacci)
    private Vector3 torcia_start_angles= new Vector3(0.005f, -45f, -75f);
    private Vector3 torcia_end_angles= new Vector3(0.005f, -45f, -50f);

    private HUDSystem hudsystem;
    private InventorySystem inventory;

    private bool flag_damage = false;
    
    //flag per il possesso delle armi
    public bool getTorch=false;
    public bool getPistol = false;
    public bool getShotgun = false;
    public bool getSmg = false;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        //Recupero gameobjects

        hudsystem = GetComponent<HUDSystem>();
        inventory = GetComponent<InventorySystem>();

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

        RotazioneVerticale(MainCamera.transform);

        SwitchWeapon();

        //Kitmedico
        if (Input.GetKeyDown("k"))
        {
            inventory.useMedKit();
        }

        //serve per far si che il nemico infligga danno una sola volta durante ogni animazione di attacco (vedi l'ontriggerenter per il resto del codice)
        if (!enemy.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            flag_damage = true;
        }


        }


    public void LateUpdate(){


        /////////////////
        //ROTAZIONE VERTICALE DI BRACCIA E TESTA

        RotazioneVerticale(GameObject.FindGameObjectWithTag("Testa").transform);

        RotazioneVerticale(GameObject.FindGameObjectWithTag("Braccio_dx").transform);
        RotazioneVerticale(GameObject.FindGameObjectWithTag("Braccio_sx").transform); 
        

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

        if (animator.GetBool("Pistol"))
        {
            if (posX < -15.0f) posX = -15.0f;
            if (posX > 20.0f) posX = 20.0f;
        }
        else
        {
            if (posX < -45.0f) posX = -45.0f;
            if (posX > 60.0f) posX = 60.0f;
        }

        //Offset per le braccia (quando hai la pistola e sta mirando)
        if (animator.GetBool("WeaponLess") || (animator.GetBool("Pistol") && (animator.GetBool("isAiming") || animator.GetBool("isFiring")) || (animator.GetBool("Pistol")) && parte_corpo.name.Equals("Camera")))
        {
            parte_corpo.localEulerAngles = new Vector3(posX, GameObject.FindGameObjectWithTag("Player").transform.rotation.y, 0f);
        }

        //Aggiusto l'offset iniziale della rotazione verticale quando è accovacciato
        if (!parte_corpo.gameObject.CompareTag("Braccio_dx") && animator.GetBool("isCrouching") && !(parte_corpo.gameObject.CompareTag("Testa") && !animator.GetBool("isAiming")))
        {

            if (parte_corpo.name.Equals("MainCamera"))
            {
                parte_corpo.localEulerAngles = new Vector3(posX, GameObject.FindGameObjectWithTag("Player").transform.rotation.y, 0f);
            }
            else
            {
                parte_corpo.localEulerAngles = new Vector3(posX, GameObject.FindGameObjectWithTag("Player").transform.rotation.y + pos_dx_noAim, 0f);
            }
                torcia_imp.transform.localEulerAngles = torcia_end_angles;
        }

        //offset del braccio sx quando hai la la torcia
        if (!parte_corpo.gameObject.CompareTag("Braccio_dx") && !parte_corpo.gameObject.name.Equals("MainCamera") && animator.GetBool("Torch") && !animator.GetBool("isAiming"))
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
            GetComponent<CharacterController>().height = 3.3f;
            GetComponent<CharacterController>().center = new Vector3(0f, 1.1f, 0f);
            MainCamera.transform.localPosition = Vector3.Lerp(new Vector3(2f, 3.5f, -3f), new Vector3(2f, 3.0f, -3f), 1f);
        }
        else
        {
            //Di default non è accovacciato
            animator.SetBool("isCrouching", false);
            GetComponent<CharacterController>().height = 3.8f;
            GetComponent<CharacterController>().center = new Vector3(0f, 1.59f, 0f);
            MainCamera.transform.localPosition = Vector3.Lerp(new Vector3(2f, 3.0f, -3f), new Vector3(2f, 3.5f, -3f), 1f);
            
        }
        
        //MIRA
        //Se preme il tasto dx del mouse, se non ho la torcia e se non sto correndo
        if (autoaim || Input.GetButton("Aim") && !animator.GetBool("Torch") && !animator.GetBool("isCrouching") && !animator.GetBool("isRunning"))
        {
            animator.SetBool("isAiming", true);
            hudsystem.hudReticle(true);

            pistola_imp.transform.localPosition = pistol_end_pos;
            pistola_imp.transform.localEulerAngles = pistol_end_angles;

            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, end_fov, Time.deltaTime * 5);

        }
        else
        {
            animator.SetBool("isAiming", false);
            pistola_imp.transform.localPosition = pistol_start_pos;
            pistola_imp.transform.localEulerAngles = pistol_start_angles;

            if (animator.GetBool("Pistol") && !animator.GetBool("isRunning")) hudsystem.hudReticle(false);
            else hudsystem.hudReticle(false);

            MainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(MainCamera.GetComponent<Camera>().fieldOfView, start_fov, Time.deltaTime * 5);

        }

        //salto
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
        }

        //turn
        if (Input.GetAxis("Horizontal") > 0)
        {
            animator.SetFloat("isTurningMouse", 1);
        }

        //turn
        if (Input.GetAxis("Horizontal") < 0)
        {
            animator.SetFloat("isTurningMouse", -1);
        }


    }

    //Da completare man mano che inseriremo le altre armi
    private void SwitchWeapon()
    {

        //Torcia
        if (Input.GetKeyDown("1") && getTorch)
        {
            if(animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);

            animator.SetBool("Torch", true);
        }

        //Pistola
        if (Input.GetKeyDown("2") && getPistol)
        {
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);
            animator.SetBool("Pistol", true);
        }

        //Shotgun
        if (Input.GetKeyDown("3") && getShotgun)
        {
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Smg")) animator.SetBool("Smg", false);
            //animator.SetBool("Shotgun", true);
        }

        //Smg
        if (Input.GetKeyDown("4") && getSmg)
        {
            if (animator.GetBool("Torch")) animator.SetBool("Torch", false);
            if (animator.GetBool("Pistol")) animator.SetBool("Pistol", false);
            //if (animator.GetBool("Shotgun")) animator.SetBool("Shotgun", false);
            //animator.SetBool("Smg", true);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Braccio_sx") || other.gameObject.CompareTag("Braccio_dx"))
        {
            if (enemy.GetCurrentAnimatorStateInfo(0).IsName("Attacking") && flag_damage)
            {
                inventory.takeDamage(15);
                flag_damage = false;

                if (inventory.getStatus()) animator.SetTrigger("Death");
            }
        }

        if (other.gameObject.CompareTag("Braccio_sx") && other.gameObject.CompareTag("Braccio_dx"))
        {
            if (enemy.GetCurrentAnimatorStateInfo(0).IsName("Attacking") && flag_damage)
            {
                inventory.takeDamage(30);
                flag_damage = false;

                if (inventory.getStatus()) animator.SetTrigger("Death");
            }
        }
    }
}
