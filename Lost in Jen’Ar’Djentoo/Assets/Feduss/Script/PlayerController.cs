using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject axe;

    // Use this for initialization
    void Start()
    {
        axe = GameObject.Find("l'ascia (Impugnata)");
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
    }

    public void ActiveMeleeAttack()
    {
        axe.GetComponent<WeaponScript>().attack_flag = true;
    }
}
