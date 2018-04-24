using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{


    MouseLook mouselook= new MouseLook();
    private GameObject axe;

    // Use this for initialization
    void Start()
    {
        axe = GameObject.Find("l'ascia (Impugnata)");
        mouselook.lockCursor=true;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if (Input.GetKeyDown("k"))
        {
            useMedkit();
        }
    }

    public void ActiveMeleeAttack()
    {
        axe.GetComponent<WeaponScript>().attack_flag = true;
    }

    public void useMedkit()
    {
        GetComponent<InventorySystem>().useMedKit();
    }
}
