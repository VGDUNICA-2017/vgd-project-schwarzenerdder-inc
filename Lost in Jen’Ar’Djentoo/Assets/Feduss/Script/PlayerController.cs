using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{


    MouseLook mouselook = new MouseLook();
    private GameObject axe;
    private InventorySystem inventario;
    private GameObject smg_imp;

    // Use this for initialization
    void Start()
    {
        //axe = GameObject.Find("l'ascia (Impugnata)");
        mouselook.lockCursor = true;
        Cursor.visible = false;
        inventario = GetComponent<InventorySystem>();
        smg_imp = GameObject.Find("MP5 (Impugnato)");
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown("k"))
        {
            useMedkit();
        }
    }

    public void useMedkit()
    {
        inventario.useMedKit();
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

}
