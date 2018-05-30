using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Misc : MonoBehaviour
{
    /// <summary>
    /// author: feduss
    /// </summary>
    private InventorySystem inventario;
    public bool crouching = false;
    private Vector3 crouchPosition = new Vector3();
    private Vector3 standPosition = new Vector3();
    private Vector3 deathposition = new Vector3();

    private CharacterController cc;

    private HUDSystem hud;

    private bool onetime = true;

    private GameObject ascia_imp;
    private GameObject pistola_imp;
    private GameObject smg_imp;

    // Use this for initialization
    void Start()
    {
        print(Time.timeScale);
        //Assegnazione variabili
        inventario = GetComponent<InventorySystem>();
        smg_imp = GameObject.Find("MP5 (Impugnato)");
        cc = GetComponent<CharacterController>();
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
        pistola_imp = GameObject.Find("P226 (Impugnata)");
        ascia_imp = GameObject.Find("l'ascia (Impugnata)");
        smg_imp = GameObject.Find("MP5 (Impugnato)");
    }

    // Update is called once per frame
    void Update()
    {
        useMedkit(); //
        crouch();

        //con la gestatus verifico che il player sia morta...se lo è, effettuo tutta una serie di procedure per la sua morte (una sola volta - onetime)
        if (inventario.getStatus() && onetime) {

            onetime = false;
            death();            
        }

        

    }

    public void useMedkit()
    {
        if (Input.GetKeyDown("k"))
        {
            inventario.useMedKit();
        }
    }

    //Funzione invocata durante l'animazione di ricarica delle armi, per far corrispondere l'aggiornamento delle munizioni al momento quasi esatto di inserimento del caricatore
    public void reload(int index)
    {

        inventario.reloadWeapon(index);
    }

    //Funzione chiamata all'inizio dell'animazione di sparo dell'mp5...funge da intermediaria, perchè le funzioni richiamabili durante un'animazione sono solo quelle presenti nelle script di ciò che viene animato,
    //cioè, in questo caso, le braccia  
    public void event_shot_smg()
    {
        inventario.shot(2); //Scalo le munizioni
        smg_imp.GetComponent<WeaponScript>().RaycastShot();
    }

    public void crouch()
    {

        //Mi accovaccio e sto accovacciato tenendo premuto left ctrl
        if (Input.GetButton("Crouch"))
        {
            
            //Se non è accovacciato
            if (!crouching)
            {
                //La posizione da accovacciato è quella attuale, con la y modificata
                crouchPosition = transform.position;     
                crouchPosition.y -= 1.6f;

                crouching = true;

                //Faccio un lerp tra la posizione in piedi e quella da accovacciato
                transform.position = Vector3.Lerp(transform.position, crouchPosition, Time.deltaTime * 3);

                //Adatto il collider
                cc.height = 66;
                Vector3 center=cc.center;
                center.y = 46;
                cc.center = center;
            }
        }


        //Quando rilascio left ctrl
        if (Input.GetButtonUp("Crouch"))
        {

            crouching = false;

            //La posizione in piedi sarà quella quando ho rilascio left ctrl, con la y modificata (questo perchè posso muovermi da accovacciato)
            standPosition = transform.position;
            standPosition.y += 1.6f;

            //Solito lerp
            transform.position = Vector3.Lerp(transform.position, standPosition, Time.deltaTime * 3);

            //Adatto il collider
            cc.height = 100;
            Vector3 center = cc.center;
            center.y = 40;
            cc.center = center;

        }
        
    }

    //Funzione richiamata dagli oggetti raccolti
    public void supportFunction(GameObject g)
    {
        StartCoroutine(DisableAfterSomeSeconds(g));
    }

    //Coroutine che elimina l'oggetto raccolto e, dopo 2 secondi, disabilita le scritte a schermo
    IEnumerator DisableAfterSomeSeconds(GameObject g)
    {
        Destroy(g);

        yield return new WaitForSeconds(2f);

        hud.centralBoxEnabler(false);
        hud.sideBoxEnabler(false);
        
    }

    public void death()
    {
        //Calcolo la posizione di morte (che sarebbe quella attuale ma con la y modificata) ed effettuo il lerp
        deathposition = transform.position;
        deathposition.y -= 3f;
        gameObject.transform.position = Vector3.Lerp(transform.position, deathposition, Time.deltaTime*20f);
    
        //Elimino script vitali per il player (per evitare comportamenti indesiderati da morto)
        Destroy(gameObject.GetComponent<Moving>());
        Destroy(gameObject.GetComponent<flashlight>());
        Destroy(gameObject.GetComponent<SwitchWeapon>());

        //Disattivo le armi
        if(ascia_imp!=null) ascia_imp.SetActive(false);
        if(pistola_imp!=null) pistola_imp.SetActive(false);
        if(smg_imp!=null) smg_imp.SetActive(false);

        Destroy(gameObject.GetComponent<PlayAnimation>());

        //Normalizzo la rotazione del player (perchè potrebbe morire guardando in alto, per esempio, e l'animazione di morte sarebbe sbagliata)
        GameObject.FindGameObjectWithTag("Hands").transform.eulerAngles = new Vector3(-90f, transform.eulerAngles.y, 0f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        
        //Attivo l'animazione di morte
        GetComponent<Animator>().SetTrigger("Death");

        //Disattivo alcuni elementi dell'hud
        hud.deathScreenTrigger();

        StartCoroutine(afterDeath());

    }

    //Coroutine che, dopo 2 secondi dalla morte, riporta al menù principale
    IEnumerator afterDeath()
    {
        yield return new WaitForSeconds(2f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }


    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Setto snow (la variabile che indica se sei a contatto col terreno innevato) a secondo della collisione con il terreno o no
        if (hit.gameObject.CompareTag("Terreno"))
        {
            PlayAnimation.snow = true;
        }
        else
        {
            PlayAnimation.snow = false;
        }
    }

}
