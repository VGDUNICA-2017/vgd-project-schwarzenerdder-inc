using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{


    MouseLook mouselook= new MouseLook();
    private GameObject axe;
    private InventorySystem inventario;

    // Use this for initialization
    void Start()
    {
        //axe = GameObject.Find("l'ascia (Impugnata)");
        mouselook.lockCursor=true;
        Cursor.visible = false;
        inventario = GetComponent<InventorySystem>();
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

    public void reload(int i)
    {
        print("bello st'o isssv");
        inventario.reloadWeapon(i);
    }
}
