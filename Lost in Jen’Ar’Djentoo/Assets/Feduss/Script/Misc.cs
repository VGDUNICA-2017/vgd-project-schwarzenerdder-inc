using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Misc : MonoBehaviour
{
    private GameObject axe;
    private InventorySystem inventario;
    private GameObject smg_imp;

    public bool crouching = false;
    private Vector3 crouchPosition = new Vector3();
    private Vector3 standPosition = new Vector3();

    private CharacterController cc;

    private HUDSystem hud;

    // Use this for initialization
    void Start()
    {
        //axe = GameObject.Find("l'ascia (Impugnata)");
        inventario = GetComponent<InventorySystem>();
        smg_imp = GameObject.Find("MP5 (Impugnato)");
        cc = GetComponent<CharacterController>();
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        useMedkit();
        crouch();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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

}
