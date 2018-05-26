using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Misc : MonoBehaviour
{
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
        //axe = GameObject.Find("l'ascia (Impugnata)");
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
        useMedkit();
        crouch();
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

    public void supportFunction(GameObject g)
    {
        StartCoroutine(DisableAfterSomeSeconds(g));
    }

    IEnumerator DisableAfterSomeSeconds(GameObject g)
    {
        Destroy(g);

        yield return new WaitForSeconds(2f);

        hud.centralBoxEnabler(false);
        hud.sideBoxEnabler(false);
        
    }

    public void death()
    {
        print("livep: " + transform.position);
        deathposition = transform.position;
        deathposition.y -= 3f;
        print("deathp: " + deathposition);
        gameObject.transform.position = Vector3.Lerp(transform.position, deathposition, Time.deltaTime*20f);

        Destroy(gameObject.GetComponent<Moving>());
        Destroy(gameObject.GetComponent<flashlight>());
        Destroy(gameObject.GetComponent<SwitchWeapon>());

        if(ascia_imp!=null) ascia_imp.SetActive(false);
        if(pistola_imp!=null) pistola_imp.SetActive(false);
        if(smg_imp!=null) smg_imp.SetActive(false);

        Destroy(gameObject.GetComponent<PlayAnimation>());

        GameObject.FindGameObjectWithTag("Hands").transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles = new Vector3(0f, 0f, 0f);
        
        GetComponent<Animator>().SetTrigger("Death");

        hud.deathScreenTrigger();

        StartCoroutine(afterDeath());

    }

    IEnumerator afterDeath()
    {
        yield return new WaitForSeconds(2f);

        GetComponent<Load>().Load_(SceneManager.GetActiveScene().name);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        //Setto snow (la variabile che indica se sei a contatto col terreno innevato) a secondo della collisione con il terreno o no
        print(hit.gameObject.tag);
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
